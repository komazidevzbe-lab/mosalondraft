using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class BookingService(DataContext context) : IBookingService
{
    private const decimal FixedDepositAmount = 250m;

    public async Task<BookingReviewDto> CreatePendingBookingAsync(CreateBookingDto createBookingDto, int userId)
    {
        ValidateBasicBookingRequest(createBookingDto);

        var bookingMode = createBookingDto.BookingMode.Trim().ToLowerInvariant();

        var booking = new Booking
        {
            BookingReference = GenerateBookingReference(),
            UserId = userId,
            BookingMode = bookingMode,
            ClientFullName = createBookingDto.ClientFullName.Trim(),
            ClientEmailAddress = createBookingDto.ClientEmailAddress.Trim().ToLowerInvariant(),
            ClientPhoneNumber = createBookingDto.ClientPhoneNumber.Trim(),
            PreferredContactMethod = createBookingDto.PreferredContactMethod.Trim().ToLowerInvariant(),
            BookingStatus = "PendingPayment",
            PaymentStatus = "PendingPayment",
            TotalDurationMinutes = 0,
            TotalAmount = 0,
            DepositAmount = 0,
            BalanceAmount = 0,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var itemDto in createBookingDto.Items)
        {
            var service = await context.SalonServices
                .Include(salonService => salonService.ServiceTypes)
                .Include(salonService => salonService.LengthOptions)
                .SingleOrDefaultAsync(salonService =>
                    salonService.Id == itemDto.SalonServiceId &&
                    salonService.IsActive);

            if (service == null)
            {
                throw new ArgumentException($"Invalid service selected: {itemDto.SalonServiceId}.");
            }

            var selectedServiceType = service.ServiceTypes
                .SingleOrDefault(type =>
                    type.Id == itemDto.SalonServiceTypeId &&
                    type.IsActive);

            if (selectedServiceType == null)
            {
                throw new ArgumentException($"Invalid service type selected for {service.Name}.");
            }

            var activeLengthOptions = service.LengthOptions
                .Where(length => length.IsActive)
                .ToList();

            var serviceRequiresLength = activeLengthOptions.Count > 0;

            SalonServiceLengthOption? selectedLength = null;

            if (serviceRequiresLength && itemDto.SalonServiceLengthOptionId == null)
            {
                throw new ArgumentException($"{service.Name} requires a length option.");
            }

            if (!serviceRequiresLength && itemDto.SalonServiceLengthOptionId != null)
            {
                throw new ArgumentException($"{service.Name} does not allow a length option.");
            }

            if (itemDto.SalonServiceLengthOptionId != null)
            {
                selectedLength = activeLengthOptions.SingleOrDefault(length =>
                    length.Id == itemDto.SalonServiceLengthOptionId.Value);

                if (selectedLength == null)
                {
                    throw new ArgumentException($"Invalid length option selected for {service.Name}.");
                }
            }

            if (itemDto.AppointmentDate < DateOnly.FromDateTime(DateTime.UtcNow.Date))
            {
                throw new ArgumentException("Appointment date cannot be in the past.");
            }

            var endTime = itemDto.StartTime.AddMinutes(service.DurationMinutes);

            if (itemDto.GalleryImageId != null)
            {
                var favoriteExists = await context.GalleryImageFavorites
                    .AnyAsync(favorite =>
                        favorite.UserId == userId &&
                        favorite.GalleryImageId == itemDto.GalleryImageId.Value &&
                        favorite.GalleryImage.IsActive);

                if (!favoriteExists)
                {
                    throw new ArgumentException("Selected gallery reference image is not in your favourites.");
                }
            }

            var lengthAddOnPrice = selectedLength?.PriceAddOn ?? 0m;
            var finalItemPrice = service.BasePrice + lengthAddOnPrice;

            var bookingItem = new BookingServiceItem
            {
                SalonServiceId = service.Id,
                SalonServiceTypeId = selectedServiceType.Id,
                SalonServiceLengthOptionId = selectedLength?.Id,
                AppointmentDate = itemDto.AppointmentDate,
                StartTime = itemDto.StartTime,
                EndTime = endTime,
                ServiceNameSnapshot = service.Name,
                ServiceTypeNameSnapshot = selectedServiceType.Name,
                LengthNameSnapshot = selectedLength?.Name,
                BasePriceSnapshot = service.BasePrice,
                LengthAddOnPriceSnapshot = lengthAddOnPrice,
                FinalPrice = finalItemPrice,
                DurationMinutesSnapshot = service.DurationMinutes,
                Notes = string.IsNullOrWhiteSpace(itemDto.Notes) ? null : itemDto.Notes.Trim(),
                ReferenceImageType = GetReferenceImageType(itemDto),
                GalleryImageId = itemDto.GalleryImageId,
                UploadedReferenceImageUrl = itemDto.UploadedReferenceImageUrl
            };

            booking.Items.Add(bookingItem);
        }

        booking.TotalDurationMinutes = booking.Items.Sum(item => item.DurationMinutesSnapshot);
        booking.TotalAmount = booking.Items.Sum(item => item.FinalPrice);
        booking.DepositAmount = Math.Min(FixedDepositAmount, booking.TotalAmount);
        booking.BalanceAmount = booking.TotalAmount - booking.DepositAmount;

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        return await GetBookingReviewOrThrow(booking.Id, userId);
    }

    public async Task<BookingReviewDto?> GetBookingReviewAsync(int bookingId, int userId)
    {
        return await GetBookingReviewQuery(bookingId, userId)
            .SingleOrDefaultAsync();
    }

    public async Task<BookingReviewDto?> GetConfirmedBookingAsync(int bookingId, int userId)
    {
        return await GetBookingReviewQuery(bookingId, userId)
            .Where(booking => booking.BookingStatus == "Confirmed")
            .SingleOrDefaultAsync();
    }

    private async Task<BookingReviewDto> GetBookingReviewOrThrow(int bookingId, int userId)
    {
        var booking = await GetBookingReviewAsync(bookingId, userId);

        if (booking == null)
        {
            throw new KeyNotFoundException("Booking not found.");
        }

        return booking;
    }

    private IQueryable<BookingReviewDto> GetBookingReviewQuery(int bookingId, int userId)
    {
        return context.Bookings
            .AsNoTracking()
            .Where(booking => booking.Id == bookingId && booking.UserId == userId)
            .Select(booking => new BookingReviewDto
            {
                BookingId = booking.Id,
                BookingReference = booking.BookingReference,
                BookingMode = booking.BookingMode,
                BookingStatus = booking.BookingStatus,
                PaymentStatus = booking.PaymentStatus,
                ClientFullName = booking.ClientFullName,
                ClientEmailAddress = booking.ClientEmailAddress,
                ClientPhoneNumber = booking.ClientPhoneNumber,
                PreferredContactMethod = booking.PreferredContactMethod,
                TotalDurationMinutes = booking.TotalDurationMinutes,
                TotalAmount = booking.TotalAmount,
                DepositAmount = booking.DepositAmount,
                BalanceAmount = booking.BalanceAmount,
                Items = booking.Items
                    .OrderBy(item => item.AppointmentDate)
                    .ThenBy(item => item.StartTime)
                    .Select(item => new BookingReviewItemDto
                    {
                        Id = item.Id,
                        SalonServiceId = item.SalonServiceId,
                        ServiceName = item.ServiceNameSnapshot,
                        ServiceTypeName = item.ServiceTypeNameSnapshot,
                        LengthName = item.LengthNameSnapshot,
                        AppointmentDate = item.AppointmentDate,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        DurationMinutes = item.DurationMinutesSnapshot,
                        BasePrice = item.BasePriceSnapshot,
                        LengthAddOnPrice = item.LengthAddOnPriceSnapshot,
                        FinalPrice = item.FinalPrice,
                        Notes = item.Notes,
                        ReferenceImageUrl = item.GalleryImage != null
                            ? item.GalleryImage.ImageUrl
                            : item.UploadedReferenceImageUrl
                    })
                    .ToList()
            });
    }

    private static void ValidateBasicBookingRequest(CreateBookingDto createBookingDto)
    {
        var bookingMode = createBookingDto.BookingMode.Trim().ToLowerInvariant();

        if (bookingMode is not "combined" and not "separate")
        {
            throw new ArgumentException("Booking mode must be combined or separate.");
        }

        if (createBookingDto.Items.Count == 0)
        {
            throw new ArgumentException("At least one service must be selected.");
        }

        if (string.IsNullOrWhiteSpace(createBookingDto.ClientFullName))
        {
            throw new ArgumentException("Client full name is required.");
        }

        if (string.IsNullOrWhiteSpace(createBookingDto.ClientEmailAddress))
        {
            throw new ArgumentException("Client email address is required.");
        }

        if (string.IsNullOrWhiteSpace(createBookingDto.ClientPhoneNumber))
        {
            throw new ArgumentException("Client phone number is required.");
        }

        if (string.IsNullOrWhiteSpace(createBookingDto.PreferredContactMethod))
        {
            throw new ArgumentException("Preferred contact method is required.");
        }

        var contactMethod = createBookingDto.PreferredContactMethod.Trim().ToLowerInvariant();

        if (contactMethod is not "whatsapp" and not "phone" and not "email")
        {
            throw new ArgumentException("Preferred contact method must be WhatsApp, phone, or email.");
        }
    }

    private static string GenerateBookingReference()
    {
        return $"BK-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }

    private static string? GetReferenceImageType(CreateBookingItemDto itemDto)
    {
        if (itemDto.GalleryImageId != null)
        {
            return "Gallery";
        }

        if (!string.IsNullOrWhiteSpace(itemDto.UploadedReferenceImageUrl))
        {
            return "Upload";
        }

        return null;
    }
}
using API.DTOs;

namespace API.Interfaces;

public interface IBookingService
{
    Task<BookingReviewDto> CreatePendingBookingAsync(CreateBookingDto createBookingDto, int userId);

    Task<BookingReviewDto?> GetBookingReviewAsync(int bookingId, int userId);

    Task<BookingReviewDto?> GetConfirmedBookingAsync(int bookingId, int userId);
}
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ContactService(
    DataContext context,
    UserManager<AppUser> userManager,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor
) : IContactService
{
    private const long MaxReviewImageSizeInBytes = 3 * 1024 * 1024;
    private const string DefaultReviewImageUrl = "assets/contact/reviewer.png";
    private const string DefaultReviewLocation = "Client";

    private static readonly string[] AllowedInterests =
    [
        "Manicure",
        "Pedicure",
        "Makeup",
        "Brows & Lashes",
        "Booking Question",
        "Collaboration"
    ];

    private static readonly string[] AllowedImageContentTypes =
    [
        "image/jpeg",
        "image/jpg",
        "image/png",
        "image/webp"
    ];

    public async Task<ContactMessageDto> CreateContactMessageAsync(
        CreateContactMessageDto createContactMessageDto,
        ClaimsPrincipal user)
    {
        var fullName = createContactMessageDto.FullName.Trim();
        var emailAddress = createContactMessageDto.EmailAddress.Trim().ToLowerInvariant();
        var phoneNumber = NormalisePhoneNumber(createContactMessageDto.PhoneNumber);
        var interest = createContactMessageDto.Interest.Trim();
        var message = createContactMessageDto.Message.Trim();

        if (!AllowedInterests.Contains(interest))
        {
            throw new ArgumentException("Please select a valid interest option.");
        }

        var now = DateTime.UtcNow;

        var contactMessage = new ContactMessage
        {
            FullName = fullName,
            EmailAddress = emailAddress,
            PhoneNumber = phoneNumber,
            Interest = interest,
            Message = message,
            UserId = GetAuthenticatedUserId(user),
            MessageStatus = ContactMessageStatus.Sent,
            SubmittedAt = now,
            CreatedAt = now
        };

        context.ContactMessages.Add(contactMessage);
        await context.SaveChangesAsync();

        return MapContactMessageToDto(contactMessage);
    }

    public async Task<List<ContactMessageDto>> GetMyContactMessagesAsync(ClaimsPrincipal user)
    {
        var userId = GetAuthenticatedUserId(user);

        if (userId == null)
        {
            return [];
        }

        var messages = await context.ContactMessages
            .AsNoTracking()
            .Where(message => message.UserId == userId)
            .OrderByDescending(message => message.SubmittedAt)
            .ToListAsync();

        return messages.Select(MapContactMessageToDto).ToList();
    }

    public async Task<ClientReviewDto> CreateClientReviewAsync(
        CreateClientReviewDto createClientReviewDto,
        ClaimsPrincipal user)
    {
        var appUser = await GetAuthenticatedUser(user);

        if (appUser == null)
        {
            throw new ArgumentException("You must be signed in to submit a review.");
        }

        var clientName = $"{appUser.FirstName} {appUser.LastName}".Trim();

        if (string.IsNullOrWhiteSpace(clientName))
        {
            throw new ArgumentException("Your account profile is missing your name.");
        }

        var reviewText = createClientReviewDto.ReviewText.Trim();

        if (string.IsNullOrWhiteSpace(reviewText))
        {
            throw new ArgumentException("Review text is required.");
        }

        var location = string.IsNullOrWhiteSpace(createClientReviewDto.Location)
            ? DefaultReviewLocation
            : createClientReviewDto.Location.Trim();

        var imageUrl = await SaveReviewImage(createClientReviewDto.Image);
        var nextDisplayOrder = await GetNextReviewDisplayOrder();
        var now = DateTime.UtcNow;

        var clientReview = new ClientReview
        {
            UserId = appUser.Id,
            ClientName = clientName,
            Location = location,
            ReviewText = reviewText,
            Rating = createClientReviewDto.Rating,
            ImageUrl = imageUrl,
            AltText = $"{clientName} review profile image",
            IsApproved = false,
            IsFeatured = false,
            DisplayOrder = nextDisplayOrder,
            CreatedAt = now
        };

        context.ClientReviews.Add(clientReview);
        await context.SaveChangesAsync();

        return MapClientReviewToDto(clientReview);
    }

    private async Task<string> SaveReviewImage(IFormFile? image)
    {
        if (image == null || image.Length == 0)
        {
            return DefaultReviewImageUrl;
        }

        if (!AllowedImageContentTypes.Contains(image.ContentType.ToLowerInvariant()))
        {
            throw new ArgumentException("Invalid image file type. Please upload a JPG, PNG, or WEBP image.");
        }

        if (image.Length > MaxReviewImageSizeInBytes)
        {
            throw new ArgumentException("Image file is too large. Please upload an image smaller than 3MB.");
        }

        var webRootPath = environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(environment.ContentRootPath, "wwwroot");
        }

        var uploadFolder = Path.Combine(webRootPath, "uploads", "reviews");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        return BuildAbsoluteUploadUrl(fileName);
    }

    private string BuildAbsoluteUploadUrl(string fileName)
    {
        var request = httpContextAccessor.HttpContext?.Request;

        if (request == null)
        {
            return $"/uploads/reviews/{fileName}";
        }

        return $"{request.Scheme}://{request.Host}/uploads/reviews/{fileName}";
    }

    private async Task<int> GetNextReviewDisplayOrder()
    {
        var hasReviews = await context.ClientReviews.AnyAsync();

        if (!hasReviews)
        {
            return 1;
        }

        return await context.ClientReviews.MaxAsync(review => review.DisplayOrder) + 1;
    }

    private async Task<AppUser?> GetAuthenticatedUser(ClaimsPrincipal user)
    {
        var userId = GetAuthenticatedUserId(user);

        if (userId == null)
        {
            return null;
        }

        return await userManager.Users
            .SingleOrDefaultAsync(appUser => appUser.Id == userId.Value);
    }

    private static int? GetAuthenticatedUserId(ClaimsPrincipal user)
    {
        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdValue))
        {
            return null;
        }

        return int.TryParse(userIdValue, out var userId) ? userId : null;
    }

    private static string NormalisePhoneNumber(string phoneNumber)
    {
        return phoneNumber.Trim().Replace(" ", string.Empty);
    }

    private static ContactMessageDto MapContactMessageToDto(ContactMessage contactMessage)
    {
        return new ContactMessageDto
        {
            Id = contactMessage.Id,
            FullName = contactMessage.FullName,
            EmailAddress = contactMessage.EmailAddress,
            PhoneNumber = contactMessage.PhoneNumber,
            Interest = contactMessage.Interest,
            Message = contactMessage.Message,
            MessageStatus = contactMessage.MessageStatus,
            Statuses = GetStatusFlow(contactMessage.MessageStatus),
            AdminResponse = contactMessage.AdminResponse,
            RespondedAt = contactMessage.RespondedAt,
            SubmittedAt = contactMessage.SubmittedAt
        };
    }

    private static ClientReviewDto MapClientReviewToDto(ClientReview clientReview)
    {
        return new ClientReviewDto
        {
            Id = clientReview.Id,
            ClientName = clientReview.ClientName,
            Location = clientReview.Location,
            ReviewText = clientReview.ReviewText,
            Rating = clientReview.Rating,
            ImageUrl = clientReview.ImageUrl,
            AltText = clientReview.AltText,
            IsApproved = clientReview.IsApproved,
            IsFeatured = clientReview.IsFeatured,
            DisplayOrder = clientReview.DisplayOrder,
            CreatedAt = clientReview.CreatedAt
        };
    }

    private static List<string> GetStatusFlow(string status)
    {
        return status switch
        {
            ContactMessageStatus.Seen => ["Sent", "Seen"],
            ContactMessageStatus.ResponsePending => ["Sent", "Seen", "Response pending"],
            ContactMessageStatus.Responded => ["Sent", "Seen", "Responded"],
            _ => ["Sent"]
        };
    }
}
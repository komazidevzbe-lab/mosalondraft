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

    private static readonly string[] AllowedMessageStatuses =
    [
        ContactMessageStatus.Sent,
        ContactMessageStatus.Seen,
        ContactMessageStatus.ResponsePending,
        ContactMessageStatus.Responded
    ];

    // ===============================
    // Contact messages
    // ===============================

    public async Task<ContactMessageDto> CreateContactMessageAsync(
        CreateContactMessageDto createContactMessageDto,
        ClaimsPrincipal user)
    {
        var appUser = await GetAuthenticatedUser(user);

        if (appUser == null)
        {
            throw new ArgumentException("You must be signed in to send a message.");
        }

        var interest = createContactMessageDto.Interest.Trim();
        var message = createContactMessageDto.Message.Trim();

        ValidateInterest(interest);
        ValidateMessage(message);

        var fullName = GetUserFullName(appUser);

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Your account profile is missing your name.");
        }

        if (string.IsNullOrWhiteSpace(appUser.Email))
        {
            throw new ArgumentException("Your account profile is missing your email address.");
        }

        if (string.IsNullOrWhiteSpace(appUser.PhoneNumber))
        {
            throw new ArgumentException("Your account profile is missing your phone number.");
        }

        var now = DateTime.UtcNow;

        var contactMessage = new ContactMessage
        {
            FullName = fullName,
            EmailAddress = appUser.Email.Trim().ToLowerInvariant(),
            PhoneNumber = appUser.PhoneNumber.Trim(),
            Interest = interest,
            Message = message,
            UserId = appUser.Id,
            MessageStatus = ContactMessageStatus.Sent,
            SubmittedAt = now,
            CreatedAt = now,
            UpdatedAt = null
        };

        context.ContactMessages.Add(contactMessage);
        await context.SaveChangesAsync();

        return MapContactMessageToDto(contactMessage);
    }

    public async Task<List<ContactMessageDto>> GetMyContactMessagesAsync(ClaimsPrincipal user)
    {
        var appUser = await GetAuthenticatedUser(user);

        if (appUser == null)
        {
            throw new ArgumentException("You must be signed in to view your messages.");
        }

        var messages = await context.ContactMessages
            .AsNoTracking()
            .Where(message => message.UserId == appUser.Id)
            .OrderByDescending(message => message.SubmittedAt)
            .ToListAsync();

        return messages
            .Select(MapContactMessageToDto)
            .ToList();
    }

    public async Task<List<ContactMessageDto>> GetAllContactMessagesAsync()
    {
        var messages = await context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(message => message.SubmittedAt)
            .ToListAsync();

        return messages
            .Select(MapContactMessageToDto)
            .ToList();
    }

    public async Task<ContactMessageDto?> UpdateContactMessageStatusAsync(
        int messageId,
        UpdateContactMessageStatusDto updateContactMessageStatusDto)
    {
        var status = updateContactMessageStatusDto.MessageStatus.Trim();

        ValidateMessageStatus(status);

        var contactMessage = await context.ContactMessages
            .SingleOrDefaultAsync(message => message.Id == messageId);

        if (contactMessage == null)
        {
            return null;
        }

        contactMessage.MessageStatus = status;
        contactMessage.AdminResponse = string.IsNullOrWhiteSpace(updateContactMessageStatusDto.AdminResponse)
            ? null
            : updateContactMessageStatusDto.AdminResponse.Trim();
        contactMessage.UpdatedAt = DateTime.UtcNow;

        if (status == ContactMessageStatus.Responded)
        {
            contactMessage.RespondedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        return MapContactMessageToDto(contactMessage);
    }

    // ===============================
    // Client reviews
    // ===============================

    public async Task<ClientReviewDto> CreateClientReviewAsync(
        CreateClientReviewDto createClientReviewDto,
        ClaimsPrincipal user)
    {
        var appUser = await GetAuthenticatedUser(user);

        if (appUser == null)
        {
            throw new ArgumentException("You must be signed in to submit a review.");
        }

        var clientName = GetUserFullName(appUser);
        var location = createClientReviewDto.Location.Trim();
        var reviewText = createClientReviewDto.ReviewText.Trim();

        if (string.IsNullOrWhiteSpace(clientName))
        {
            throw new ArgumentException("Your account profile is missing your name.");
        }

        ValidateLocation(location);
        ValidateReviewText(reviewText);
        ValidateRating(createClientReviewDto.Rating);

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
            CreatedAt = now,
            UpdatedAt = null
        };

        context.ClientReviews.Add(clientReview);
        await context.SaveChangesAsync();

        return MapClientReviewToDto(clientReview);
    }

    public async Task<List<ClientReviewDto>> GetAllClientReviewsAsync()
    {
        var reviews = await context.ClientReviews
            .AsNoTracking()
            .OrderByDescending(review => review.CreatedAt)
            .ToListAsync();

        return reviews
            .Select(MapClientReviewToDto)
            .ToList();
    }

    public async Task<List<ClientReviewDto>> GetPendingClientReviewsAsync()
    {
        var reviews = await context.ClientReviews
            .AsNoTracking()
            .Where(review => !review.IsApproved)
            .OrderByDescending(review => review.CreatedAt)
            .ToListAsync();

        return reviews
            .Select(MapClientReviewToDto)
            .ToList();
    }

    public async Task<List<ClientReviewDto>> GetApprovedClientReviewsAsync()
    {
        var reviews = await context.ClientReviews
            .AsNoTracking()
            .Where(review => review.IsApproved)
            .OrderBy(review => review.DisplayOrder)
            .ToListAsync();

        return reviews
            .Select(MapClientReviewToDto)
            .ToList();
    }

    public async Task<ClientReviewDto?> UpdateClientReviewApprovalAsync(
        int reviewId,
        UpdateClientReviewApprovalDto updateClientReviewApprovalDto)
    {
        var clientReview = await context.ClientReviews
            .SingleOrDefaultAsync(review => review.Id == reviewId);

        if (clientReview == null)
        {
            return null;
        }

        clientReview.IsApproved = updateClientReviewApprovalDto.IsApproved;
        clientReview.IsFeatured = updateClientReviewApprovalDto.IsFeatured;
        clientReview.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return MapClientReviewToDto(clientReview);
    }

    // ===============================
    // Image upload support
    // ===============================

    private async Task<string> SaveReviewImage(IFormFile? image)
    {
        if (image == null || image.Length == 0)
        {
            return DefaultReviewImageUrl;
        }

        ValidateReviewImage(image);

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

    // ===============================
    // Validation helpers
    // ===============================

    private static void ValidateInterest(string interest)
    {
        if (string.IsNullOrWhiteSpace(interest))
        {
            throw new ArgumentException("Interest is required.");
        }

        if (!AllowedInterests.Contains(interest))
        {
            throw new ArgumentException("Please select a valid interest option.");
        }
    }

    private static void ValidateMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message is required.");
        }

        if (message.Length > 1000)
        {
            throw new ArgumentException("Message cannot be longer than 1000 characters.");
        }
    }

    private static void ValidateMessageStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Message status is required.");
        }

        if (!AllowedMessageStatuses.Contains(status))
        {
            throw new ArgumentException("Please select a valid message status.");
        }
    }

    private static void ValidateLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location is required.");
        }

        if (location.Length > 160)
        {
            throw new ArgumentException("Location cannot be longer than 160 characters.");
        }
    }

    private static void ValidateReviewText(string reviewText)
    {
        if (string.IsNullOrWhiteSpace(reviewText))
        {
            throw new ArgumentException("Review text is required.");
        }

        if (reviewText.Length > 1000)
        {
            throw new ArgumentException("Review text cannot be longer than 1000 characters.");
        }
    }

    private static void ValidateRating(int rating)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.");
        }
    }

    private static void ValidateReviewImage(IFormFile image)
    {
        if (!AllowedImageContentTypes.Contains(image.ContentType.ToLowerInvariant()))
        {
            throw new ArgumentException("Invalid image file type. Please upload a JPG, PNG, or WEBP image.");
        }

        if (image.Length > MaxReviewImageSizeInBytes)
        {
            throw new ArgumentException("Image file is too large. Please upload an image smaller than 3MB.");
        }
    }

    // ===============================
    // Shared helpers
    // ===============================

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
        var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrWhiteSpace(userIdValue) && int.TryParse(userIdValue, out var userId))
        {
            return await userManager.Users
                .SingleOrDefaultAsync(appUser => appUser.Id == userId);
        }

        var email = user.Identity?.Name;

        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        return await userManager.Users
            .SingleOrDefaultAsync(appUser => appUser.NormalizedEmail == email.ToUpperInvariant());
    }

    private static string GetUserFullName(AppUser appUser)
    {
        return $"{appUser.FirstName} {appUser.LastName}".Trim();
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
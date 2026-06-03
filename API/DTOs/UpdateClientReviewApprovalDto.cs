namespace API.DTOs;

public class UpdateClientReviewApprovalDto
{
    // ===============================
    // Admin review approval data
    // Controls whether a review is visible and featured.
    // ===============================

    public bool IsApproved { get; set; }

    public bool IsFeatured { get; set; }
}
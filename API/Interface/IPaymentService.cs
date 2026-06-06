using API.DTOs;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces;

public interface IPaymentService
{
    Task<PaymentInitiationResponseDto> InitiateDepositPaymentAsync(int bookingId, int userId);

    Task<bool> MarkPaymentCancelledAsync(int bookingId, int userId);

    Task<bool> ProcessPayFastNotificationAsync(IFormCollection payFastForm);
}
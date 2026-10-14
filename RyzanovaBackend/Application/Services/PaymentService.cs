using Application.DTOs;
using Application.Exceptions;
using Domain.Services;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class PaymentService
{
    private const string WfpApproved = "Approved";
    private static readonly string[] WfpFailedStatuses = ["Declined", "Expired"];

    private readonly IPaymentRepository _paymentRepository;
    private readonly IThemeRepository _themeRepository;
    private readonly IWayForPayClient _wfp;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IThemeRepository themeRepository,
        IWayForPayClient wfp,
        ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _themeRepository = themeRepository;
        _wfp = wfp;
        _logger = logger;
    }

    public async Task<PaymentLinkResponse> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken ct = default)
    {
        var theme = await _themeRepository.GetByIdAsync(request.ThemeId)
            ?? throw new NotFoundException("Theme not found");

        var payment = new Payment(Guid.CreateVersion7(), request.Phone, theme.Id);
        await _paymentRepository.CreateAsync(payment);

        var separator = request.RedirectPath.Contains('?') ? "&" : "?";
        var returnUrl = $"{request.RedirectPath}{separator}payment_id={payment.Id}";

        var amount = theme.Price / 100m;

        var invoiceUrl = await _wfp.CreateInvoiceAsync(
            orderReference: payment.Id.ToString(),
            amount: amount,
            productName: theme.Name,
            returnUrl: returnUrl,
            ct);

        return new PaymentLinkResponse(invoiceUrl);
    }

    public async Task<WebhookAck> HandleWebhookAsync(WayForPayWebhookData data, CancellationToken ct = default)
    {
        _logger.LogInformation("WayForPay webhook: {@Data}", data);

        var ack = BuildAck(data.OrderReference);

        if (!_wfp.VerifyWebhookSignature(data))
        {
            _logger.LogError("Webhook signature is not valid for {OrderReference}", data.OrderReference);
            return ack;
        }

        if (!Guid.TryParse(data.OrderReference, out var paymentId))
        {
            _logger.LogError("Invalid order reference: {OrderReference}", data.OrderReference);
            return ack;
        }

        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            _logger.LogError("Payment not found: {PaymentId}", paymentId);
            return ack;
        }

        if (data.TransactionStatus == WfpApproved && payment.Status != PaymentStatus.Success)
        {
            payment.MarkSuccess();
            await _paymentRepository.UpdateAsync(payment);
            _logger.LogInformation("Payment {PaymentId} marked as SUCCESS", payment.Id);
        }
        else if (WfpFailedStatuses.Contains(data.TransactionStatus) && payment.Status != PaymentStatus.Failed)
        {
            payment.MarkFailed();
            await _paymentRepository.UpdateAsync(payment);
            _logger.LogInformation("Payment {PaymentId} marked as FAILED", payment.Id);
        }

        return ack;
    }

    private WebhookAck BuildAck(string orderReference)
    {
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        const string status = "accept";
        var signature = _wfp.SignWebhookResponse(orderReference, status, time);
        return new WebhookAck(orderReference, status, time, signature);
    }

    public async Task<ThemeResponse> GetThemeByPaymentAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId)
            ?? throw new NotFoundException("Payment not found");

        if (payment.Status != PaymentStatus.Success)
        {
            throw new ForbiddenException("Payment is not completed");
        }

        var theme = await _themeRepository.GetByIdAsync(payment.ThemeId)
            ?? throw new NotFoundException("Associated theme not found");

        return new ThemeResponse(
            theme.Id, theme.Name, theme.Slug, theme.Description, theme.Type.ToString(), theme.Price,
            [.. theme.Videos.Select(v => new VideoResponse(v.Id, v.Title, v.Url, v.SequenceNumber, v.ThemeId))],
            [.. theme.Photos.Select(p => new PhotoResponse(p.Id, p.Url, p.SequenceNumber, p.ThemeId))]);
    }
}

public record WebhookAck(string OrderReference, string Status, long Time, string Signature);

using Api.ModelBinders;
using Application.DTOs;
using Application.Services;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers;

[ApiController]
[Route("api/v1/payments")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(PaymentLinkResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentLinkResponse>> Create(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _paymentService.CreatePaymentAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("webhook")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Webhook(
        [ModelBinder(typeof(WayForPayWebhookBinder))] WayForPayWebhookData data,
        CancellationToken cancellationToken)
    {
        var ack = await _paymentService.HandleWebhookAsync(data, cancellationToken);
        return Ok(ack);
    }
}

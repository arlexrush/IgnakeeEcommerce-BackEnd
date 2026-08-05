using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using System.Text;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/payments")]
public class PaymentController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _webhookSecret;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(
        IUnitOfWork unitOfWork,
        IOptions<Ecommerce.Application.Models.Payment.StripeSettings> stripeSettings,
        ILogger<PaymentController> logger)
    {
        _unitOfWork = unitOfWork;
        _webhookSecret = stripeSettings.Value.WebhookSecret
            ?? throw new InvalidOperationException("StripeSettings:WebhookSecret is required.");
        _logger = logger;
    }

    /// <summary>
    /// Es responsable for handling Stripe webhook events related to payment intents. It verifies the webhook signature, processes the event, and updates the corresponding order's payment status in the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook(CancellationToken cancellationToken)
    {
        Request.EnableBuffering();
        using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                payload,
                signature,
                _webhookSecret,
                throwOnApiVersionMismatch: false);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(ex, "Invalid Stripe webhook signature.");
            return BadRequest();
        }

        if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
        {
            return Ok();
        }

        var order = await _unitOfWork.Repository<Order>()
            .GetEntityAsync(x => x.PaymentIntentId == paymentIntent.Id, null, false);

        if (order is null)
        {
            _logger.LogWarning("Stripe webhook received for unknown PaymentIntent {PaymentIntentId}.", paymentIntent.Id);
            return Ok();
        }

        switch (stripeEvent.Type)
        {
            case "payment_intent.processing":
                if (order.PaymentStatus is PaymentStatus.Pending)
                {
                    order.MarkPaymentProcessing();
                }
                break;
            case "payment_intent.succeeded":
                if (order.PaymentStatus is not PaymentStatus.Succeeded)
                {
                    order.MarkPaymentSucceeded();
                }
                break;
            case "payment_intent.payment_failed":
                if (order.PaymentStatus is not PaymentStatus.Succeeded)
                {
                    order.MarkPaymentFailed();
                }
                break;
            default:
                return Ok();
        }

        _unitOfWork.Repository<Order>().UpdateEntity(order);
        await _unitOfWork.Complete();
        return Ok();
    }
}

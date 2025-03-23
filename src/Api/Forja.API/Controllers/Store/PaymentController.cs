namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet("{paymentId:guid}")]
    public async Task<IActionResult> GetPaymentById([FromRoute] Guid paymentId)
    {
        if (paymentId == Guid.Empty) return BadRequest(new { error = "Payment ID is required." });

        try
        {
            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment == null) return NotFound(new { error = $"No payment found with ID {paymentId}." });
            return Ok(payment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet("transaction/{transactionId}")]
    public async Task<IActionResult> GetPaymentByTransactionId([FromRoute] string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId)) return BadRequest(new { error = "Transaction ID is required." });

        try
        {
            var payment = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);
            if (payment == null) return NotFound(new { error = $"No payment found with Transaction ID {transactionId}." });
            return Ok(payment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetPaymentsByOrderId([FromRoute] Guid orderId)
    {
        if (orderId == Guid.Empty) return BadRequest(new { error = "Order ID is required." });

        try
        {
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("execute")]
    public async Task<IActionResult> ExecutePayment([FromBody] PaymentRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var transactionId = await _paymentService.ExecutePaymentAsync(request);
            return Ok(new { transactionId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpPut("/update-status")]
    public async Task<IActionResult> UpdatePaymentStatus([FromBody] PaymentUpdateSatusRequest statusRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedPayment = await _paymentService.UpdatePaymentStatusAsync(statusRequest);
            if (updatedPayment == null)
            {
                return NotFound(new { error = $"Payment with ID {statusRequest.Id} not found." });
            }
            return Ok(updatedPayment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = "StoreManagePolicy")]
    [HttpDelete("{paymentId:guid}")]
    public async Task<IActionResult> DeletePayment([FromRoute] Guid paymentId)
    {
        try
        {
            await _paymentService.DeletePaymentAsync(paymentId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
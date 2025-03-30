namespace Forja.API.Controllers.Store;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IAuditLogService _auditLogService;

    public PaymentController(IPaymentService paymentService,
        IAuditLogService auditLogService)
    {
        _paymentService = paymentService;
        _auditLogService = auditLogService;
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
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get payment by id: {paymentId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
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
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get payment by transaction id: {transactionId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
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
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.View,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to get payment by order id: {orderId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
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
            
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = transactionId,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Payment created successfully. Transaction ID: {transactionId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(new { transactionId });
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Create,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to execute payment with order id: {request.OrderId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
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
            
            try
            {
                var logEntry = new LogEntry<PaymentDto>
                {
                    State = updatedPayment,
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Payment updated successfully. Payment ID: {updatedPayment.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return Ok(updatedPayment);
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Update,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to update payment with order id: {statusRequest.Id}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
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
            
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "deleted",
                    UserId = null,
                    Exception = null,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Information,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Payment with id: {paymentId} - deleted successfully" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            try
            {
                var logEntry = new LogEntry<string>
                {
                    State = "Error",
                    UserId = null,
                    Exception = ex,
                    ActionType = AuditActionType.Delete,
                    EntityType = AuditEntityType.Payment,
                    LogLevel = LogLevel.Error,
                    Details = new Dictionary<string, string>
                    {
                        { "Message", $"Failed to delete payment with id: {paymentId}" }
                    }
                };
                
                await _auditLogService.LogWithLogEntryAsync(logEntry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logging audit log entry: {e.Message}");
            }
            return BadRequest(new { error = ex.Message });
        }
    }
}
namespace Forja.Application.Services.Support;

/// <summary>
/// Service implementation for managing FAQ entities.
/// </summary>
public class FAQService : IFAQService
{
    private readonly IFAQRepository _faqRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="FAQService"/> class.
    /// </summary>
    /// <param name="faqRepository">The repository handling data operations for FAQs.</param>
    public FAQService(IFAQRepository faqRepository)
    {
        _faqRepository = faqRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<FAQDto>> GetAllAsync()
    {
        var result = await _faqRepository.GetAllAsync();
        
        return result.Select(SupportEntityToDtoMapper.MapToFAQDto);
    }

    /// <inheritdoc />
    public async Task<FAQDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid FAQ ID.", nameof(id));
        }

        var faq =  await _faqRepository.GetByIdAsync(id);
        
        return faq == null ? null : SupportEntityToDtoMapper.MapToFAQDto(faq);
    }

    /// <inheritdoc />
    public async Task<FAQDto?> CreateAsync(FAQCreateRequest request)
    {
        if (!SupportRequestsValidator.ValidateFAQCreateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var faq = new FAQ
        {
            Id = Guid.NewGuid(),
            Question = request.Question,
            Answer = request.Answer,
            Order = request.Order
        };

        var createdFaq = await _faqRepository.AddAsync(faq);

        return createdFaq == null ? null : SupportEntityToDtoMapper.MapToFAQDto(createdFaq);
    }

    /// <inheritdoc />
    public async Task<FAQDto?> UpdateAsync(FAQUpdateRequest request)
    {
        if (!SupportRequestsValidator.ValidateFAQUpdateRequest(request, out var errors))
        {
            throw new ArgumentException($"Invalid request. Errors: {errors}", nameof(request));
        }

        var existingFaq = await _faqRepository.GetByIdAsync(request.Id);
        if (existingFaq == null)
        {
            throw new KeyNotFoundException("FAQ not found.");
        }

        existingFaq.Question = request.Question;
        existingFaq.Answer = request.Answer;
        existingFaq.Order = request.Order;

        var updatedFaq = await _faqRepository.UpdateAsync(existingFaq);

        return updatedFaq == null ? null : SupportEntityToDtoMapper.MapToFAQDto(updatedFaq);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid FAQ ID.", nameof(id));
        }

        var faq = await _faqRepository.GetByIdAsync(id);
        if (faq == null)
        {
            throw new KeyNotFoundException("FAQ not found.");
        }

        await _faqRepository.DeleteAsync(id);
    }
}
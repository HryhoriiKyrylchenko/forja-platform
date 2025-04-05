namespace Forja.Application.Services.Common;

public class FilterDataService : IFilterDataService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMatureContentRepository _matureContentRepository;
        
    public FilterDataService(IGenreRepository genreRepository, 
        IMechanicRepository mechanicRepository, 
        ITagRepository tagRepository, 
        IMatureContentRepository matureContentRepository)
    {
        _genreRepository = genreRepository;
        _mechanicRepository = mechanicRepository;
        _tagRepository = tagRepository;
        _matureContentRepository = matureContentRepository;
    }
    
    public async Task<ProductsFilterDataDto> GetGameFilterDataAsync()
    {
        var genres = await _genreRepository.GetAllAsync();
        var mechanics = await _mechanicRepository.GetAllAsync();
        var tags = await _tagRepository.GetAllAsync();
        var matureContent = await _matureContentRepository.GetAllAsync();
        
        var genreList = genres.Select(g => g.Name).ToList();
        var mechanicList = mechanics.Select(m => m.Name).ToList();
        var tagList = tags.Select(t => t.Title).ToList();
        var matureContentList = matureContent.Select(mc => mc.Name).ToList();

        return new ProductsFilterDataDto
        {
            Genres = genreList,
            Mechanics = mechanicList,
            Tags = tagList,
            MatureContents = matureContentList
        };
    }
}
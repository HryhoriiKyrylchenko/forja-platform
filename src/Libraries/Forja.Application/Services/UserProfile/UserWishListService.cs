namespace Forja.Application.Services.UserProfile;

/// <summary>
    /// Service class for UserWishList functionality.
    /// Provides logic and coordination for managing UserWishList entities.
    /// </summary>
    public class UserWishListService : IUserWishListService
    {
        private readonly IUserWishListRepository _userWishListRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileManagerService _fileManagerService;
        private readonly IReviewRepository _reviewRepository;

    public UserWishListService( IUserWishListRepository userWishListRepository, 
                                IProductRepository productRepository, 
                                IUserRepository userRepository,
                                IFileManagerService fileManagerService,
                                IReviewRepository reviewRepository)
    {
        _userWishListRepository = userWishListRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _fileManagerService = fileManagerService;
        _reviewRepository = reviewRepository;
    }

    /// <inheritdoc />
    public async Task<List<UserWishListDto>> GetAllAsync()
        {
            var wishLists = await _userWishListRepository.GetAllAsync();
            return wishLists.Select(UserProfileEntityToDtoMapper.MapToUserWishListDto).ToList();
        }

        /// <inheritdoc />
        public async Task<UserWishListDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }
            var wishList = await _userWishListRepository.GetByIdAsync(id);
            return wishList == null ? null : UserProfileEntityToDtoMapper.MapToUserWishListDto(wishList);
        }

        /// <inheritdoc />
        public async Task<UserWishListDto?> AddAsync(UserWishListCreateRequest request)
        {
            if (!UserProfileRequestsValidator.ValidateUserWishListCreateRequest(request))
            {
                throw new ArgumentException("Invalid request.", nameof(request));
            }
            var userWishList = new UserWishList
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ProductId = request.ProductId
            };

            var createdWishList = await _userWishListRepository.AddAsync(userWishList);

            if (createdWishList != null)
            {
                var product = await _productRepository.GetByIdAsync(createdWishList.ProductId);
                if (product != null)
                {
                    createdWishList.Product = product;
                }

                var user = await _userRepository.GetByIdAsync(createdWishList.UserId);
                if (user != null)
                {
                    createdWishList.User = user;
                }
            }

            return createdWishList == null ? null : UserProfileEntityToDtoMapper.MapToUserWishListDto(createdWishList);
        }

        /// <inheritdoc />
        public async Task<UserWishListDto?> UpdateAsync(UserWishListUpdateRequest request)
        {
            if (!UserProfileRequestsValidator.ValidateUserWishListUpdateRequest(request))
            {
                throw new ArgumentException("Invalid request.", nameof(request));
            }
            var userWishList = await _userWishListRepository.GetByIdAsync(request.Id);
            if (userWishList == null)
            {
                throw new KeyNotFoundException($"UserWishList with ID {request.Id} not found.");
            }

            userWishList.UserId = request.UserId;
            userWishList.ProductId = request.ProductId;

            var result = await _userWishListRepository.UpdateAsync(userWishList);
            return result == null ? null : UserProfileEntityToDtoMapper.MapToUserWishListDto(result);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }
            await _userWishListRepository.DeleteAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<UserWishListDto>> GetByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(userId));
            }
            var wishLists = await _userWishListRepository.GetByUserIdAsync(userId);
            return wishLists.Select(UserProfileEntityToDtoMapper.MapToUserWishListDto).ToList();
        }
        
        /// <inheritdoc />
        public async Task<List<UserWishListWithExtendedGameDto>> GetByUserIdWithExtendedGameAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(userId));
            }
            var wishLists = await _userWishListRepository.GetByUserIdAsync(userId);
            
            var result = new List<UserWishListWithExtendedGameDto>();

            foreach (var wish in wishLists)
            {
                if (wish.Product is not Game game)
                    continue; 
                
                var logoUrl = await _fileManagerService.GetPresignedProductLogoUrlAsync(game.Id, 1900);
                var reviewCount = await _reviewRepository.GetProductApprovedReviewsCountAsync(game.Id);

                var addons = new List<GameAddonSmallDto>();
                foreach (var addon in game.GameAddons)
                {
                    var addonLogo = await _fileManagerService.GetPresignedProductLogoUrlAsync(addon.Id, 1900);
                    addons.Add(GamesEntityToDtoMapper.MapToGameAddonSmallDto(addon, addonLogo));
                }

                var achievements = new List<AchievementShortDto>();
                foreach (var achievement in game.Achievements)
                {
                    var achievementLogo = await _fileManagerService.GetPresignedAchievementImageUrlAsync(achievement.Id, 1900);
                    achievements.Add(UserProfileEntityToDtoMapper.MapToAchievementShortDto(achievement, achievementLogo));;
                }
                
                result.Add(UserProfileEntityToDtoMapper.MapToUserWishListWithExtendedGameDto(wish.Id, 
                    game,
                    logoUrl,
                    achievements,
                    addons,
                    reviewCount));
            }

            return result;
        }

    #region User Statistics Methods
    /// <inheritdoc />
    public async Task<int> GetWishListCountAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(userId));
        }

        return await _userWishListRepository.GetCountByUserIdAsync(userId);
    }

    #endregion
}

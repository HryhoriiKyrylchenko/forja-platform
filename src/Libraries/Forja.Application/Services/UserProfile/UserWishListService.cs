namespace Forja.Application.Services.UserProfile;

/// <summary>
    /// Service class for UserWishList functionality.
    /// Provides logic and coordination for managing UserWishList entities.
    /// </summary>
    public class UserWishListService : IUserWishListService
    {
        private readonly IUserWishListRepository _userWishListRepository;

        public UserWishListService(IUserWishListRepository userWishListRepository)
        {
            _userWishListRepository = userWishListRepository;
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
    /// <inheritdoc />
    #endregion
}

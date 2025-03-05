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
        public async Task<IEnumerable<UserWishListDto>> GetAllAsync()
        {
            var wishLists = await _userWishListRepository.GetAllAsync();
            return wishLists.Select(MapToUserWishListDto);
        }

        /// <inheritdoc />
        public async Task<UserWishListDto?> GetByIdAsync(Guid id)
        {
            var wishList = await _userWishListRepository.GetByIdAsync(id);
            return wishList == null ? null : MapToUserWishListDto(wishList);
        }

        /// <inheritdoc />
        public async Task<UserWishListDto> AddAsync(Guid userId, Guid productId)
        {
            var userWishList = new UserWishList
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = productId
            };

            var createdWishList = await _userWishListRepository.AddAsync(userWishList);
            return MapToUserWishListDto(createdWishList);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Guid id, Guid userId, Guid productId)
        {
            var userWishList = await _userWishListRepository.GetByIdAsync(id);
            if (userWishList == null)
                throw new KeyNotFoundException($"UserWishList with ID {id} not found.");

            userWishList.UserId = userId;
            userWishList.ProductId = productId;

            await _userWishListRepository.UpdateAsync(userWishList);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            await _userWishListRepository.DeleteAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserWishListDto>> GetByUserIdAsync(Guid userId)
        {
            var wishLists = await _userWishListRepository.GetByUserIdAsync(userId);
            return wishLists.Select(MapToUserWishListDto);
        }

        /// <summary>
        /// Maps a UserWishList entity to a UserWishListDTO.
        /// </summary>
        /// <param name="userWishList">The UserWishList entity to map.</param>
        /// <returns>The mapped UserWishListDTO.</returns>
        private static UserWishListDto MapToUserWishListDto(UserWishList userWishList)
        {
            return new UserWishListDto
            {
                Id = userWishList.Id,
                UserId = userWishList.UserId,
                UserName = userWishList.User.Username,
                ProductId = userWishList.ProductId,
                ProductName = userWishList.Product.Title
            };
        }
    }

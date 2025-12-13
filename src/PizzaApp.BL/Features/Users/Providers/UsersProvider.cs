using AutoMapper;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.BL.Features.Users.Exceptions;
using PizzaApp.DataAccess.Repository.UserRepository;

namespace PizzaApp.BL.Features.Users.Providers;

public class UsersProvider(IUserRepository userRepository, IMapper mapper) : IUsersProvider
{
    public async Task<UserModel> GetByIdAsync(int id)
    {
        var user = await userRepository.GetByIdWithAllDataAsync(id) ??
            throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        
        return mapper.Map<UserModel>(user);
    }
    
    public async Task<List<UserModel>> GetAllAsync()
    {
        var users = await userRepository.GetAllWithAllDataAsync();
        return mapper.Map<List<UserModel>>(users);
    }
}
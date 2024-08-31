using AutoMapper;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public class GetProfileUserUseCase(ILoggedUser loggedUser,IMapper mapper) : IGetProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();
        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
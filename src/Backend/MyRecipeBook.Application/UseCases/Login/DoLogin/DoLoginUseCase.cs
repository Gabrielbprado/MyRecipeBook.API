using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.BaseException;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(IUserReadOnlyRepository readOnlyRepository, IPasswordCrypt encrypt,IAccessTokenGenerator accessTokenGenerator) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IPasswordCrypt _encrypt = encrypt;
    private readonly IAccessTokenGenerator _tokenGenerator = accessTokenGenerator;
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await _readOnlyRepository.GetByEmail(request.Email);
        var passwordIsValid = user != null && _encrypt.Verify(request.Password, user.Password);
        if (passwordIsValid is false)
        {
            throw new InvalidLoginException();
        }
        return new ResponseRegisterUserJson
        {
            Name = user.Name,
             Tokens= new ResponseTokensJson()
            {
             AccessToken = _tokenGenerator.Generate(user.UserIdentifier)
            }
        };
        
    }
}
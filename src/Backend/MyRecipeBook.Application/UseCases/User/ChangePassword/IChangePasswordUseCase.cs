using MyRecipeBook.Communication.Requests.ChangePassword;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
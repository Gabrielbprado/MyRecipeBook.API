using AutoMapper;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RequestRegisterUserJson,User>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
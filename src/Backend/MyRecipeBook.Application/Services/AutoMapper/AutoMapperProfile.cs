using AutoMapper;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Communication.Enums;
using DishTypes = MyRecipeBook.Communication.Enums.DishTypes;


namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RequestRegisterUserJson,User>();
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<RequestRecipeJson, Recipe>()
            .ForMember(r => r.Ingredients, opts => opts.Ignore())
            .ForMember(r => r.Instructions, opts => opts.Ignore())
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        CreateMap<DishTypes , Domain.Entities.DishTypes>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));
        CreateMap<RequestIngredientJson, Ingredient>();
        CreateMap<RequestInstructionJson, Instruction>();
    }
}
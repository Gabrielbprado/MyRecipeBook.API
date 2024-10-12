using AutoMapper;
using MyRecipeBook.Communication.Requests.Recipe;
using MyRecipeBook.Communication.Requests.User;
using MyRecipeBook.Communication.Responses.User;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Responses.Recipe;
using MyRecipeBook.Domain.Dtos;
using Sqids;
using DishTypes = MyRecipeBook.Communication.Enums.DishTypes;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        private readonly SqidsEncoder<long> _idEncoder;

        public AutoMapperProfile(SqidsEncoder<long> idEncoder)
        {
            _idEncoder = idEncoder;
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, User>();
            CreateMap<RequestRecipeJson, Recipe>()
                .ForMember(r => r.Ingredients, opts => opts.Ignore())
                .ForMember(r => r.Instructions, opts => opts.Ignore())
                .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

            CreateMap<RequestIngredientJson, Ingredient>();
            CreateMap<RequestInstructionJson, Instruction>();
            CreateMap<RequestFilterRecipeJson, FilterRecipesDto>();

        }

        private void DomainToResponse()
        {
            CreateMap<User, ResponseUserProfileJson>();

            CreateMap<Recipe, ResponseRegisteredRecipeJson>()
                .ForMember(r => r.Id, opts => opts.MapFrom(source => _idEncoder.Encode(source.Id)));

            CreateMap<DishTypes, Domain.Entities.DishTypes>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

            CreateMap<Recipe, ResponseShortRecipeJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(dest => dest.AmountIngredients, opt => opt.MapFrom(source => source.Ingredients.Count));


            CreateMap<Recipe,ResponseRecipeJson>()
                .ForMember(des => des.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(des => des.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Select(r => r.Type)));
            
            CreateMap<Ingredient,ResponseIngredientJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));
            
            CreateMap<Instruction,ResponseInstructionsJson>()
                .ForMember(des => des.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));
        }
    }
}

using AutoMapper;
using CommonTestUtilities.IdEncrypt;
using MyRecipeBook.Application.Services.AutoMapper;
using Sqids;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        var sqids = IdEncryptBuilder.Builder();
        return  new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile(sqids));
        }).CreateMapper();
    }
    
}
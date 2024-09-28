using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using Sqids;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "d1SMPg2YaQDEpuFOVRBU845soG0qnLmArefly3N"
        });
        return  new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile(sqids));
        }).CreateMapper();
    }
    
}
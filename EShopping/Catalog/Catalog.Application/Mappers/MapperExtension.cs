using AutoMapper;
using AutoMapper.Execution;

namespace Catalog.Application.Mappers;

public static class MapperExtension
{
    private static readonly Lazy<IMapper> LazyMapper = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty =
                propertyInfo => propertyInfo.GetMethod.IsPublic || propertyInfo.GetMethod.IsAssembly;
            cfg.AddProfile<ProductMapperProfile>();
        });
        return config.CreateMapper();
    });

    public static IMapper Mapper => LazyMapper.Value;
}
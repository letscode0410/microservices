using AutoMapper;
using AutoMapper.Internal;

namespace Basket.Application.Mappers;

public class MapperExtension
{
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(
        () =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = propInfo => propInfo.GetMethod.IsPublic || propInfo.GetMethod.IsAssembly;
                cfg.AddProfile<BasketMappingProfile>();
            });

            return config.CreateMapper();
        }
    );

    public static IMapper Mapper = Lazy.Value;
}
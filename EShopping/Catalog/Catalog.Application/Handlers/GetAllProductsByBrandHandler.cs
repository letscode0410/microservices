using System.Collections;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllProductsByBrandHandler:IRequestHandler<GetAllProductsByBrandQuery,IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsByBrandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IList<ProductResponse>> Handle(GetAllProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        var productList = await _productRepository.GetAllProductsByBrand(request.Name);
        return MapperExtension.Mapper.Map<IList<ProductResponse>>(productList);
    }
}
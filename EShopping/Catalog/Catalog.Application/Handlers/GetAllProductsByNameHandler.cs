using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllProductsByNameHandler:IRequestHandler<GetAllProductsByNameQuery,IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsByNameHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IList<ProductResponse>> Handle(GetAllProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var productList = await _productRepository.GetAllProductsByName(request.Name);
        return MapperExtension.Mapper.Map<IList<ProductResponse>>(productList);
    }
}
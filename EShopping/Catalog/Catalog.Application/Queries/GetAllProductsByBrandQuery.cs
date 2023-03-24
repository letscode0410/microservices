using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllProductsByBrandQuery:IRequest<IList<ProductResponse>>
{
    public string Name { get; }
    public GetAllProductsByBrandQuery(string name)
    {
        this.Name = name;
    }
}
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllProductsByNameQuery:IRequest<IList<ProductResponse>>
{
    public string Name { get; }
    public GetAllProductsByNameQuery(string name)
    {
        this.Name = name;
    }
}
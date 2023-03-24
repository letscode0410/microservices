using System.Globalization;
using System.Net;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class CatalogController:ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{id}",Name = "GetProductId")]
    [ProducesResponseType(typeof(ProductResponse),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> GetAllProducts()
    {
        var query = new GetAllProductsQuery();
        var results = await _mediator.Send(query);
        return Ok(results);
    }
    
}
using Microsoft.AspNetCore.Mvc;
using RaritetBooks.API.Common;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.Queries.Products.GetProducts;
using System.Text.Json;

namespace RaritetBooks.API.Controllers;

public class ProductController(ILogger<ProductController> logger) : ApplicationController
{
    /// <summary>
    /// Get products by filtered search
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// List of filtered products
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    [HttpGet("search")]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBySearch(
        [FromServices] IQueryHandler<GetProductsRequest, GetProductsResponse, Error> handler,
        [FromQuery] GetProductsRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method GET api/product/search started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method GET api/product/search finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }
}
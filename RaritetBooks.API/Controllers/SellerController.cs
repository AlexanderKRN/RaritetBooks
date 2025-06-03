using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using RaritetBooks.API.Authorization;
using RaritetBooks.API.Common;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.Sellers.DeletePhoto;
using RaritetBooks.Application.Features.Sellers.PublishProduct;
using RaritetBooks.Application.Features.Sellers.UpdateProduct;
using RaritetBooks.Application.Features.Sellers.UploadPhoto;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.Queries.Sellers.GetProductsOfSellerById;
using RaritetBooks.Infrastructure.Queries.Sellers.GetSellerByIdWithPhotos;
using RaritetBooks.Infrastructure.Queries.Sellers.GetSellers;
using System.Text.Json;

namespace RaritetBooks.API.Controllers;

public class SellerController(ILogger<SellerController> logger) : ApplicationController
{
    /// <summary>
    /// Get products of the seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="sellerId"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// List of products of the seller
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpGet("products/{sellerId:guid}")]
    [HasPermission(Permissions.Products.READ)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProductsOfSellerById(
    [FromServices] IQueryHandler<Guid, GetProductsOfSellerByIdResponse, Error> handler,
    Guid sellerId,
    CancellationToken ct)
    {
        logger.LogInformation($"Method GET api/seller/products/{sellerId} started.");

        var result = await handler.Handle(sellerId, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method GET api/seller/products/{sellerId} finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Create new product by the seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="request"></param>
    /// <returns>
    /// Returns value of new product
    /// Guid of created product
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpPost("product")]
    [HasPermission(Permissions.Products.CREATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Publish(
        [FromServices] ICommandHandler<PublishProductRequest, Guid, Error> handler,
        [FromForm] PublishProductRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/seller/product started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/seller/product finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Update product by the seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Guid of updated product
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpPut("product")]
    [HasPermission(Permissions.Products.UPDATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(
        [FromServices] ICommandHandler<UpdateProductRequest, Guid, Error> handler,
        [FromBody] UpdateProductRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method PUT api/seller/product started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method PUT api/seller/product finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Delete product by the seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Guid of deleted product
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpDelete("product/{id:guid}")]
    [HasPermission(Permissions.Products.DELETE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<Guid, Guid, Error> handler,
        Guid id,
        CancellationToken ct)
    {
        logger.LogInformation($"Method DELETE api/seller/products/{id} started.");

        var result = await handler.Handle(id, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method DELETE api/seller/products/{id} finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Add photo of the seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Path to S3 of added photo
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpPost("photo")]
    [HasPermission(Permissions.Sellers.UPDATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadPhoto(
        [FromServices] ICommandHandler<UploadSellerPhotoRequest, string, Error> handler,
        [FromForm] UploadSellerPhotoRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/seller/photo started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/seller/photo finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Get seller by ID with his photos
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Seller data with list of photos
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpGet("photos")]
    [HasPermission(Permissions.Sellers.READ)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByIdWithPhotos(
        [FromServices] IQueryHandler<
            GetSellerByIdWithPhotosRequest, GetSellerByIdWithPhotosResponse, Error> handler,
        [FromQuery] GetSellerByIdWithPhotosRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method GET api/seller/photos started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method GET api/seller/photos finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Delete photo of seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Confirmation or error
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpDelete("photo")]
    [HasPermission(Permissions.Sellers.UPDATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeletePhoto(
        [FromServices] ICommandHandler<DeleteSellerPhotoRequest, bool, Error> handler,
        [FromQuery] DeleteSellerPhotoRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method DELETE api/seller/photo started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method DELETE api/seller/photo finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok();
    }

    /// <summary>
    /// Get all sellers with photos
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// List of sellers with photos
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpGet("all")]
    [HasPermission(Permissions.Sellers.READ)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllSellers(
        [FromServices] IQueryHandler<GetSellersResponse, Error> handler,
        CancellationToken ct)
    {
        logger.LogInformation($"Method GET api/seller/all started.");

        var result = await handler.Handle(ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method GET api/seller/all finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }
}
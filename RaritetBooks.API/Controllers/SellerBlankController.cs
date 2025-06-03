using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using RaritetBooks.API.Authorization;
using RaritetBooks.API.Common;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.SellerBlanks.Apply;
using RaritetBooks.Application.Features.SellerBlanks.Approve;
using RaritetBooks.Application.Features.SellerBlanks.Decline;
using RaritetBooks.Domain.Common;
using System.Text.Json;

namespace RaritetBooks.API.Controllers;

public class SellerBlankController(ILogger<SellerBlankController> logger) : ApplicationController
{
    /// <summary>
    /// Create request from any user to become new seller
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="blankRequest"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Guid of new seller or error
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpPost("create")]
    [HasPermission(Permissions.SellerRequests.CREATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<ApplySellerBlankRequest, Guid, Error> handler,
        [FromBody] ApplySellerBlankRequest blankRequest,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/sellerblank/create started. "
            + $"Request: {JsonSerializer.Serialize(blankRequest)}");

        var result = await handler.Handle(blankRequest, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/sellerblank/create finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Approval of new request by administrator
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="blankRequest"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Confirmation or error
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpPost("approve")]
    [HasPermission(Permissions.SellerRequests.UPDATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Approve(
        [FromServices] ICommandHandler<ApproveSellerBlankRequest, bool, Error> handler,
        [FromBody] ApproveSellerBlankRequest blankRequest,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/sellerblank/approve started. "
            + $"Request: {JsonSerializer.Serialize(blankRequest)}");

        var result = await handler.Handle(blankRequest, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/sellerblank/approve finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok();
    }

    /// <summary>
    /// Decline of new request by administrator
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="blankRequest"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Confirmation or error
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="401">User is unauthorized</response>
    [HttpPost("decline")]
    [HasPermission(Permissions.SellerRequests.UPDATE)]
    [Authorize]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Decline(
        [FromServices] ICommandHandler<DeclineSellerBlankRequest, bool, Error> handler,
        [FromBody] DeclineSellerBlankRequest blankRequest,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/sellerblank/decline started. "
            + $"Request: {JsonSerializer.Serialize(blankRequest)}");

        var result = await handler.Handle(blankRequest, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/sellerblank/decline finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using RaritetBooks.API.Common;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.Users.Login;
using RaritetBooks.Application.Features.Users.RefreshToken;
using RaritetBooks.Application.Features.Users.Register;
using RaritetBooks.Domain.Common;
using System.Text.Json;

namespace RaritetBooks.API.Controllers;

public class UserController(ILogger<UserController> logger) : ApplicationController
{
    /// <summary>
    /// Request for registration from any guest
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
    [HttpPost("register")]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] ICommandHandler<RegisterRequest, bool, Error> handler,
        [FromBody] RegisterRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/user/register started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/user/register finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok();
    }

    /// <summary>
    /// Login of registered user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Access token, user ID and role, set up refresh token
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    [HttpPost("login")]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromServices] ICommandHandler<LoginRequest, LoginResponse, Error> handler,
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/user/login started. "
            + $"Request: {JsonSerializer.Serialize(request)}");

        var result = await handler.Handle(request, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/user/login finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }

    /// <summary>
    /// Activation of user by link request from personal email
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="activationLink"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Redirect to user page as confirmation of success activation
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    [HttpGet("activate/{activationLink:guid}")]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate(
        [FromServices] ICommandHandler<Guid, bool, Error> handler,
        Guid activationLink,
        CancellationToken ct)
    {
        logger.LogInformation($"Method GET api/user/activate started. "
            + $"Request: {JsonSerializer.Serialize(activationLink)}");

        var result = await handler.Handle(activationLink, HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method GET api/user/activate finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Redirect("http://localhost:5173/login");
    }

    /// <summary>
    /// Logout request from user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// Confirmation or error
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    [HttpPost("logout")]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout(
        [FromServices] ICommandHandler<bool, Error> handler,
        CancellationToken ct)
    {
        logger.LogInformation($"Method POST api/user/logout started.");

        var result = await handler.Handle(HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method POST api/user/logout finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok();
    }

    /// <summary>
    /// Automatical request for new tokens since access token expired 
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// </remarks>
    /// <param name="handler"></param>
    /// <param name="ct"></param>
    /// <returns>
    /// New access token, repeat user ID and role, set up refresh token
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    [HttpGet("refresh")]
    [ApiVersionNeutral]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh(
        [FromServices] ICommandHandler<RefreshTokenResponse, Error> handler,
        CancellationToken ct)
    {
        logger.LogInformation($"Method GET api/user/refresh started.");

        var result = await handler.Handle(HttpContext, ct);
        if (result.IsFailure)
            return BadRequest(result.Error);

        logger.LogInformation($"Method GET api/user/refresh finished. "
            + $"Response: {JsonSerializer.Serialize(result.Value)}");

        return Ok(result.Value);
    }
}
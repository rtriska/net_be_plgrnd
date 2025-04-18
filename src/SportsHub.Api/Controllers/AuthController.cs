﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Api.Models.Auth;
using IAuthorizationService = SportsHub.Api.Services.IAuthorizationService;

namespace SportsHub.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthorizationService _authService;

    public AuthController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    [HttpPost("/users")]
    public async Task<IActionResult> SignUp([FromBody] SignupRequest signupRequest)
    {
        var result = await _authService.SignUp(signupRequest);
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("sign_in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
    {
        var result = await _authService.SignIn(signInRequest);
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = new []{ result.Error } });
        }
        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("sign_out")]
    public async Task<IActionResult> SignOutCurrent()
    {
        var result = await _authService.SignOut();
        if (result.IsFailure)
        {
            return BadRequest(new { Errors = new[] { result.Error } });
        }

        return Ok(new { Message = "Signed out successfully." });
    }
}
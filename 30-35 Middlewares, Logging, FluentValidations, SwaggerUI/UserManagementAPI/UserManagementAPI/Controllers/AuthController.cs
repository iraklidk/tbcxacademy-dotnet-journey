using Microsoft.AspNetCore.Mvc;
using System;
using UserManagement.Domain.Entity;
using UserManagementAPI.models;
using UserManagementAPI.Services;

[ApiController]
[Route("api/registration")]
public class RegistrationController : ControllerBase
{
    private readonly ISmsService _smsService;
    private readonly AppDbContext _db;

    public RegistrationController(ISmsService smsService, AppDbContext db)
    {
        _smsService = smsService;
        _db = db;
    }

    [HttpPost("send-otp")]
    public IActionResult SendOtp([FromBody] SendOtpRequest request)
    {
        _smsService.SendOtp(request.PhoneNumber);
        return Ok(new { message = "OTP sent" });
    }

    [HttpPost("verify-otp")]
    public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        var valid = _smsService.VerifyOtp(request.PhoneNumber, request.OtpCode);
        if (!valid)
            return BadRequest(new { message = "Invalid or expired OTP" });

        // ✅ CREATE USER HERE
        if (!_db.Users.Any(u => u.PhoneNumber == request.PhoneNumber))
        {
            _db.Users.Add(new User
            {
                PhoneNumber = request.PhoneNumber
            });
            _db.SaveChanges();
        }

        return Ok(new { message = "Registration completed" });
    }
}

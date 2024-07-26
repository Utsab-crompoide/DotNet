using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Services;

namespace Saas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CognitoController(
        IAmazonCognitoIdentityProvider _cognitoService,
        IConfiguration _configuration,
        ILogger<CognitoController> _logger) : BaseController
    {
        [HttpPost("/api/cognito/forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email, IEmailService _emailService)
        {
            try
            {
                var userPoolId = _configuration.GetSection("Cognito:UserPoolId").Value;
                AdminGetUserRequest userRequest = new AdminGetUserRequest
                {
                    Username = email,
                    UserPoolId = userPoolId,
                };

                var response = await _cognitoService.AdminGetUserAsync(userRequest);
                if (response == null)
                {
                    return NotFound("User attributes not found");
                }
                var hint = FindAttributeByName("custom:passwordHint", response.UserAttributes);
                var fromEmail = _configuration.GetSection("EMAIL_FROM").Value;
                if (fromEmail == null)
                {
                    _logger.LogError("ForgotPassword: from email not set");
                    return BadRequest("from email not set");
                }
                var subject = "Password hint";
                var body = $"Hello, \n\nYour Password hint is {hint?.Value}";
                await _emailService.SendEmailAsync(email, fromEmail, subject, body);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("ForgotPassword: " + e.InnerException?.Message + e.Message);
                throw;
            }
        }
        public static AttributeType? FindAttributeByName(string name, List<AttributeType> attributeTypes)
        {
            if (attributeTypes == null || attributeTypes.Count == 0)
            {
                throw new ArgumentException("Provided argument is not an array.");
            }
            return attributeTypes.FirstOrDefault(x => x.Name == name);
        }
    }
}
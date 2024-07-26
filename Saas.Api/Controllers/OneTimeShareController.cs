using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Helpers;
using Saas.Api.Model;
using Saas.Entities;
using Saas.Repository.Interfaces;

namespace Saas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OneTimeShareController(IOneTimeShareRepository _oneTimeShareRepository,
    IMapper _mapper) : BaseController
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OneTimeShareRequestModel), 200)]
    public async Task<IActionResult> GetOneTimeShareById(Guid id, string salt)
    {
        var oneTimeShare = await _oneTimeShareRepository.GetByIdAsync(id);

        if (oneTimeShare is null)
        {
            return NotFound();
        }

        try
        {
            var decryptedData = DecryptOneTimeShare(oneTimeShare, salt);

            if (string.IsNullOrEmpty(decryptedData))
            {
                return BadRequest("Invalid salt provided.");
            }

            if (IsShareValid(oneTimeShare))
            {
                oneTimeShare.AccessCount++;
                await _oneTimeShareRepository.UpdateAsync(oneTimeShare);

                var response = MapToResponseModel(oneTimeShare, decryptedData);
                return Ok(response);
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Error decrypting data: " + ex.Message);
        }

        return NotFound();
    }

    private static string DecryptOneTimeShare(OneTimeShare share, string salt)
    {
        return EncryptionHelper.Decrypt(share.Content, Encoding.UTF8.GetBytes(salt), Convert.FromBase64String(share.IV));
    }

    private static bool IsShareValid(OneTimeShare share)
    {
        return share.AccessCount < 1 && share.ExpiresAt > DateTime.UtcNow;
    }

    private OneTimeShareResponseModel MapToResponseModel(OneTimeShare share, string decryptedData)
    {
        var response = _mapper.Map<OneTimeShareResponseModel>(share);
        response.Content = decryptedData;
        return response;
    }


    [HttpPost]
    public async Task<IActionResult> CreateOneTimeShare([FromBody] OneTimeShareRequestModel oneTimeShareModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var connectingUser = GetCurrentUserId();

        var oneTimeShare = _mapper.Map<OneTimeShare>(oneTimeShareModel);
        oneTimeShare.CreatedBy = connectingUser;
        oneTimeShare.UpdatedBy = connectingUser;


        oneTimeShare.ExpiresAt = oneTimeShareModel.ExpiresAt.ToUniversalTime();
        oneTimeShare.IV = oneTimeShareModel.IV;

        await _oneTimeShareRepository.AddAsync(oneTimeShare);

        var oneTimeShareToSend = _mapper.Map<OneTimeShareResponseModel>(oneTimeShare);

        return CreatedAtAction(nameof(GetOneTimeShareById), new { id = oneTimeShare.Id }, oneTimeShareToSend);
    }


}


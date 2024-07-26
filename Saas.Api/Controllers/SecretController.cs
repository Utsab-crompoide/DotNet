using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Api.Helpers;
using Saas.Api.Model;
using Saas.Entities.Dtos;
using Saas.Entities.enums;
using Saas.Repository.Interfaces;

namespace Saas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SecretController(ISecretRepository _secretRepository,
    IMapper _mapper) : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SecretResponseModel), 200)]
    public async Task<IActionResult> GetSecretById(Guid id)
    {
        var secret = await _secretRepository.GetSecretByIdIncludeCollectionAsync(id);

        if (secret == null)
        {
            return NotFound(Constant.SecretNotFound);
        }
        var secretWithCollection = new SecretResponseModel
        {
            Id = secret.Id,
            Name = secret.Name,
            Content = secret.Content,
            Type = secret.Type,
            IsShared = secret.IsShared,
            CreatedAt = secret.CreatedAt,
            CollectionSecretModels = secret.CollectionSecrets.Select(cs => new CollectionSecretModel
            {
                Name = cs.Collection != null ? cs.Collection.Name : "",
                CollectionId = cs.CollectionId
            }).ToList()
        };
        return Ok(secretWithCollection);
    }
    [HttpGet("{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetSecretByUserId(int currentPage, int pageSize)
    {
        var connectingUser = GetCurrentUserId();

        var paginatedResult = await _secretRepository.GetSecretByUserIdAsync(connectingUser, currentPage, pageSize);

        if (paginatedResult == null)
        {
            return NotFound("Secret not found");
        }
        var secretToSend = paginatedResult.Data.Select(secret => new SecretResponseModel
        {
            Id = secret.Id,
            Name = secret.Name,
            Content = secret.Content,
            Type = secret.Type,
            IsShared = secret.IsShared,
            CreatedAt = secret.CreatedAt,
            CreatedBy = connectingUser == secret.CreatedBy ? "Me" : null,//Todo: check if created by organization

            CollectionSecretModels = secret.CollectionSecrets.Select(collectionSecret => new CollectionSecretModel
            {
                CollectionId = collectionSecret.CollectionId,
                Name = collectionSecret.Collection != null ? collectionSecret.Collection.Name : "",
            }).ToList()
        }).ToList();

        var paginatedSecretDtos = new PaginatedResult<SecretResponseModel>(secretToSend, paginatedResult.TotalItems, paginatedResult.CurrentPage, paginatedResult.PageSize);

        return Ok(paginatedSecretDtos);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSecretByUserId()
    {
        var connectingUser = GetCurrentUserId();

        var result = await _secretRepository.GetSecretByUserIdAsync(connectingUser);

        if (result == null)
        {
            return NotFound(Constant.SecretNotFound);
        }
        var secretToSend = SecretCollectionSelect(connectingUser, result);

        return Ok(secretToSend);
    }



    [HttpGet("{secretType}/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetSecretByUserIdAndSecretType(SecretType secretType, int currentPage, int pageSize)
    {
        var connectingUser = GetCurrentUserId();

        var paginatedResult = await _secretRepository.GetSecretByUserIdAndSecretTypeAsync(connectingUser, secretType, currentPage, pageSize);
        if (paginatedResult == null)
        {
            return NotFound(Constant.SecretNotFound);
        }
        var secretToSend = SecretCollectionSelect(connectingUser, paginatedResult.Data);

        var paginatedSecretDtos = new PaginatedResult<SecretResponseModel>(secretToSend, paginatedResult.TotalItems, paginatedResult.CurrentPage, paginatedResult.PageSize);

        return Ok(paginatedSecretDtos);
    }
    [HttpGet("all/{secretType}")]
    public async Task<IActionResult> GetAllSecretByUserIdAndSecretType(SecretType secretType)
    {
        var connectingUser = GetCurrentUserId();

        var result = await _secretRepository.GetSecretByUserIdAndSecretTypeAsync(connectingUser, secretType);
        if (result == null)
        {
            return NotFound(Constant.SecretNotFound);
        }
        var secretToSend = SecretCollectionSelect(connectingUser, result);
        return Ok(secretToSend);
    }
    [HttpGet("collection/{collectionid}")]
    public async Task<IActionResult> GetAllSecretByUserIdAndCollection(Guid collectionid)
    {
        var connectingUser = GetCurrentUserId();

        var result = await _secretRepository.GetSecretByUserIdAndCollectionAsync(connectingUser, collectionid);
        if (result == null)
        {
            return NotFound(Constant.SecretNotFound);
        }
        var secretToSend = SecretCollectionSelect(connectingUser, result);
        return Ok(secretToSend);
    }
    private static List<SecretResponseModel> SecretCollectionSelect(Guid connectingUser, List<Secret> result)
    {
        var secretToSend = result.Select(secret => new SecretResponseModel
        {
            Id = secret.Id,
            Name = secret.Name,
            Content = secret.Content,
            Type = secret.Type,
            IsShared = secret.IsShared,
            CreatedAt = secret.CreatedAt,
            CreatedBy = connectingUser == secret.CreatedBy ? "Me" : null,//Todo: check if created by organization

            CollectionSecretModels = secret.CollectionSecrets.Select(collectionSecret => new CollectionSecretModel
            {
                CollectionId = collectionSecret.CollectionId,
                Name = collectionSecret.Collection != null ? collectionSecret.Collection.Name : "",
            }).ToList()
        }).ToList();
        return secretToSend;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSecret([FromBody] SecretWithCollectionsRequestModel secretModel, ISecretService _secretService)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var connectingUser = GetCurrentUserId();


        var secret = _mapper.Map<Secret>(secretModel.Secret);
        secret.CreatedBy = connectingUser;
        secret.UpdatedBy = connectingUser;

        var secretCollection = new SecretWithCollection()
        {
            Secret = secret,
            CollectionIds = secretModel.CollectionIds
        };
        var result = await _secretService.AddSecretWithCollections(secretCollection);

        if (result.Success)
            return StatusCode(StatusCodes.Status201Created, result.Message);
        else
            return BadRequest(result.Message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSecret(Guid id, [FromBody] SecretWithCollectionsRequestModel secretModel, ISecretService _secretService)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var connectingUser = GetCurrentUserId();
            var existingSecret = await _secretRepository.GetByIdAsync(id);
            if (existingSecret == null)
            {
                return NotFound("Secret not found");
            }
            var checkifUsersSecret = await _secretRepository.CheckSecretByUserId(connectingUser, id);


            if (!checkifUsersSecret)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Permission denied");
            }
            var secret = _mapper.Map(secretModel.Secret, existingSecret);

            secret.UpdatedBy = connectingUser;

            var secretCollection = new SecretWithCollection()
            {
                Secret = secret,
                CollectionIds = secretModel.CollectionIds
            };
            var result = await _secretService.UpdateSecretWithCollections(secretCollection);

            if (result.Success)
                return StatusCode(StatusCodes.Status204NoContent, result.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception
            return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSecret(Guid id)
    {
        var connectingUser = GetCurrentUserId();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingSecret = await _secretRepository.GetByIdAsync(id);
        if (existingSecret == null)
        {
            return NotFound("Secret not found");
        }
        var checkifUsersSecret = await _secretRepository.CheckSecretByUserId(connectingUser, id);

        if (!checkifUsersSecret)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Permission denied");
        }

        await _secretRepository.DeleteAsync(existingSecret);

        return NoContent();
    }
    [HttpDelete("bulkdelete")]
    public async Task<IActionResult> DeleteSecrets([FromBody] List<Guid> ids)
    {
        var connectingUser = GetCurrentUserId();

        if (!ModelState.IsValid || ids == null || ids.Count == 0)
        {
            return BadRequest("Invalid request. Please provide a list of IDs to delete.");
        }

        var deletedCount = 0;
        foreach (var id in ids)
        {
            var existingSecret = await _secretRepository.GetByIdAsync(id);
            if (existingSecret != null)
            {
                var checkifUsersSecret = await _secretRepository.CheckSecretByUserId(connectingUser, id);
                if (checkifUsersSecret)
                {
                    await _secretRepository.DeleteAsync(existingSecret);
                    deletedCount++;
                }
            }
        }

        if (deletedCount == 0)
        {
            return NotFound("No secrets found to delete, or you don't have permission to delete the provided secrets.");
        }

        return Ok($"Successfully deleted {deletedCount} secrets.");
    }

}


using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Api.Helpers;
using Saas.Api.Model;
using Saas.Repository.Interfaces;

namespace Saas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CollectionController(ICollectionRepository _collectionRepository,
    IMapper _mapper) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCollectionById(Guid id)
    {
        var collection = await _collectionRepository.GetByIdAsync(id);

        if (collection == null)
        {
            return NotFound(Constant.CollectionNotFound);
        }
        var CollectionToSend = _mapper.Map<CollectionResponseModel>(collection);
        return Ok(CollectionToSend);
    }
    [HttpGet("user")]
    public async Task<IActionResult> GetCollectionsByUserId()
    {
        var connectingUser = GetCurrentUserId();

        var collection = await _collectionRepository.GetCollectionsByUserIdAsync(connectingUser);

        if (collection == null)
        {
            return NotFound(Constant.CollectionNotFound);
        }
        var collectionToSend = _mapper.Map<List<CollectionResponseModel>>(collection);
        return Ok(collectionToSend);
    }


    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CollectionModel collectionModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var connectingUser = GetCurrentUserId();
        var isUsersCollectionExists = await _collectionRepository.CheckCollectionExistsByUserId(connectingUser, collectionModel.Name);
        if (isUsersCollectionExists)
        {
            return BadRequest("Collection already exists");
        }
        //Todo: check if Collection is unique in Organization. 
        var collection = _mapper.Map<Collection>(collectionModel);
        collection.CreatedBy = connectingUser;
        collection.UpdatedBy = connectingUser;
        await _collectionRepository.AddAsync(collection);

        return CreatedAtAction(nameof(GetCollectionById), new { id = collection.Id }, collection);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(Guid id, [FromBody] CollectionModel collectionModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var connectingUser = GetCurrentUserId();
            var existingCollection = await _collectionRepository.GetByIdAsync(id);
            if (existingCollection == null)
            {
                return NotFound(Constant.CollectionNotFound);
            }
            var checkifUsersCollection = await _collectionRepository.CheckCollectionByUserId(connectingUser, id);


            if (!checkifUsersCollection)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Permission denied");
            }
            var collection = _mapper.Map(collectionModel, existingCollection);

            collection.UpdatedBy = connectingUser;
            await _collectionRepository.UpdateAsync(collection);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception
            return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCollection(Guid id)
    {
        var connectingUser = GetCurrentUserId();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingCollection = await _collectionRepository.GetByIdAsync(id);
        if (existingCollection == null)
        {
            return NotFound(Constant.CollectionNotFound);
        }
        var checkifUsersCollection = await _collectionRepository.CheckCollectionByUserId(connectingUser, id);

        if (!checkifUsersCollection)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Permission denied");
        }

        await _collectionRepository.DeleteAsync(existingCollection);

        return NoContent();
    }
}

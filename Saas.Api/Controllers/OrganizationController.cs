using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Api.Model;
using Saas.Repository.Interfaces;

namespace Saas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrganizationController(IOrganizationRepository _OrganizationRepository,
    IMapper _mapper) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrganizationById(Guid id)
    {
        var Organization = await _OrganizationRepository.GetByIdAsync(id);

        if (Organization == null)
        {
            return NotFound("Organization not found");
        }
        var OrganizationToSend = _mapper.Map<OrganizationResponseModel>(Organization);
        return Ok(OrganizationToSend);
    }
    [HttpGet("user")]
    public async Task<IActionResult> GetOrganizationsByUserId()
    {
        var connectingUser = GetCurrentUserId();

        var Organization = await _OrganizationRepository.GetOrganizationsByUserIdAsync(connectingUser);

        if (Organization == null)
        {
            return NotFound("Organizations not found");
        }
        var OrganizationToSend = _mapper.Map<List<OrganizationResponseModel>>(Organization);
        return Ok(OrganizationToSend);
    }
    

    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] OrganizationRequestModel organizationModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var connectingUser = GetCurrentUserId();


        var organization = _mapper.Map<Organization>(organizationModel);
        organization.CreatedBy = connectingUser;
        organization.UpdatedBy = connectingUser;
        await _OrganizationRepository.AddAsync(organization);
        var organizationToSend=_mapper.Map<OrganizationResponseModel>(organization);

        return CreatedAtAction(nameof(GetOrganizationById), new { id = organizationToSend.Id }, organizationToSend);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrganization(Guid id, [FromBody] OrganizationRequestModel OrganizationModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var connectingUser = GetCurrentUserId();
            var existingOrganization = await _OrganizationRepository.GetByIdAsync(id);
            if (existingOrganization == null)
            {
                return NotFound("Organization not found");
            }
            var checkifUsersOrganization = await _OrganizationRepository.CheckOrganizationByUserId(connectingUser, id);


            if (!checkifUsersOrganization)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Permission denied");
            }
            var Organization = _mapper.Map(OrganizationModel, existingOrganization);

            Organization.UpdatedBy = connectingUser;
            await _OrganizationRepository.UpdateAsync(Organization);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception
           return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrganization(Guid id)
    {
        var connectingUser = GetCurrentUserId();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingOrganization = await _OrganizationRepository.GetByIdAsync(id);
        if (existingOrganization == null)
        {
            return NotFound("Organization not found");
        }
        var checkifUsersOrganization = await _OrganizationRepository.CheckOrganizationByUserId(connectingUser, id);

        if (!checkifUsersOrganization)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "Permission denied");
        }

        await _OrganizationRepository.DeleteAsync(existingOrganization);

        return NoContent();
    }
}

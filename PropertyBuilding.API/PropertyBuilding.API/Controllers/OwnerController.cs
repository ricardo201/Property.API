using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerService ownerService, IMapper mapper)
        {
            _ownerService = ownerService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var owners = _ownerService.GetOwners();
            var ownersDto = _mapper.Map<IEnumerable<OwnerDto>>(owners);
            var response = new StandardResponse<IEnumerable<OwnerDto>>(ownersDto);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var owner = await _ownerService.GetOwnerAsync(id);
            var ownerDto = _mapper.Map<OwnerDto>(owner);
            var response = new StandardResponse<OwnerDto>(ownerDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OwnerDto ownerDto)
        {
            var owner = _mapper.Map<Owner>(ownerDto);
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            owner.IdUser = idUser;
            owner = await _ownerService.SaveOwnerAsync(owner);
            ownerDto = _mapper.Map<OwnerDto>(owner);
            var response = new StandardResponse<OwnerDto>(ownerDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(OwnerDto ownerDto)
        {
            var owner = _mapper.Map<Owner>(ownerDto);
            owner = await _ownerService.SaveOwnerAsync(owner);
            ownerDto = _mapper.Map<OwnerDto>(owner);
            var response = new StandardResponse<OwnerDto>(ownerDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ownerService.DeleteOwnerAsync(id);
            var response = new StandardResponse<Boolean>(result);
            return Ok(response);
        }
    }
}
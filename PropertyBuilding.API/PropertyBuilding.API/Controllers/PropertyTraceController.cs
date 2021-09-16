using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PropertyTraceController : ControllerBase
    {
        private readonly IPropertyTraceService _propertyTraceService;
        private readonly IMapper _mapper;
        public PropertyTraceController(IPropertyTraceService propertyTraceService, IMapper mapper)
        {
            _propertyTraceService = propertyTraceService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var propertyTraces = _propertyTraceService.GetPropertyTraces();
            var propertyTracesDto = _mapper.Map<IEnumerable<PropertyTraceDto>>(propertyTraces);
            var response = new StandardResponse<IEnumerable<PropertyTraceDto>>(propertyTracesDto);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var propertyTrace = await _propertyTraceService.GetPropertyTraceAsync(id);
            var propertyTraceDto = _mapper.Map<PropertyTraceDto>(propertyTrace);
            var response = new StandardResponse<PropertyTraceDto>(propertyTraceDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PropertyTraceDto propertyTraceDto)
        {
            var propertyTrace = _mapper.Map<PropertyTrace>(propertyTraceDto);
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            propertyTrace.IdUser = idUser;
            propertyTrace = await _propertyTraceService.SavePropertyTraceAsync(propertyTrace);
            propertyTraceDto = _mapper.Map<PropertyTraceDto>(propertyTrace);
            var response = new StandardResponse<PropertyTraceDto>(propertyTraceDto);
            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(PropertyTraceDto propertyTraceDto)
        {
            var propertyTrace = _mapper.Map<PropertyTrace>(propertyTraceDto);
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            propertyTrace.IdUser =  idUser;
            propertyTrace = await _propertyTraceService.UpdatePropertyTraceAsyn(propertyTrace);
            propertyTraceDto = _mapper.Map<PropertyTraceDto>(propertyTrace);
            var response = new StandardResponse<PropertyTraceDto>(propertyTraceDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _propertyTraceService.DeletePropertyTraceAsync(id);
            var response = new StandardResponse<Boolean>(result);
            return Ok(response);
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuilding.API.Responses;
using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Core.QueryFilters;
using PropertyBuilding.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyBuilding.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriServices;
        public PropertyController(IPropertyService propertyService, IMapper mapper, IUriService uriServices)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _uriServices = uriServices;
        }
        [HttpGet(Name = nameof(Get))]
        public async Task<IActionResult> Get([FromQuery] PropertyQueryFilter propertyQueryFilter)
        {
            var properties = _propertyService.GetProperties(propertyQueryFilter);
            var propertyDto = _mapper.Map<IEnumerable<PropertyDto>>(properties);
            var metaData = new MetaDataDto()
            {
                TotalCount = properties.TotalCount,
                CurrentPage = properties.CurrentPage,
                HasNextPages = properties.HasNextPages,
                HasPreviousPages = properties.HasPreviousPages,
                PageSize = properties.PageSize,
                TotalPages = properties.TotalPages,
                NextPageUrl = _uriServices.GetPorpertyPaginationUri(propertyQueryFilter, Url.RouteUrl(nameof(Get))).ToString(),
                PreviousPageUrl = _uriServices.GetPorpertyPaginationUri(propertyQueryFilter, Url.RouteUrl(nameof(Get))).ToString()
            };
            var response = new StandardResponse<IEnumerable<PropertyDto>>(propertyDto)
            {
                Meta = metaData
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            var propertyDto = _mapper.Map<PropertyDto>(property);
            var response = new StandardResponse<PropertyDto>(propertyDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PropertyDto propertyDto)
        {
            var property = _mapper.Map<Property>(propertyDto);
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            property.IdUser = idUser;
            var propertySaved = await _propertyService.SavePropertyAsync(property);
            propertyDto = _mapper.Map<PropertyDto>(propertySaved);
            var response = new StandardResponse<PropertyDto>(propertyDto);
            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(PropertyDto propertyDto)
        {
            var property = _mapper.Map<Property>(propertyDto);
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            property.IdUser = idUser;
            property = await _propertyService.UpdatePropertyAsync(property);
            propertyDto = _mapper.Map<PropertyDto>(property);
            var response = new StandardResponse<PropertyDto>(propertyDto);
            return Ok(response);
        }

        [HttpPatch]
        [Route("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice(PropertyChangePriceDto propertyChangePriceDto)
        {
            var property = _mapper.Map<Property>(propertyChangePriceDto);
            property = await _propertyService.ChangePricePropertyAsync(property);
            var propertyDto = _mapper.Map<PropertyDto>(property);
            var response = new StandardResponse<PropertyDto>(propertyDto);
            return Ok(response);
        }
    }
}
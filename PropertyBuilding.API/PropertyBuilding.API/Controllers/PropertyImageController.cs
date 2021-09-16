using PropertyBuilding.Core.DTOs;
using PropertyBuilding.Core.Entities;
using PropertyBuilding.Core.Interfaces;
using PropertyBuilding.Infrastructure.Interfaces;
using PropertyBuilding.API.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class PropertyImageController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IPropertyImageService _propertyImageService;
        public PropertyImageController(IFileService fileService, IPropertyImageService propertyImageService, IMapper mapper)
        {
            _fileService = fileService;
            _propertyImageService = propertyImageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PropertyImageDto propertyImageDto)
        {
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            var urlFile = await _fileService.SaveImageAsync(propertyImageDto.BlobFile, idUser, (int)propertyImageDto.IdProperty);
            var propertyImage = _mapper.Map<PropertyImage>(propertyImageDto);
            propertyImage.IdUser = idUser;
            propertyImage.File = urlFile;
            propertyImage = await _propertyImageService.SavePropertyImageAsync(propertyImage);
            propertyImageDto = _mapper.Map<PropertyImageDto>(propertyImage);
            var response = new StandardResponse<PropertyImageDto>(propertyImageDto);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            var propertyImage = await _propertyImageService.GetPropertyImageByIdAsync(id, idUser);
            var propertyImageDto = _mapper.Map<PropertyImageDto>(propertyImage);
            var response = new StandardResponse<PropertyImageDto>(propertyImageDto);
            return Ok(response);
        }

        [HttpGet]
        [Route("Download")]
        public async Task<FileContentResult> Download(string filename)
        {
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            var propertyImage = _propertyImageService.GetPropertyImageByFileName(filename, idUser);
            var filePath = _fileService.GetFilePath(propertyImage.File, idUser, propertyImage.IdProperty);
            return File(await System.IO.File.ReadAllBytesAsync(filePath), "application/octet-stream", propertyImage.File);
        }

        [HttpPatch]
        [Route("ChangeEnabled")]
        public async Task<IActionResult> ChangeEnabled(PropertyImageChangeEnabledDto propertyImageChangeEnabledDto)
        {
            var idUser = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type.Contains("IdUser")).Value);
            var propertyImageChangeEnabled = _mapper.Map<PropertyImage>(propertyImageChangeEnabledDto);
            propertyImageChangeEnabled = await _propertyImageService.ChangeEnabledPropertyImageAsyn(propertyImageChangeEnabled, idUser);
            var propertyImageDto = _mapper.Map<PropertyImageDto>(propertyImageChangeEnabled);
            var response = new StandardResponse<PropertyImageDto>(propertyImageDto);
            return Ok(response);
        }
    }
}
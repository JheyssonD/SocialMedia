using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Responses;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Services;
using SocialMedia.Infrastucture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService SecurityService;
        private readonly IMapper Mapper;
        private readonly IPasswordService PasswordService;

        public SecurityController(ISecurityService securityRepository, IMapper mapper, IPasswordService passwordService)
        {
            SecurityService = securityRepository;
            Mapper = mapper;
            PasswordService = passwordService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Security(SecurityDTO securityDTO)
        {
            Security security = Mapper.Map<Security>(securityDTO);
            security.Password = PasswordService.Hash(security.Password);
            await SecurityService.RegisterUser(security);
            securityDTO = Mapper.Map<SecurityDTO>(security);
            ApiResponse<SecurityDTO> response = new ApiResponse<SecurityDTO>(securityDTO);
            return Ok(response);
        }
    }
}

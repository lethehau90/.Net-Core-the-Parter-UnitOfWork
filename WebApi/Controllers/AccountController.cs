using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Data;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApi.ViewModels;
using WebApi.Helpers;
using WebApi.EntityUpdateExtensions;
using Common.Exceptions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly HauLeDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        IServiceProvider _serviceProvider;
        //private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            HauLeDbContext appDbContext,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            //_mapper = mapper;
            _appDbContext = appDbContext;
            _serviceProvider = serviceProvider;
        }

        // POST api/account
        [HttpPost("createRoles")]
        private async Task<IActionResult> CreateRoles([FromBody]AppRoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
                string[] roleNames = { "Admin", "Manager", "Member" };
                IdentityResult roleResult;

                var newAppRole = new AppRole();
                newAppRole.UpdateAppRole(roleVM);

                foreach (var roleName in newAppRole.Name)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName.ToString());
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database: Question 2
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName.ToString()));
                    }
                }

                return NoContent();
            }
            catch (NameDuplicatedException dex)
            {
                return BadRequest(dex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/account
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUser newAppUser = new AppUser();
            newAppUser.updateRegister(registerVM);

            try
            {
                newAppUser.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(newAppUser, registerVM.Password);

                if (result.Succeeded)
                {
                    

                    var roles = registerVM.Roles.ToArray();

                    await _userManager.AddToRolesAsync(newAppUser, roles);
                    
                    await _appDbContext.JobSeekers.AddAsync(new JobSeeker { IdentityId = newAppUser.Id, Location = registerVM.Location });
                    await _appDbContext.SaveChangesAsync();

                    return Ok(registerVM);
                }
                else
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }
            catch (NameDuplicatedException dex)
            {
                return BadRequest(dex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
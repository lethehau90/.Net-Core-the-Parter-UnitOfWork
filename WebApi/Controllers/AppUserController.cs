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
using WebApi.ViewModels.System;
using Data.Infrastructure.UnitOfWork;
using Common;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/appUser")]
    public class AppUserController : Controller
    {
        private readonly HauLeDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private MySettings _mySettings { get; set; } //setting config the MyAPP
        IServiceProvider _serviceProvider;
        IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        public AppUserController(
            UserManager<AppUser> userManager,
            HauLeDbContext appDbContext,
            IOptions<MySettings> mySettings,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            //_mapper = mapper;
            _appDbContext = appDbContext;
            _serviceProvider = serviceProvider;
            _mySettings = mySettings.Value;
        }

        // POST api/appUser
        [HttpPost("addroles")]
        public async Task<IActionResult> CreateRoles([FromBody]AppRoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
                IdentityResult roleResult;

                var newAppRole = new AppRole();
                newAppRole.UpdateAppRole(roleVM);

                roleResult = await RoleManager.CreateAsync(newAppRole);

                return Ok(roleResult);
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

        [Route("getlistpaging")]
        [HttpGet]
        public IActionResult GetListPaging(int page, int pageSize, string filter = null)
        {
            try
            {
                page = page > 0 ? page - 1 : page;
                int totalRow = 0;
                var query = _unitOfWork.GetRepository<AppUser>().GetAll();
                if (!string.IsNullOrEmpty(filter))
                    query = query.Where(x => x.FullName.Contains(filter));
                totalRow = query.Count();
                int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
                var model = query.OrderBy(x => x.BirthDay).Skip(page * pageSize).Take(pageSize);

               var queryModel = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(model);

                var PaginationSet = new PaginationSet<AppUserViewModel>()
                {
                    Items = queryModel.AsQueryable(),
                    MaxPage = _mySettings.MaxPage,
                    Page = page + 1,
                    TotalRow = totalRow,
                    TotalPage = totalPage
                };

                return Ok(PaginationSet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("getById/{Id}")]
        [HttpGet]
        public IActionResult GetById(string Id) //detail app user
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return BadRequest(nameof(Id) + " Không có giá trị");
                }

                AppUser query = _unitOfWork.GetRepository<AppUser>().Find(Id);
                if (query == null)
                {
                    return Ok("Không có dữ liệu");
                }

                var queryModel = Mapper.Map<AppUser, AppUserViewModel>(query);
                return Ok(queryModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/appUser
        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody]AppUserViewModel AppUserVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUser newAppUser = new AppUser();
            newAppUser.updateApUser(AppUserVM);

            try
            {
                newAppUser.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(newAppUser, AppUserVM.Password);

                if (result.Succeeded)
                {

                    var roles = AppUserVM.Roles.ToArray();

                    await _userManager.AddToRolesAsync(newAppUser, roles);

                    return Ok(AppUserVM);
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

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody]AppUserViewModel AppUserVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = await _unitOfWork.GetRepository<AppUser>().FindAsync(AppUserVM.Id);
                try
                {
                    appUser.updateApUser(AppUserVM);
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(appUser);
                        var selectedRole = AppUserVM.Roles.ToArray();

                        selectedRole = selectedRole ?? new string[] { };

                        var roles = selectedRole.Except(userRoles).ToArray();

                        await _userManager.AddToRolesAsync(appUser, roles);


                        return Ok(appUser);
                    }
                    else
                        return BadRequest(string.Join(",", result.Errors));
                }
                catch (NameDuplicatedException dex)
                {
                    return BadRequest(dex.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(Id);
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                    return Ok(Id);
                else
                    return Ok(string.Join(",", result.Errors));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
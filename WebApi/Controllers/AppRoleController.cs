using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Microsoft.AspNetCore.Identity;
using Model;
using Data;
using Data.Infrastructure.UnitOfWork;
using WebApi.ViewModels.System;
using AutoMapper;
using Common;
using Microsoft.Extensions.Options;
using WebApi.ViewModels.DataContracts;
using WebApi.EntityUpdateExtensions;
using Common.Exceptions;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/appRole")]
    public class AppRoleController : Controller
    {
        private IPermissionService _permissionService;
        private IFunctionService _functionService;
        private IUnitOfWork _unitOfWork;
        private MySettings _mySettings { get; set; } //setting config the MyAPP
        private readonly UserManager<AppUser> _userManager;

        public AppRoleController(
                IPermissionService permissionService,
                IFunctionService functionService,
                IUnitOfWork unitOfWork,
                IOptions<MySettings> mySettings,
                UserManager<AppUser> userManager
            )
        {
            _permissionService = permissionService;
            _functionService = functionService;
            _mySettings = mySettings.Value;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("getlistpaging")]
        public IActionResult GetListPaging(int page = 1, int pageSize = 20,  string filter = null)
        {
            try
            {
                page = page > 0 ? page - 1 : page;
                int totalRow = 0;
                var query  = _unitOfWork.GetRepository<AppRole>().GetAll();
                if(!string.IsNullOrEmpty(filter))
                    query = query.Where(x => x.Description.Contains(filter));
                totalRow = query.Count();
                int totalPage = (int)Math.Ceiling((double) totalRow / pageSize);
                var model = query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);

                IEnumerable<AppRoleViewModel> queryModel = Mapper.Map<IEnumerable<AppRole>, IEnumerable< AppRoleViewModel>> (model);

                var PaginationSet = new PaginationSet<AppRoleViewModel>()
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

        [HttpGet("getlistall")]
        public IActionResult Getlistall()
        {
            try
            {
                var request = _unitOfWork.GetRepository<AppRole>().GetAll();
                var requestModel = Mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(request);
                return Ok(requestModel.AsQueryable());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllPermission")]
        public IActionResult GetAllPermission(string functionId)
        {
            try
            {
                List<PermissionViewModel> permissions = new List<PermissionViewModel>();
                var roles = _unitOfWork.GetRepository<AppRole>().GetMulti(x => x.Name != "Admin").AsQueryable();
                var listPermission = _permissionService.GetByFunctionId(functionId).ToList();
                if (listPermission.Count == 0)
                {
                    foreach (var item in roles)
                    {
                        permissions.Add(new PermissionViewModel()
                        {
                            RoleId = item.Id,
                            CanCreate = false,
                            CanDelete = false,
                            CanRead = false,
                            CanUpdate = false,
                            AppRole = new AppRoleViewModel()
                            {
                                Id = item.Id,
                                Description = item.Description,
                                Name = item.Name
                            }
                        });
                    }
                }
                else
                {
                    foreach (var item in roles)
                    {
                        if (!listPermission.Any(x => x.RoleId == item.Id))
                        {
                            permissions.Add(new PermissionViewModel()
                            {
                                RoleId = item.Id,
                                CanCreate = false,
                                CanDelete = false,
                                CanRead = false,
                                CanUpdate = false,
                                AppRole = new AppRoleViewModel()
                                {
                                    Id = item.Id,
                                    Description = item.Description,
                                    Name = item.Name
                                }
                            });
                        }
                        permissions = Mapper.Map<List<Permission>, List<PermissionViewModel>>(listPermission);
                    }
                }
                return Ok(permissions.AsQueryable());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("savePermission")]
        public IActionResult SavePermission(SavePermissionRequest data)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _permissionService.DeleteAll(data.FunctionId);
                    Permission permission = null;
                    foreach (var item in data.Permissions)
                    {
                        permission = new Permission();
                        permission.UpdatePermission(item);
                        permission.FunctionId = data.FunctionId;
                        _permissionService.Add(permission);


                    }
                    var functions = _functionService.GetAllWithParentID(data.FunctionId);
                    if (functions.Any())
                    {
                        foreach (var item in functions)
                        {
                            _permissionService.DeleteAll(item.ID);

                            foreach (var p in data.Permissions)
                            {
                                var childPermission = new Permission();
                                childPermission.FunctionId = item.ID;
                                childPermission.RoleId = p.RoleId;
                                childPermission.CanRead = p.CanRead;
                                childPermission.CanCreate = p.CanCreate;
                                childPermission.CanDelete = p.CanDelete;
                                childPermission.CanUpdate = p.CanUpdate;
                                _permissionService.Add(childPermission);
                            }
                        }
                    }
                    try
                    {
                        _permissionService.SaveChange();
                        return Ok("Lưu quyền thành công");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getById/{Id}")]
        public  IActionResult GetById(string Id) //detail app role
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return BadRequest(nameof(Id) + " Không có giá trị");
                }

                var appRole = _unitOfWork.GetRepository<AppRole>().Find(Id);
                if(appRole == null)
                {
                    return Ok("Không có dữ liệu");
                }

                var appRoleModel = Mapper.Map<AppRole, AppRoleViewModel>(appRole);

                return Ok(appRoleModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult Create([FromBody]AppRoleViewModel appRoleVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newAppRole = new AppRole();
                newAppRole.UpdateAppRole(appRoleVm);
                try
                {
                    _unitOfWork.GetRepository<AppRole>().Insert(newAppRole);
                    return Ok(appRoleVm);
                }
                catch (NameDuplicatedException dex)
                {
                    return BadRequest(dex.Message);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody]AppRoleViewModel appRoleVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appRole = _unitOfWork.GetRepository<AppRole>().Find(appRoleVm.Id);
                try
                {
                    appRole.UpdateAppRole(appRoleVm, "update");
                    _unitOfWork.GetRepository<AppRole>().Update(appRole);
                    return Ok(appRole);
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

        public IActionResult Delete(string Id)
        {
            try
            {
                var appRole = _unitOfWork.GetRepository<AppRole>().Find(Id);
                _unitOfWork.GetRepository<AppRole>().Delete(appRole);
                return Ok(Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
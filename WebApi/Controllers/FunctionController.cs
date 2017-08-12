using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Model;
using WebApi.ViewModels.System;
using AutoMapper;
using WebApi.EntityUpdateExtensions;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Function")]
    public class FunctionController : Controller
    {
        private IFunctionService _functionService;
        public FunctionController(IFunctionService functionService)
        {
            this._functionService = functionService;
        }

        [HttpGet("getAllHierachy")]
        public IActionResult GetAllHierachy(string userName)
        {
            try
            {
                IQueryable<Function> model;
                if (User.IsInRole("Admin"))
                {
                    model = _functionService.GetAll(string.Empty);
                }
                else
                {
                    model = _functionService.GetAllWithPermission(User.Identity.Name);
                }
                IEnumerable<FunctionViewModel> modelVm = Mapper.Map<IEnumerable<Function>, IEnumerable<FunctionViewModel>>(model);
                var parents = modelVm.Where(x => x.Parent == null);
                foreach (var parent in parents)
                {
                    parent.ChildFunctions = modelVm.Where(x => x.ParentId == parent.ID).ToList();
                }

                return Ok(parents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAll")]
        public IActionResult GetAll(string filter = "")
        {
            try
            {
                var model = _functionService.GetAll(filter);
                IEnumerable<FunctionViewModel> modelVm = Mapper.Map<IEnumerable<Function>, IEnumerable<FunctionViewModel>>(model);
                return Ok(modelVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getById")]
        public IActionResult GetById(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return BadRequest(" không có giá trị");
                }
                var function = _functionService.Get(Id);
                if (function == null)
                {
                    return Ok("No data");
                }
                var modelVm = Mapper.Map<Function, FunctionViewModel>(function);

                return Ok(modelVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult Create([FromBody]FunctionViewModel functionViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newFunction = new Function();
                if (_functionService.CheckExistedId(functionViewModel.ID))
                {
                    return BadRequest("Id đã tồn tại");
                }
                else
                {
                    if (functionViewModel.ParentId == "") functionViewModel.ParentId = null;
                    newFunction.UpdateFunction(functionViewModel);

                    _functionService.Create(newFunction);
                    _functionService.Save();
                    return Ok(functionViewModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody]FunctionViewModel functionViewModel)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var function = _functionService.Get(functionViewModel.ID);
                try
                {
                    if (functionViewModel.ParentId == "") functionViewModel.ParentId = null;
                    function.UpdateFunction(functionViewModel);
                    _functionService.Update(function);
                    _functionService.Save();

                    return Ok(function);
                }
                catch (Exception dex)
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
        public IActionResult Delete(string Id)
        {
            try
            {
                _functionService.Delete(Id);
                _functionService.Save();
                return Ok(Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
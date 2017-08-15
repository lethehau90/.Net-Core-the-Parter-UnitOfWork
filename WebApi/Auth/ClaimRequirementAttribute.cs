using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.ViewModels.System;

namespace WebApi.Auth
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAsyncActionFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string Function = _claim.Value;
            string Action = _claim.Type;

            var roles = JsonConvert.DeserializeObject<List<string>>(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "rolesCore").Value);



            if (roles.Count > 0)
            {
                if (!roles.Contains(RoleEnum.Admin.ToString()))
                {
                    var permissions = JsonConvert.DeserializeObject<List<PermissionViewModel>>(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "permissions").Value);

                    if (!permissions.Exists(x => x.FunctionId == Function && x.CanCreate) && Action == ActionEnum.Create.ToString())
                    {
                        context.Result = new UnauthorizedResult();

                    }
                    else if (!permissions.Exists(x => x.FunctionId == Function && x.CanRead) && Action == ActionEnum.Read.ToString())
                    {
                        context.Result = new UnauthorizedResult();

                    }
                    else if (!permissions.Exists(x => x.FunctionId == Function && x.CanDelete) && Action == ActionEnum.Delete.ToString())
                    {
                        context.Result = new UnauthorizedResult();

                    }
                    else if (!permissions.Exists(x => x.FunctionId == Function && x.CanUpdate) && Action == ActionEnum.Update.ToString())
                    {
                        context.Result = new UnauthorizedResult();
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
            //var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            //if (!hasClaim)
            //{
            //    context.Result = new UnauthorizedResult();
            //    //await next();
            //}
            //else
            //{
            //    await next();
            //}
        }
    }
}

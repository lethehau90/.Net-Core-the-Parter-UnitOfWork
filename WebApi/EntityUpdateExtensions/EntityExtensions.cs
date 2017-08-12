using Model;
using WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using WebApi.ViewModels.System;

namespace WebApi.EntityUpdateExtensions
{
    public static class EntityExtensions
    {
        public static void UpdateStudent(this Student student, StudentViewModel studentVM)
        {
            student.Id = studentVM.Id;
            student.Name = studentVM.Name;
            student.EnrollmentDate = studentVM.EnrollmentDate;
        }

        public static void updateApUser(this AppUser appUser, AppUserViewModel appUserVM)
        {
            appUser.Id = appUserVM.Id; ;
            appUser.FullName = appUserVM.FullName;
            if (!string.IsNullOrEmpty(appUserVM.BirthDay))
            {
                DateTime dateTime = DateTime.ParseExact(appUserVM.BirthDay, "dd/MM/yyyy", new CultureInfo("vi-VN"));
                appUser.BirthDay = dateTime;
            }
            appUser.Email = appUserVM.Email;
            appUser.PasswordHash = appUserVM.Password;
            appUser.UserName = appUserVM.UserName;
            appUser.Address = appUserVM.Address;
            appUser.PhoneNumber = appUserVM.PhoneNumber;
            appUser.Avatar = appUserVM.Avatar;
            appUser.Status = appUserVM.Status;
            appUser.Gender = appUserVM.Gender;
        }

        public static void updateCredentials(this AppUser account, CredentialsViewModel CredentialsVM)
        {
            account.UserName = CredentialsVM.UserName;
            account.PasswordHash = CredentialsVM.Password;
        }

        public static void UpdateAppRole(this AppRole appRole, AppRoleViewModel appRoleViewModel, string action = "add")
        {
            if (action == "update")
                appRole.Id = appRoleViewModel.Id;
            else
                appRole.Id = Guid.NewGuid().ToString();
            appRole.Name = appRoleViewModel.Name;
            appRole.Description = appRoleViewModel.Description;
        }

        public static void UpdateFunction(this Function function, FunctionViewModel functionVm)
        {
            function.Name = functionVm.Name;
            function.DisplayOrder = functionVm.DisplayOrder;
            function.IconCss = functionVm.IconCss;
            function.Status = functionVm.Status;
            function.ParentId = functionVm.ParentId;
            function.Status = functionVm.Status;
            function.URL = functionVm.URL;
            function.ID = functionVm.ID;
        }
        public static void UpdatePermission(this Permission permission, PermissionViewModel permissionVm)
        {
            permission.RoleId = permissionVm.RoleId;
            permission.FunctionId = permissionVm.FunctionId;
            permission.CanCreate = permissionVm.CanCreate;
            permission.CanDelete = permissionVm.CanDelete;
            permission.CanRead = permissionVm.CanRead;
            permission.CanUpdate = permissionVm.CanUpdate;
        }


    }
}

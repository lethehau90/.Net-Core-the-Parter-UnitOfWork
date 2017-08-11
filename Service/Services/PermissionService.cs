using Data.Infrastructure.UnitOfWork;
using Data.Repositories;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Service.Services
{
    public interface IPermissionService
    {
        IQueryable<Permission> GetByFunctionId(string functionId);
        IQueryable<Permission> GetByUserId(string userId);
        void Add(Permission permission);
        void DeleteAll(string functionId);
        void SaveChange();
    }

    public class PermissionService : IPermissionService
    {
        private IPermissionRepository _permissionRepository;
        private IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            this._permissionRepository = permissionRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Permission permission)
        {
            _permissionRepository.Insert(permission);
        }

        public void DeleteAll(string functionId)
        {
            var query = _permissionRepository.GetMulti(x => x.FunctionId == functionId);
            _permissionRepository.Delete(query);
        }

        public IQueryable<Permission> GetByFunctionId(string functionId)
        {
            return _permissionRepository
                .GetMulti(x => x.FunctionId == functionId, new string[] { "AppRole", "AppRole" });
        }

        public IQueryable<Permission> GetByUserId(string userId)
        {
            return _permissionRepository.GetByUserId(userId);
        }

        public void SaveChange()
        {
            _unitOfWork.SaveChanges();
        }
    }
}

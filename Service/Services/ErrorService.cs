using Data.Infrastructure.UnitOfWork;
using Data.Repositories;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Services
{
    public interface IErrorService
    {
        void Create(Error error);

        void Save();
        
    }
    public class ErrorService : IErrorService
    {
        private IErrorRepository _errorRepository;
        private IUnitOfWork _unitOfWork;
        public ErrorService(IErrorRepository errorRepository, IUnitOfWork unitOfWork)
        {
            _errorRepository = errorRepository;
            _unitOfWork = unitOfWork;
        }
        public void Create(Error error)
        {
             _errorRepository.InsertAsync(error);
        }

        public void Save()
        {
            _unitOfWork.SaveChangesAsync();
        }
    }
}

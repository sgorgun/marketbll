using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CustomerModel model)
        {
            if (model.IsValidModel())
            {
                var customer = _mapper.Map<Customer>(model);

                await _unitOfWork.CustomerRepository.AddAsync(customer);

                await _unitOfWork.SaveAsync(); 
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            if (_unitOfWork.CustomerRepository.GetByIdAsync(modelId) == null)
                throw new MarketException();

            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            return (await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync()).Select(c => _mapper.Map<CustomerModel>(c));
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);

            if (customer == null)
                return null;

            return _mapper.Map<CustomerModel>(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customer = (await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync()).Where(c => c.Receipts.Any(r => r.ReceiptDetails.Any(rd => rd.ProductId == productId)));
            return _mapper.Map<IEnumerable<CustomerModel>>(customer);
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            if (model.IsValidModel())
            {
                var customer = _mapper.Map<Customer>(model);
                customer.Person.Id = model.Id;

                _unitOfWork.CustomerRepository.Update(customer);

                await _unitOfWork.SaveAsync(); 
            }
        }
    }
}

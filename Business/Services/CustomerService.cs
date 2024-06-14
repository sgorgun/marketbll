﻿using AutoMapper;
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
        private const int MINYEAR = 1900;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CustomerModel model)
        {
            ValidateCustomerModel(model);

            var customer = _mapper.Map<Customer>(model);

            await _unitOfWork.CustomerRepository.AddAsync(customer);

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(modelId);
            if (customer == null)
                throw new MarketException();

            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return customers.Select(c => _mapper.Map<CustomerModel>(c));
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return customer != null ? _mapper.Map<CustomerModel>(customer) : null;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var filteredCustomers = customers.Where(c => c.Receipts.Any(r => r.ReceiptDetails.Any(rd => rd.ProductId == productId)));
            return _mapper.Map<IEnumerable<CustomerModel>>(filteredCustomers);
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            ValidateCustomerModel(model);

            var customer = _mapper.Map<Customer>(model);
            customer.Person.Id = model.Id;

            _unitOfWork.CustomerRepository.Update(customer);

            await _unitOfWork.SaveAsync();
        }

        private static void ValidateCustomerModel(CustomerModel model)
        {
            if (model == null)
                throw new MarketException();

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
                throw new MarketException();

            if (model.BirthDate.Year < MINYEAR || model.BirthDate.Year > DateTime.Now.Year)
                throw new MarketException();

            if (model.DiscountValue < 0)
                throw new MarketException();
        }
    }
}

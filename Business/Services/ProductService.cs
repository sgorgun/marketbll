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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddAsync(ProductModel model)
        {
            ValidateModel(model);

            var product = _mapper.Map<Product>(model);

            product.Category.Id = product.ProductCategoryId;
            _unitOfWork.ProductCategoryRepository.Update(product.Category);

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidateCategoryModel(categoryModel);

            var productCategory = _mapper.Map<ProductCategory>(categoryModel);

            await _unitOfWork.ProductCategoryRepository.AddAsync(productCategory);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(modelId);
            if (product == null)
                throw new MarketException();

            await _unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var categories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(categories);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            if (filterSearch == null)
                throw new ArgumentNullException(nameof(filterSearch));

            var query = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();

            if (filterSearch.MaxPrice != null)
                query = query.Where(p => p.Price <= filterSearch.MaxPrice);

            if (filterSearch.MinPrice != null)
                query = query.Where(p => p.Price >= filterSearch.MinPrice);

            if (filterSearch.CategoryId != null)
                query = query.Where(p => p.ProductCategoryId == filterSearch.CategoryId);

            return _mapper.Map<IEnumerable<ProductModel>>(query);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(product);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                throw new MarketException();

            await _unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            ValidateModel(model);

            var product = _mapper.Map<Product>(model);
            product.Category = new ProductCategory();
            product.Category.Id = model.ProductCategoryId;

            _unitOfWork.ProductCategoryRepository.Update(product.Category);
            await _unitOfWork.SaveAsync();

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidateCategoryModel(categoryModel);

            var productCategory = _mapper.Map<ProductCategory>(categoryModel);

            _unitOfWork.ProductCategoryRepository.Update(productCategory);
            await _unitOfWork.SaveAsync();
        }

        private static void ValidateModel(ProductModel model)
        {
            if (model == null)
                throw new MarketException();

            if (model.Price < 0)
                throw new MarketException();

            if (string.IsNullOrEmpty(model.ProductName))
                throw new MarketException();
        }

        private static void ValidateCategoryModel(ProductCategoryModel categoryModel)
        {
            if (categoryModel == null)
                throw new MarketException();

            if (string.IsNullOrEmpty(categoryModel.CategoryName))
                throw new MarketException();
        }
    }
}

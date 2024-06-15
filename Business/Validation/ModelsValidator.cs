using Business.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Business.Validation
{
    public static class ModelsValidator
    {
        public static bool IsValidModel(this BaseModel baseModel)
        {

            if (baseModel == null)
                throw new MarketException("Model: Model can't be null!");

            Type modelType = baseModel.GetType();

            if (modelType.Equals(typeof(ProductCategoryModel)))
                return ProductCategoryModelIsValid(baseModel as ProductCategoryModel);

            if (modelType.Equals(typeof(ProductModel)))
                return ProductModelIsValid(baseModel as ProductModel);

            if (modelType.Equals(typeof(ReceiptModel)))
                return ReceiptModelIsValid(baseModel as ReceiptModel);

            if (modelType.Equals(typeof(CustomerModel)))
                return CustomerModelIsValid(baseModel as CustomerModel);

            if (modelType.Equals(typeof(ReceiptDetailModel)))
                return ReceiptDetailModelIsValid(baseModel as ReceiptDetailModel);
            return false;
        }
        private static bool ProductCategoryModelIsValid(ProductCategoryModel productCategoryModel)
        {
            if (productCategoryModel == null)
                throw new MarketException("ProductCategory Model: Product category can't be null!");

            if (productCategoryModel.Id < 0)
                throw new MarketException("ProductCategory Model: Product category ID field can't be of a negative number!");

            if (String.IsNullOrEmpty(productCategoryModel.CategoryName))
                throw new MarketException("ProductCategory Model: Product category Name field can't be null or empty!");

            return true;
        }

        private static bool ProductModelIsValid(ProductModel productModel)
        {
            if (productModel == null)
                throw new MarketException("Product Model: Product can't be null!");

            if (productModel.Id < 0)
                throw new MarketException("Product Model: Product's ID field can't be of a negative number!");

            if (productModel.Price < 0)
                throw new MarketException("Product Model: Product's Price field can't be of a negative number!");

            if (String.IsNullOrEmpty(productModel.ProductName))
                throw new MarketException("Product Model: Product category Name field can't be null or empty!");

            return true;
        }

        private static bool ReceiptModelIsValid(ReceiptModel receiptModel)
        {
            if (receiptModel == null)
                throw new MarketException("Receipt Model: Receipt can't be null!");

            if (receiptModel.Id < 0)
                throw new MarketException("Receipt Model: Receipt's ID field can't be of a negative number!");

            if (receiptModel.CustomerId < 0)
                throw new MarketException("Receipt Model: Customer's ID field can't be of a negative number!");

            return true;
        }

        public static bool CustomerModelIsValid(CustomerModel customerModel)
        {
            if (customerModel == null)
                throw new MarketException("Customer Model: Customer can't be null!");

            if (customerModel.Id < 0)
                throw new MarketException("Customer Model: Customer's ID field can't be of a negative number!");

            if (customerModel.DiscountValue < 0)
                throw new MarketException("Customer Model: DiscountValue field can't be of a negative number!");

            if (String.IsNullOrEmpty(customerModel.Name) || String.IsNullOrEmpty(customerModel.Surname))
                throw new MarketException("Customer Model: Customer's name and surname can't be null or empty!");

            DateTime minDate = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime maxDate = new(2005, 1, 1, 23, 59, 59, DateTimeKind.Utc);

            if (customerModel.BirthDate < minDate || customerModel.BirthDate > maxDate)
                throw new MarketException("Customer Model: Invalid date of birth!");

            return true;
        }

        public static bool ReceiptDetailModelIsValid(ReceiptDetailModel receiptDetailModel)
        {
            if (receiptDetailModel == null)
                throw new MarketException("ReceiptDetail Model: Customer can't be null!");

            if (receiptDetailModel.Id < 0 ||
                receiptDetailModel.ReceiptId < 0 ||
                receiptDetailModel.ProductId < 0 ||
                receiptDetailModel.DiscountUnitPrice < 0 ||
                receiptDetailModel.UnitPrice <= 0 ||
                receiptDetailModel.Quantity <= 0)
                throw new MarketException("ReceiptDetail Model: Receipt's, Product's, ReceiptDetails' ID fields can't be of a negative number! DiscountUnitPrice has to be 0 or more, and UnitPrice and Quantity must be more than 0!");

            return true;
        }

    }
}

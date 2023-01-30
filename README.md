# Trade Market - BLL


## Domain description

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market.


## Task

Make a Business Logic Layer (BLL) for the electronic system **"Trade Market"** with a three-layered architecture in the form of dynamic library called “Business”. Data Access Layer (DAL) is used from the Trade Market task – DAL.

The structure of the BLL project in the final form:
- The folder **Models** contains classes of logic’s models - it is necessary to develop models according to the diagram (fig.).
- The folder **Interfaces** contains BLL service interfaces.
- The folder **Services** contains service classes that implement interfaces from the folder **Interfaces** – all services must be implemented according to interfaces from the folder **Interfaces**.
- The root folder of the project contains **AutomapperProfile.cs** file to display DAL entities in the BLL model. And opposite – implement two-way view 
```
Product <-> ProductModel
Customer <-> CustomerModel
Receipt <-> ReceiptModel
ReceiptDetail <-> ReceiptDetailModel,
ProductCategory <-> ProductCategoryModel
```
 
in the AutomapperProfile class.
- The folder **Validation** which contains **MarketException.cs** file – make the class of user exception **MarketException**.

![Business Entities](/Business/BusinessModels_Scheme.jpeg)

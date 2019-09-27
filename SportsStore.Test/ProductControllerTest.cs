using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;
using System.Linq;
using SportsStore.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Test
{
    public class ProductControllerTest
    {
        [Fact]
        public void Can_Paginate() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ ProductID = 1, Name = "P1"},
                new Product{ ProductID = 2, Name = "P2"},
                new Product{ ProductID = 3, Name = "P3"},
                new Product{ ProductID = 4, Name = "P4"},
                new Product{ ProductID = 5, Name = "P5"},

            }).AsQueryable<Product>());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
        [Fact]
        public void Can_Send_Pag1nation_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ ProductID = 1, Name = "P1"},
                new Product{ ProductID = 2, Name = "P2"},
                new Product{ ProductID = 3, Name = "P3"},
                new Product{ ProductID = 4, Name = "P4"},
                new Product{ ProductID = 5, Name = "P5"},

            }).AsQueryable<Product>());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            Paginginfo pageinfo = result.Paginginfo;
            Assert.Equal(2, pageinfo.CurrentPage);
            Assert.Equal(3, pageinfo.ItemsPerPage);
            Assert.Equal(5, pageinfo.Totalitems);
            Assert.Equal(2, pageinfo.TotalPages);
        }
        [Fact]
        public void Can_Fillter_Producs() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product{ ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product{ ProductID = 3, Name = "P3", Category = "Cat3"},
                new Product{ ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product{ ProductID = 5, Name = "P5", Category = "Cat5"},

            }).AsQueryable<Product>());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            Product[] result =
                (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();
             
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[0].Category == "Cat2");

        }
        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product{ ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product{ ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product{ ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product{ ProductID = 5, Name = "P5", Category = "Cat3"},

            }).AsQueryable<Product>());
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            Func<ViewResult, ProductsListViewModel> GetModel = result =>
            result?.ViewData?.Model as ProductsListViewModel;

            int? res1 = GetModel(target.List("Cat1"))?.Paginginfo.Totalitems;
            int? res2 = GetModel(target.List("Cat2"))?.Paginginfo.Totalitems;
            int? res3 = GetModel(target.List("Cat3"))?.Paginginfo.Totalitems;
            int? resAll = GetModel(target.List(null))?.Paginginfo.Totalitems;

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
         

        }

    }
}

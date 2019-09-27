using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string category, int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = repository.Products
            .Where(p => category == null || p.Category == category)
            .OrderBy(p => p.ProductID)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize),
                Paginginfo = new Paginginfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    Totalitems =  category == null? 
                    repository.Products.Count():
                    repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            });
    }
}

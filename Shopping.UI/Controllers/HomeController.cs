using Shopping.Core;
using Shopping.Core.Contracts;
using Shopping.Core.Models;
using Shopping.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.UI.Controllers
{
    public class HomeController : Controller
    {
        IRepoistory<Product> context;
        IRepoistory<ProductCategory> productCategories;

        public HomeController(IRepoistory<Product> context, IRepoistory<ProductCategory> productCategories)
        {
            this.context = context;
            this.productCategories = productCategories;
        }
        public ActionResult Index(string Category=null)
        {
            List<Product> products;
            List<ProductCategory> categories = productCategories.Collection().ToList();
            if (Category == null)
            {
                products = context.Collection().ToList();
            }
            else
            {
                products = context.Collection().Where(x => x.Category == Category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Product = products;
            model.ProductCategories = categories;

           
            return View(model);
        }

        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping.Core;
using Shopping.Core.Contracts;
using Shopping.Core.Models;
using Shopping.Core.ViewModels;



namespace Shopping.UI.Controllers
{
    public class ProductManagerController : Controller
    {
        // GET: Product
        // GET: ProductManager
        IRepoistory<Product> context;
        IRepoistory<ProductCategory> productCategories;
        public ProductManagerController(IRepoistory<Product> context, IRepoistory<ProductCategory> productCategories)
        {
            this.context = context;
            this.productCategories = productCategories;
        }
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product,HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//Productimages//")+product.Image);
                }
                context.Insert(product);
                context.commit();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string Id, HttpPostedFileBase file)
        {
            Product product = context.Find(Id);
            ProductCategory category = new ProductCategory();

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();
                return View(viewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();

            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//Productimages//")+ productToEdit.Image);
                }
                productToEdit.Name = product.Name;
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Price = product.Price;
                context.commit();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(string Id)
        {

            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }

        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {

            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.commit();
                return RedirectToAction("Index");
            }

        }
    }
}
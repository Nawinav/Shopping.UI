using Shopping.Core;
using Shopping.Core.Contracts;
using Shopping.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shopping.Service
{
    public class BasketService
    {
        IRepoistory<Product> productContext;
        IRepoistory<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepoistory<Product> ProductContext, IRepoistory<Basket> BasketContext)
        {
            this.productContext = ProductContext;
            this.basketContext = BasketContext;
        }

        private Basket GetBasket(HttpContextBase httpContext,bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();
            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
               
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {

            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;


        }

        public void AddToBasket(HttpContextBase httpContext,string productId)
        {
            Basket basket = GetBasket(httpContext,true);
            BasketItem item = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }
            basketContext.commit();
            
        }

        public void RemovefromBasket(HttpContextBase httpContext,string ItemId)
        {
            Basket basket = GetBasket(httpContext,true);
            BasketItem item = basket.BasketItems.FirstOrDefault(x => x.Id == ItemId);
            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.commit();
            }
        }
    }
}

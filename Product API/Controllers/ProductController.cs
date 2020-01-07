using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductDataAccess;

namespace UserAPI.Controllers
{
    public class ProductController : ApiController
    {
        public IEnumerable<Product> Get()
        {
            using (ProductDataEntities productEntities = new ProductDataEntities())
            {
                return productEntities.Products.ToList();
            }
        }

        public Product Get(int id)
        {
            using (ProductDataEntities productEntities = new ProductDataEntities())
            {
                var productEntity = productEntities.Products.FirstOrDefault(e => e.ProductId == id);
                return productEntity;
            }
        }

        public HttpResponseMessage Post([FromBody] Product product)
        {
            using (ProductDataEntities productEntities = new ProductDataEntities())
            {
                try
                {
                    productEntities.Products.Add(product);
                    productEntities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, product);
                    message.Headers.Location = new Uri(Request.RequestUri + product.ProductId.ToString());
                    return message;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] Product product)
        {
            using (ProductDataEntities productEntities = new ProductDataEntities())
            {
                try
                {
                    Product productEntity = productEntities.Products.FirstOrDefault(e => e.ProductId == id);
                    if (productEntity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user with the id of " + id.ToString() + " was not found.");
                    }
                    else
                    {
                        productEntity.ProductName = product.ProductName;
                        productEntity.ProductDescription = product.ProductDescription;
                        productEntity.ProductPrice = product.ProductPrice;
                        productEntity.ProductType = product.ProductType;

                        productEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, productEntities);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            using (ProductDataEntities productEntities = new ProductDataEntities())
            {
                var entity = productEntities.Products.FirstOrDefault(e => e.ProductId == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The product with the id of " + id.ToString() + " was not found.");
                }
                else
                {
                    productEntities.Products.Remove(entity);
                    productEntities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
        }
    }
}

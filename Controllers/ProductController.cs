using Newtonsoft.Json;
using Project.BusinessLogic;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult SaveProduct()
        {
            return View();
        }
        [HttpPost]
        public string SaveProduct(string orderObj)
        {
            var obj = JsonConvert.DeserializeObject<Product>(orderObj);
            return BL_Product.SaveProdcut(obj, BaseController.WDMSUSER.UID);
        }
        public ActionResult GetProductList()
        {
            var res = BL_Product.GetProductList(BaseController.WDMSUSER.UID);
            return View(res.ToList());
        }
    }
}
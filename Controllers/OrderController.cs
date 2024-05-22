using Newtonsoft.Json;
using Project.BusinessLogic;
using Project.BusinessObject;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Project.Controllers
{
    public class OrderController : BaseController

    {
        // GET: Order
        public ActionResult Index()
        {
            ViewBag.Products = BL_Orders.GetPrdoucts();
            ViewBag.PaymentMode = BL_Orders.GetPaymentMode();
            ViewBag.Suppliers = BL_Orders.GetSupplier();
            return View();
        }
        [HttpPost]
        public JsonResult GetProductsBySupplier(int supplierID)
        {
            var res = BL_Orders.GetProductsBySupplier(supplierID);
            var json = new JavaScriptSerializer().Serialize(res);
            return Json(new { json = json });
        }
        [HttpPost]
        public string SaveOrder(string orderObj)
        {
            var obj = JsonConvert.DeserializeObject<CreateOrder>(orderObj);
            return BL_Orders.SaveOrder(obj, BaseController.WDMSUSER.UID);
        }
        [HttpPost]
        public JsonResult GetPrice(int productID)
        {
            var res = BL_Orders.GetPrdouctPrice(productID);
            var json = new JavaScriptSerializer().Serialize(res);
            return Json(new { json = json });
        }
        public ActionResult History()
        {
            var res = BL_Orders.GetOrderHistory(BaseController.WDMSUSER.UID, "Buyer");
            return View(res.ToList());
        }
        public ActionResult GetSupplierOrderHistory(string orderDelivered)
        {
            var res = BL_Orders.GetOrderHistory(BaseController.WDMSUSER.UID, "Supplier");
            res = res.Where(x => x.Delivered == orderDelivered).ToList();
            return View(res.ToList());
        }
        public ActionResult GetOrderOnPending(string orderDelivered)
        {
            var res = BL_Orders.GetOrderHistory(BaseController.WDMSUSER.UID, "Supplier");
            res = res.Where(x => x.Delivered == orderDelivered).ToList();
            return View(res.ToList());
        }
        public ActionResult OrderDelivered(int? orderID)
        {
            var res = BL_Orders.OrderDelivered(orderID);
            //if(res.Equals("Success"))  
                return Redirect("/Order/GetSupplierOrderHistory?orderDelivered=YES");
            //return View(res);
        }
        public ActionResult ManagePayment(int? orderID)
        {
            //ViewBag.ManagePayment
            ManagePayment res = BL_Orders.GetPaymentDetails(orderID);
            ViewBag.AmountToPay = res.TotalPayment / res.NoOfInstallments;
            ViewBag.OrderID = orderID;
            return View(res);
        }
        public string Manage_Payment(int? NoofPaymentPaid, int? orderID)
        {
            NoofPaymentPaid = 0;
            var res = BL_Orders.SaveOrderPayments(NoofPaymentPaid.Value, orderID.Value);
            res = "Success";
            //if(res.Equals("Success"))
                //return Redirect("/Order/GetSupplierOrderHistory?orderDelivered=YES");
            return res;
        }
    }
}
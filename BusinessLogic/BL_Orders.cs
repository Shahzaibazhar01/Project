using Project.BusinessObject;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.BusinessLogic
{
    public class BL_Orders
    {
        internal static dynamic GetPrdoucts()
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var temp = db.Products.ToList();
                List<SelectListItem> products = new List<SelectListItem>() {
                    new SelectListItem { Text = "Select", Value = "Select" }
                };
                //foreach (var item in temp)
                //    products.Add(new SelectListItem { Text = item.Name, Value = item.PID.ToString() });
                return products;
            }
        }

        internal static object GetProductsBySupplier(int supplierID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var temp = (from a in db.Products
                           where a.SupplierID == supplierID
                           select new { Name = a.Name, ID = a.PID }).ToList();
                return temp;
            }
        }

        internal static dynamic GetSupplier()
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var temp = from a in db.Users
                               join b in db.UserRoles on a.URole equals b.RID
                               where b.RName == "Supplier"
                               select new { Name = a.UName, ID = a.UID };
                List<SelectListItem> supplier = new List<SelectListItem>() {
                    new SelectListItem { Text = "Select", Value = "Select" }
                };
                foreach (var item in temp)
                    supplier.Add(new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                return supplier;
            }
        }

        internal static string SaveOrder(CreateOrder obj, int buyerID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                try
                {
                    var order = new ORDER();
                    order.BuyerID = buyerID;
                    order.ODate = DateTime.Now;
                    order.OTotal = obj.Total;
                    order.SupplierId = obj.Supplier;
                    db.ORDERS.Add(order);
                    db.SaveChanges();

                    var orderid = order.OID; //db.ORDERS.Max(x => x.OID) + 1;
                    var orderDetails = new OrderDetail();
                    orderDetails.OPrice = obj.Price;
                    orderDetails.Qunatity = obj.Quantity;
                    orderDetails.OrderID = orderid;
                    orderDetails.ProductID = obj.Product;
                    db.OrderDetails.Add(orderDetails);

                    var payment = new Payment();
                    payment.NoOfInstallment = string.IsNullOrEmpty(obj.NooOfInstallment.ToString()) ? 1: obj.NooOfInstallment;
                    payment.PaymentModeID = obj.PaymentMode;
                    payment.OrderID = orderid;
                    db.Payments.Add(payment);

                    db.SaveChanges();
                    return "Ordered Successfully";
                }
                catch(Exception ex)
                {
                    return "Failed";
                }
            }
        }

        internal static string SaveOrderPayments(int noofPaymentPaid, int orderID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                try
                {
                    var order = db.ORDERS.Where(x => x.OID == orderID).FirstOrDefault();
                    var payment = db.Payments.Where(x => x.OrderID == orderID).FirstOrDefault();
                    payment.NoOfInstallmentPaid = (payment.NoOfInstallmentPaid == null) ? 0 : payment.NoOfInstallmentPaid;
                    payment.NoOfInstallmentPaid = payment.NoOfInstallmentPaid + 1;
                    payment.PDateTime = DateTime.Now;
                    var paymentDetail = new PaymentDetail();
                    paymentDetail.PaymentID = payment.PYID;
                    paymentDetail.PaidAmount = order.OTotal / payment.NoOfInstallment;
                    db.PaymentDetails.Add(paymentDetail);
                    db.SaveChanges();
                    return "Success";
                }
                catch(Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }

        internal static ManagePayment GetPaymentDetails(int? orderID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            { 
                var res = (from a in db.ORDERS
                                     join b in db.Payments on a.OID equals b.OrderID
                                     where b.OrderID == orderID
                                     select new ManagePayment { NoOfInstallmentsPaid = b.NoOfInstallmentPaid, NoOfInstallments = b.NoOfInstallment, TotalPayment = a.OTotal, PaymentTypeID = b.PaymentModeID }).FirstOrDefault();
                //res = (ManagePayment)res;
                res.PaymentType = "Pay " + db.PaymentModes.Where(x => x.PMID == res.PaymentTypeID).Select(x => x.PMName).FirstOrDefault();
                res.PaymentType += (res.PaymentType == "Installment" ? (" No " + (res.NoOfInstallmentsPaid + 1)) : "");
                return res;
            }
        }

        internal static object OrderDelivered(int? orderID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                try
                {
                    var temp = db.ORDERS.Where(x => x.OID == orderID).FirstOrDefault();
                    temp.IsDelivered = true;
                    temp.DeliveredTime = DateTime.Now;
                    db.SaveChanges();
                    return "Success";
                }
                catch(Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }

        internal static List<CreateOrder> GetOrderHistory(int user,string role)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                List<CreateOrder> list = new List<CreateOrder>();
                var order =  role == "Buyer" ? db.ORDERS.Where(x => x.BuyerID == user).ToList() : db.ORDERS.Where(x => x.SupplierId == user).ToList();
                foreach (var item in order)
                {
                    var orderDetails = db.OrderDetails.Where(x => x.OrderID == item.OID).FirstOrDefault();
                    var payment = db.Payments.Where(x => x.OrderID == item.OID).FirstOrDefault();
                    var product = db.Products.Where(x => x.PID == orderDetails.ProductID).FirstOrDefault();
                    var paymentMode = db.PaymentModes.Where(x => x.PMID == payment.PaymentModeID).Select(x => x.PMName).FirstOrDefault();
                    var buyer = db.Users.Where(x => x.UID == item.BuyerID).FirstOrDefault();
                    list.Add(new CreateOrder
                    {
                        OrderID = item.OID,
                        Total = item.OTotal,
                        NooOfInstallment = payment.NoOfInstallment,
                        NooOfInstallmentPaid = payment.NoOfInstallmentPaid,
                        PaymentModeName = paymentMode,
                        Price = orderDetails.OPrice,
                        ProductName = product.Name,
                        DeliveryCharges = product.DeliveryCharges,
                        Quantity = orderDetails.Qunatity,
                        BuyerName = buyer.UName,
                        OrderTime = item.ODate,
                        Delivered = item.IsDelivered == true ? "YES" : "PENDING",
                        DeliverTime = item.DeliveredTime,
                        Status = (payment.NoOfInstallment == payment.NoOfInstallmentPaid) ? "Fully Paid" : "Not Fully Paid"
                    });
                }
                return list;
            }
        }

        internal static object GetPrdouctPrice(int productID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var res = db.Products.Where(x => x.PID == productID).Select(x => new 
                {
                    Price = x.PPrice,
                    DeliveryCharges = x.DeliveryCharges
                }).FirstOrDefault();
                return res;
            }
        }

        internal static dynamic GetPaymentMode()
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var temp = db.PaymentModes.ToList();
                List<SelectListItem> PaymentModes = new List<SelectListItem>() {
                    new SelectListItem { Text = "Select", Value = "Select" }
                };
                foreach (var item in temp)
                {
                    PaymentModes.Add(new SelectListItem { Text = item.PMName, Value = item.PMID.ToString() });
                }
                return PaymentModes;
            }
        }
    }
}
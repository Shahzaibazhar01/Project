using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.BusinessLogic
{
    public class BL_Product
    {
        internal static string SaveProdcut(Product obj, int supplierID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                try
                {
                    var product = new Product();
                    product.DeliveryCharges = obj.DeliveryCharges;
                    product.Name = obj.Name;
                    product.PPrice = obj.PPrice;
                    product.SupplierID = supplierID;
                    db.Products.Add(product);
                    db.SaveChanges();
                    return "Product Added Successfully";
                }
                catch(Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }

        internal static List<Product> GetProductList(int supplierID)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                return db.Products.Where(x => x.SupplierID == supplierID).ToList();
            }
        }
    }
}
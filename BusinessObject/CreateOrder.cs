using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Project.BusinessObject
{
    public class CreateOrder
	{
		public int? OrderID { get; set; }
		[DisplayName("Total")]
		public int? Total { get; set; }
		[DisplayName("Price")]
		public int? Price { get; set; }
		[DisplayName("Delivery Charges")]
		public int? DeliveryCharges { get; set; }
		[DisplayName("No Of Installment")]
		public int? NooOfInstallment { get; set; }
		public int? NooOfInstallmentPaid { get; set; }
		public int? Product { get; set; }
		public string ProductName { get; set; }
		public int? PaymentMode { get; set; }
		[DisplayName("PaymentMode")]
		public string PaymentModeName { get; set; }
		public int? Quantity { get; set; }
		public int? BuyerID { get; set; }
		public string BuyerName { get; set; }
		public string Received { get; set; }
		public string Delivered { get; set; }
		public DateTime? DeliverTime { get; set; }
		public DateTime? OrderTime { get; set; }
		public int? Supplier { get; set; }
		public string Status { get; set; }

	}
	public class ManagePayment
	{
		public int? PaymentTypeID { get; set; }
		public string PaymentType { get; set; }
		public int? NoOfInstallments { get; set; }
		public int? NoOfInstallmentsPaid { get; set; }
		public int? TotalPayment { get; set; }
		[DisplayName("Amount To Pay")]
		public int? AmountToPay { get; set; }
		public bool Paid { get; set; }
	}
}
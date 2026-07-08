using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TMCWD.Model.Billing.Interfaces;

namespace TMCWD.Model.Billing
{
    public class Billing : IBilling
    {
        public Billing() { }

        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Account Id")]
        public int AccountId { get; set; }

        [DisplayName("Job Order Id")]
        public int JobOrderId { get; set; }

        [DisplayName("Billing Reference Id")]
        public string BillingReferenceId { get; set; } = string.Empty;

        [DisplayName("Payment Transaction Id")]
        public string PaymentTransactionId { get; set; } = string.Empty;

        [DisplayName("Materials Amount")]
        public decimal MaterialsAmount { get; set; }

        [DisplayName("Penalties")]
        public decimal Penalties { get; set; }

        [DisplayName("Other Charges")]
        public decimal OtherCharges { get; set; }

        [DisplayName("Billing Adjustment")]
        public decimal BillingAdjustment { get; set; }

        [DisplayName("Total Bill Amount")]
        public decimal TotalBillAmount { get; set; }

        [DisplayName("Status")]
        public PaymentStatus PaymentStatus { get; set; }

        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Updated By")]
        public int UpdatedBy { get; set; }

        [DisplayName("Date Updated")]
        public DateTime DateUpdated { get; set; }
    }
}

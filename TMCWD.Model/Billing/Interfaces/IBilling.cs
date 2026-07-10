using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMCWD.Model.Billing.Interfaces
{
    public interface IBilling
    {

        public int Id { get; set; }

        public int AccountId { get; set; }

        public int JobOrderId { get; set; }

        public string BillingReferenceId { get; set; }

        public string PaymentTransactionId { get; set; }

        public decimal MaterialsAmount { get; set; }

        public decimal Penalties { get; set; }

        public decimal OtherCharges { get; set; }

        public decimal BillingAdjustment { get; set; }

        public decimal TotalBillAmount { get; set; }

        public PaymentType PaymentType { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}

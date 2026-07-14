using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TMCWD.Model.Billing.Interfaces;

namespace TMCWD.Model.Billing
{
    public class Billing : BillingBase
    {

        #region fields

        private decimal _totalPaid;
        private AdvancePayment _advancePayment;
        private decimal _unpaid;

        #endregion

        #region constructors

        public Billing() : base() 
        {
            _totalPaid = 0;
            _advancePayment = new();
            _unpaid = 0;
        }

        #endregion

        #region properties

        [DisplayName("Advance Payment")]
        public AdvancePayment AdvancePayment
        {
            get
            {
                return _advancePayment;
            }
        }

        [DisplayName("Unpaid Amount")]
        public decimal UnpaidAmount
        {
            get
            {
                return _unpaid;
            }
        }

        [DisplayName("Total Paid Amount")]
        public decimal TotalPaidAmount
        {
            get
            {
                return _totalPaid;
            }
        }

        #endregion

        #region old codes

        //[DisplayName("Id")]
        //public int Id { get; set; }

        //[DisplayName("Account Id")]
        //public int AccountId { get; set; }

        //[DisplayName("Job Order Id")]
        //public int JobOrderId { get; set; }

        //[DisplayName("Billing Reference Id")]
        //public string BillingReferenceId { get; set; } = string.Empty;

        //[DisplayName("Payment Transaction Id")]
        //public string PaymentTransactionId { get; set; } = string.Empty;

        //[DisplayName("Materials Amount")]
        //public decimal MaterialsAmount { get; set; }

        //[DisplayName("Penalties")]
        //public decimal Penalties { get; set; }

        //[DisplayName("Other Charges")]
        //public decimal OtherCharges { get; set; }

        //[DisplayName("Billing Adjustment")]
        //public decimal BillingAdjustment { get; set; }

        //[DisplayName("Total Bill Amount")]
        //public decimal TotalBillAmount { get; set; }

        //[DisplayName("Status")]
        //public PaymentStatus PaymentStatus { get; set; }

        //[DisplayName("Created By")]
        //public int CreatedBy { get; set; }

        //[DisplayName("Date Created")]
        //public DateTime DateCreated { get; set; }

        //[DisplayName("Updated By")]
        //public int UpdatedBy { get; set; }

        //[DisplayName("Date Updated")]
        //public DateTime DateUpdated { get; set; }

        #endregion

        public override void LoadPayments(List<PaymentBase> payments, AdvancePayment advancePayment)
        {
            _totalPaid = ComputeTotalPayments(payments);
            _advancePayment = advancePayment;
            _unpaid = TotalBillAmount - (_totalPaid + _advancePayment.Amount);
            if (_unpaid <= 0)
            {
                _unpaid = 0;
                _advancePayment.Amount = Math.Abs(_unpaid);
            }
            else
            {
                _advancePayment.Amount = 0;
            }
        }

        private decimal ComputeTotalPayments(List<PaymentBase> payments)
        {
            decimal totalPayments = 0;

            foreach (var payment in payments)
            {
                totalPayments += payment.PaidAmount;
            }

            return totalPayments;
        }

    }
}

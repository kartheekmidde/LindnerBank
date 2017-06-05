using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Models
{
    public class Transaction
    {

  //- A debit cannot be for more that the current account balance

        public int ID { get; set; }
        //public int AccountID { get; set; }
        //public string Account { get; set; }

        [CustomValidation(typeof(Transaction), "TrDateCantBeInFuture")]
        [CustomValidation(typeof(Transaction), "TrDateCantBeBeforeCurrentYear")]
        public DateTime TransactionDate { get; set; }

        public static ValidationResult TrDateCantBeInFuture(DateTime transactionDate, ValidationContext context)
        {
            if (transactionDate > DateTime.Today)
            {
                return new ValidationResult("Transaction date cannot be in the future.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult TrDateCantBeBeforeCurrentYear(DateTime transactionDate, ValidationContext context)
        {
            if (transactionDate.Year < DateTime.Today.Year)
            {
                return new ValidationResult("Transaction date cannot be before current year.");
            }
            return ValidationResult.Success;
        }

        [CustomValidation(typeof(Transaction), "TrAmountCantBeBeZero")]
        public double Amount { get; set; }

        public static ValidationResult TrAmountCantBeBeZero(double amount, ValidationContext context)
        {
            if (amount == 0.00)
            {
                return new ValidationResult("Transaction amount cannot be 0.");
            }
            return ValidationResult.Success;
        }

        [Required(ErrorMessage = "Note is required.")]
        public string Note { get; set; }

        public virtual Account Account { get; set; }
        public virtual int AccountId { get; set; }
        //public string Username { get; set; }

        public double DisplayPrincipalPlusInterest()
        {
            double interest = Account.yearToDateInterest(TransactionDate, Amount);
            return interest; // Delete after you have implemented your method
        }
    }
}
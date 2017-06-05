using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Models
{
    public class Account
    {

        public Account()
        {
            Transactions = new List<Transaction>();
            WishListItems = new List<WishListItem>();
        }

        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<WishListItem> WishListItems { get; set; }

        public int ID { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Owner { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Recipient { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime OpenDate { get; set; }

        [Range(0, 100, ErrorMessage = "Interest rate should be between 0 and 100.")]
        public double InterestRate { get; set; }


        public static ValidationResult OwnerRecptCantBeSame(Account account, ValidationContext context)
        {

            if (account.Owner == account.Recipient)
            {
                return new ValidationResult("Owner and recipient cannot be same email address.");
            }
            return ValidationResult.Success;
        }

        public double yearToDateInterest(DateTime tDate, double tAmount)
        {
            //A=P(1+​r/n)^nt
            double p = tAmount;
            double rByN = (InterestRate / 100) / 12;

            DateTime firstDay = new DateTime(DateTime.Now.Year, 1, 1);
            int n = 12;
            double t = ((tDate - firstDay).Days);

            double amount = p * (Math.Pow((1 + rByN), ((n * t)/365)));

            return amount;
        }

        //public double yearToDateInterest(Transaction transaction)
        //{
        //    //A=P(1+​r/n)^nt
        //    double p = transaction.Amount;
        //    double rByN = (InterestRate / 100) / 12;

        //    DateTime firstDay = new DateTime(DateTime.Now.Year, 1, 1);
        //    int n = 12;
        //    int t = ((transaction.TransactionDate - firstDay).Days) / 365;

        //    double amount = p * Math.Pow((1 + rByN), n * t);

        //    return amount;
        //}
    }
}
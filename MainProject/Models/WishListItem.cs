using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Models
{
    public class WishListItem
    {

  //- A recipient cannot delete a wish list item

        public WishListItem()
        {
            DateAdded = DateTime.Now;
        }
        public int Id { get; set; }
        //public int AccountID { get; set; }
        //public string Account { get; set; }
        public DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "Cost is required.")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Url(ErrorMessage = "Url should be valid.")]
        public string Link { get; set; }
        public Boolean Purchased { get; set; }

        public virtual Account Account { get; set; }
        public virtual int AccountId { get; set; }
    }
}
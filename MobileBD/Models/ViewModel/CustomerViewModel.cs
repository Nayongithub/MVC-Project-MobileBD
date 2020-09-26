using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MobileBD.Models.ViewModel
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please enter your name.")]

        [Display(Name = "Full Name")]
        [StringLength(128)]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please select your gender."), StringLength(15)]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter your store name.")]

        [Display(Name = "Store Name")]
        [StringLength(128)]
        public string StoreName { get; set; }
        public string StoreId { get; set; }

        [Required(ErrorMessage = "Please enter number.")]
        [StringLength(11)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "Invalid Phone number. Example:01xxxxxxxxx")]
        public string Number { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter correct email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your membership date.")]

        [Display(Name = "Membership")]
        [DataType(DataType.DateTime)]
        
        public DateTime Membership { get; set; }
        [Required(ErrorMessage = "Please enter your store address.")]

        [Display(Name = "Store Address")]
        [StringLength(250)]
        public string StoreAddress { get; set; }
        public string Picture { get; set; }
        [Required(ErrorMessage = "Please insert a picture.")]

        public HttpPostedFileBase UserImageFile { get; set; }
        //public virtual IList<Product> Products { get; set; }
        [Required(ErrorMessage = "Please select location")]
        [ForeignKey("OurLocation")]
        public int OurLocationID { get; set; }

        //nav
        public virtual OurLocation OurLocation { get; set; }
    }
}
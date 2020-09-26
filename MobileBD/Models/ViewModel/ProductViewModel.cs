using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MobileBD.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Please enter your name."), Display(Name = "Full Name")]
        [StringLength(128)]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please select your gender."), StringLength(15)]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please enter phone number."), Display(Name = "Phone")]
        [StringLength(11)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{5})$", ErrorMessage = "Invalid Phone number. Example:01xxxxxxxxx")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Please enter email address."), StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter correct email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your mobile name."), Display(Name = "Mobile Full Name")]
        [StringLength(60)]
        public string MobileFullName { get; set; }
        [Required(ErrorMessage = "Please select condition.")]
        [EnumDataType(typeof(Condition))]
        public Condition Condition { get; set; }
        [Required]
        public bool Sell { get; set; }
        [Required]
        public bool Exchange { get; set; }
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please enter your address.")]
        [StringLength(250)]
        public string Address { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Post Date")]
        public DateTime? PostDate { get; set; }
        [Required(ErrorMessage = "Please enter your description.")]
        [StringLength(2000)]
        public string Description { get; set; }
        public string Picture { get; set; }
        //[Required(ErrorMessage ="Image type(JPG, JPEG, PNG) or maximum size(4MB) is invalid")]

        public HttpPostedFileBase UserImageFile { get; set; }
        public string StoreId { get; set; }
        public bool ForSlider { get; set; }

        //FK
        [Required(ErrorMessage = "Please select brand name")]
        [ForeignKey("MobileBrand")]
        public int MobileBrandID { get; set; }
        [Required(ErrorMessage = "Please select location")]
        [ForeignKey("OurLocation")]
        public int OurLocationID { get; set; }
        //public string Tanure
        //{
        //    get
        //    {
        //        var y = (DateTime.Now - PostDate.Value).Days / 365;
        //        //int m = ((DateTime.Now - this.JoinDate).Days % 365) / 30;
        //        //return $"{y} y {m} m";
        //        return $"{y} y";
        //    }
        //}
        //nav
        public virtual MobileBrand MobileBrand { get; set; }
        public virtual OurLocation OurLocation { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel;


namespace MobileBD.Models
{
    
    public enum Condition
    {
        New = 1,
        Used = 2
    }
    public class Role
    {
        public int RoleID { get; set; }
        [Required]
        public string RoleName { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [ForeignKey("Role")]
        public int? RoleID { get; set; }
        //nav
        public virtual Role Role { get; set; }
    }
    public class MobileBrand
    {
        public int MobileBrandID { get; set; }

        [Display(Name = "Brand Name")]
        [StringLength(60)]
        public string MobileBrandName { get; set; }
        //nav
        public virtual IList<Product> Products { get; set; }
    }
    public class OurLocation
    {
        public int OurLocationID { get; set; }

        [Display(Name = "Location Name")]
        [StringLength(60)]
        public string OurLocationName { get; set; }
        //nav
        public virtual IList<Product> Products { get; set; }
        public virtual IList<Customer> Customers { get; set; }
    }
    public class Product
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
        public string StoreId { get; set; }
        public bool ForSlider { get; set; }


        //FK
        [Required(ErrorMessage = "Please select brand name")]
        [ForeignKey("MobileBrand")]
        public int MobileBrandID { get; set; }
        [Required(ErrorMessage = "Please select location")]

        [ForeignKey("OurLocation")]
        public int OurLocationID { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }

        //nav
        public virtual MobileBrand MobileBrand { get; set; }
        public virtual OurLocation OurLocation { get; set; }
        public virtual Customer Customer { get; set; }

    }

    public class Customer
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
        [StringLength(100)]
        [Required(ErrorMessage = "Please insert a picture.")]

        public string Picture { get; set; }
        public virtual IList<Product> Products { get; set; }
        [Required(ErrorMessage = "Please select location")]
        [ForeignKey("OurLocation")]
        public int OurLocationID { get; set; }

        //nav
        public virtual OurLocation OurLocation { get; set; }


    }

    public class MobileDbContext : DbContext
    {
        public MobileDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MobileBrand> MobileBrands { get; set; }
        public DbSet<OurLocation> OurLocations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<MobileDbContext>
    {

        protected override void Seed(MobileDbContext context)
        {

            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "ASUS" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "GOOGLE" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "HONOR" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "HTC" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "INFINIX" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "IPHONE" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "LENOVO" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "LG" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "MOTOROLA" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "ONEPLUS" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "OPPO" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "PANASONIC" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "SAMSUNG" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "SONY" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "TECNO" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "WALTON" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "XIAOMI" });
            context.MobileBrands.Add(new MobileBrand { MobileBrandName = "OTHER" });


            context.OurLocations.Add(new OurLocation { OurLocationName = "Barishal" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Chattogram" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Dhaka" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Khulna" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Mymensingh" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Rajshahi" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Rangpur" });
            context.OurLocations.Add(new OurLocation { OurLocationName = "Sylhet" });


            context.Roles.Add(new Role { RoleName = "Admin" });
            context.Roles.Add(new Role { RoleName = "Employee" });
            context.Roles.Add(new Role { RoleName = "Customer" });


            context.Users.Add(new User { Name= "Admin", Password="admin123",RoleID=1});
            context.Users.Add(new User { Name= "Employee", Password= "employee123", RoleID=2});
            context.Users.Add(new User { Name= "Customer", Password="customer123",RoleID=3});

            
            context.SaveChanges();

        }

    }

}
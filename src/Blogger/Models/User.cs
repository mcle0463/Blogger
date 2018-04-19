using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Models
{
    public class User
    {
        public int UserId
        {
            get;
            set;
        }
        public int RoleId
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string FirstName
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string LastName
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string EmailAddress
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string Password
        {
            get;
            set;
        }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth
        {
            get;
            set;
        }
        [StringLength(1000)]
        public string City
        {
            get;
            set;
        }
        [StringLength(1000)]
        public string Address
        {
            get;
            set;
        }
        [StringLength(1000)]
        public string PostalCode
        {
            get;
            set;
        }
        [StringLength(1000)]
        public string Country
        {
            get;
            set;
        }

    }
}

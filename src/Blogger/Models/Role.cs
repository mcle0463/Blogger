using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Models
{
    public class Role
    {
        public int RoleId
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string Name
        {
            get;
            set;
        }

    }
}

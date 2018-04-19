using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Models
{
    public class BlogPost
    {
        public int BlogPostId
        {
            get;
            set;
        }
        public int UserId
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string Title
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string Content
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string ShortDescription
        {
            get;
            set;
        }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Posted
        {
            get;
            set;
        }
        [Required]
        public Boolean isAvailable
        {
            get;
            set;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Models
{
    public class Photo
    {
        public int PhotoId
        {
            get;
            set;
        }
        public int BlogPostId
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string FileName
        {
            get;
            set;
        }
        [Required]
        [StringLength(1000)]
        public string Url
        {
            get;
            set;
        }

    }
}

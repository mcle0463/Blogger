using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Models
{
    public class Comment
    {
        public int CommentId
        {
            get;
            set;
        }   
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
        public string Content
        {
            get;
            set;
        }
        public int Rating
        {
            get;
            set;
        }

    }
}

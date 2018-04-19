using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Models
{
    public class BlogPostViewModel
    {
        public BlogPost Post
        {
            get;
            set;
        }
        public User User
        {
            get;
            set;
        }

        public List<Comment>
    Comments
        {
            get;
            set;
        }
        public List<Photo>
   Photos
        {
            get;
            set;
        }
        public ICollection<IFormFile>
            Files
        {
            get;
            set;
        }
        public Comment Comment
        {
            get;
            set;
        }
    }
}

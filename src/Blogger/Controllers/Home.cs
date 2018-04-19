using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Blogger.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Blogger.Controllers
{
    public class Home : Controller
    {
        private BlogDataContext _Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Home(BlogDataContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue && HttpContext.Session.GetInt32("RoleId") == 1)
            {
                return View(_Context.BlogPosts.ToList());
            }
            else
            {
                List<BlogPost> availablePosts = (from c in _Context.BlogPosts where c.isAvailable select c).ToList();
                return View(availablePosts);
            }
        }
        public IActionResult ViewBadWords()
        {
            return View(_Context.BadWords.ToList());
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult CreateUser(User user)
        {
            _Context.Users.Add(user);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CreateBlogPost(BlogPostViewModel userPostValues)
        {

            BlogPost blog_post = new Models.BlogPost();
            blog_post = userPostValues.Post;

            String userPost = userPostValues.Post.Content;
            List<BadWord> badWords = _Context.BadWords.ToList();
            foreach (var word in badWords)
            {
                userPost = userPost.Replace(word.Word, "*****");
            }
            blog_post.Content = userPost;
            blog_post.UserId = Int32.Parse(Request.Form["UserId"]);

            _Context.BlogPosts.Add(userPostValues.Post);
            _Context.SaveChanges();


            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=cst8359;AccountKey=ecMPpNU6vimZKMDTJG4seALrY7Kq7UJYjgl0/yLanXn857C8xtUJ2sF4ciB6wy9gg+e/YeYbRTaly2DVOxWhXQ==");
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("justinsphotostorage");
            await container.CreateIfNotExistsAsync();
            // set the permissions of the container to 'blob' to make them public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);
            // for each file that may have been sent to the server from the client
            if (userPostValues.Files != null)
            {
                foreach (var file in userPostValues.Files)
                {
                    try
                    {
                        // create the blob to hold the data
                        var blockBlob = container.GetBlockBlobReference(file.FileName);
                        if (await blockBlob.ExistsAsync())
                            await blockBlob.DeleteAsync();

                        using (var memoryStream = new MemoryStream())
                        {
                            // copy the file data into memory
                            await file.CopyToAsync(memoryStream);

                            // navigate back to the beginning of the memory stream
                            memoryStream.Position = 0;

                            // send the file to the cloud
                            await blockBlob.UploadFromStreamAsync(memoryStream);
                        }

                        // add the photo to the database if it uploaded successfully
                        var photo = new Photo();
                        photo.Url = blockBlob.Uri.AbsoluteUri;
                        photo.FileName = file.FileName;
                        photo.BlogPostId = blog_post.BlogPostId;

                        _Context.Photos.Add(photo);
                        _Context.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }


            _Context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult DeleteImageNow(int id)
        {
            var imageToDelete = (from p in _Context.Photos where p.PhotoId == id select p).FirstOrDefault();
            var blogPostIndex = imageToDelete.BlogPostId;
            _Context.Remove(imageToDelete);
            _Context.SaveChanges();

            return RedirectToAction("EditBlogPost", new { id = blogPostIndex });
        }
        public IActionResult CreateComment()
        {
            Comment comment = new Models.Comment();

            String userComment = Request.Form["Content"];
            List<BadWord> badWords = _Context.BadWords.ToList();
            foreach (var word in badWords)
            {
                userComment = userComment.Replace(word.Word, "*****");
            }

            comment.Content = userComment;
            comment.BlogPostId = Int32.Parse(Request.Form["BlogPostId"]);
            comment.UserId = Int32.Parse(Request.Form["UserId"]);

            _Context.Comments.Add(comment);

            _Context.SaveChanges();
            return RedirectToAction("DisplayFullBlogPost", new { id = comment.BlogPostId });

        }
        public IActionResult CreateBadWord()
        {
            BadWord badWord = new Models.BadWord();



            badWord.Word = Request.Form["Word"];


            _Context.BadWords.Add(badWord);

            _Context.SaveChanges();
            return RedirectToAction("ViewBadWords");

        }
        public IActionResult EditBlogPost(int id)
        {
            BlogPostViewModel blogPostToUpdate = new BlogPostViewModel();
            blogPostToUpdate.Post = (from c in _Context.BlogPosts where c.BlogPostId == id select c).FirstOrDefault();
            blogPostToUpdate.Photos = (from c in _Context.Photos where c.BlogPostId == blogPostToUpdate.Post.BlogPostId select c).ToList();

            return View(blogPostToUpdate);
        }
        public IActionResult EditProfile(int id)
        {
            var userToUpdate = (from c in _Context.Users where c.UserId == id select c).FirstOrDefault();
            return View(userToUpdate);
        }
        public IActionResult DisplayFullBlogPost(int id)
        {
            var BlogPostViewModel = new BlogPostViewModel();
            BlogPostViewModel.Post = (from c in _Context.BlogPosts where c.BlogPostId == id select c).FirstOrDefault();
            BlogPostViewModel.User = (from c in _Context.Users where c.UserId == BlogPostViewModel.Post.UserId select c).FirstOrDefault();
            BlogPostViewModel.Comments = (from c in _Context.Comments where c.BlogPostId == BlogPostViewModel.Post.BlogPostId select c).ToList();
            BlogPostViewModel.Photos = (from c in _Context.Photos where c.BlogPostId == BlogPostViewModel.Post.BlogPostId select c).ToList();


            return View(BlogPostViewModel);
        }

        public IActionResult LoginUser(User user)
        {
            var UserToLogin = (from c in _Context.Users where c.EmailAddress.Equals(user.EmailAddress) select c).FirstOrDefault();
            if (UserToLogin.Password.Equals(user.Password))
            {
                HttpContext.Session.SetInt32("UserId", UserToLogin.UserId);
                HttpContext.Session.SetInt32("RoleId", UserToLogin.RoleId);
                HttpContext.Session.SetString("FirstName", UserToLogin.FirstName);
                HttpContext.Session.SetString("LastName", UserToLogin.LastName);
            }
            return RedirectToAction("Index");
        }


        public IActionResult ModifyBlogPost(BlogPost post)
        {
            var id = Convert.ToInt32(Request.Form["BlogPostId"]);
            var blogPostToUpdate = (from m in _Context.BlogPosts where m.BlogPostId == id select m).FirstOrDefault();
            blogPostToUpdate.Title = post.Title;
            blogPostToUpdate.Content = post.Content;
            blogPostToUpdate.Posted = post.Posted;
            blogPostToUpdate.isAvailable = post.isAvailable;

            _Context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult ModifyUser(User user)
        {
            var id = Convert.ToInt32(Request.Form["userId"]);
            var userToUpdate = (from m in _Context.Users where m.UserId == id select m).FirstOrDefault();
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Password = user.Password;
            userToUpdate.EmailAddress = user.EmailAddress;
            userToUpdate.Address = user.Address;
            userToUpdate.City = user.City;
            userToUpdate.Country = user.Country;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.RoleId = user.RoleId;
            userToUpdate.DateOfBirth = user.DateOfBirth;


            _Context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult DeleteBlogPost(int id)
        {
            var blogPostToUpdate = (from m in _Context.BlogPosts where m.BlogPostId == id select m).FirstOrDefault();
            _Context.Remove(blogPostToUpdate);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult DeleteBadWord(int id)
        {
            var badWordUpdate = (from m in _Context.BadWords where m.BadWordId == id select m).FirstOrDefault();
            _Context.Remove(badWordUpdate);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
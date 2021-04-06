using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(
            IPostRepository postRepository, 
            ICategoryRepository categoryRepository, 
            IUserProfileRepository userProfileRepository,
            ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userProfileRepository = userProfileRepository;
            _tagRepository = tagRepository;
        }


        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPosts(int userProfileId)
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllUserPosts(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            post.PostTags = _tagRepository.GetPostTags(id);
            if (post == null)
            {
                return NotFound();
            }          
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // GET: PostController/Edit/#
        [Authorize]
        public ActionResult Edit(int id)
        {
            int currentUserId = GetCurrentUserProfileId();
            List<Category> categories = _categoryRepository.GetAll();
            UserProfile userProfile = _userProfileRepository.GetById(currentUserId);
            Post post = _postRepository.GetPublishedPostById(id);

            if ((post == null || currentUserId != post.UserProfileId) && userProfile.UserTypeId != 1)
            {
                return NotFound();
            }
            else
            {
                var vm = new PostCreateViewModel()
                {
                    Post = post,
                    CategoryOptions = categories
                };

                return View(vm);
            }
        }

        // POST: PostController/Edit/#
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(post);
            }
        }

        // GET: PostController/Delete/#
        [Authorize]
        public ActionResult Delete(int id)
        {
            int currentUserId = GetCurrentUserProfileId();
            UserProfile userProfile = _userProfileRepository.GetById(currentUserId);

            Post post = _postRepository.GetPublishedPostById(id);

            if (post.UserProfileId != currentUserId && userProfile.UserTypeId != 1)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: PostController/Delete/#
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(post);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

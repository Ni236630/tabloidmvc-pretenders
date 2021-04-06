using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {

        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;

        public TagController(ITagRepository tagRepository, IPostRepository postRepository)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
        }

        // GET: TagController
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepository.GetAll();

            return View(tags);
        }

        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.AddTag(tag);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tag tag)
        {
            try
            {
                _tagRepository.EditTag(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tag);
            }
        }

        // GET ManageTags
        public ActionResult ManageTags(Post post)
        {
            List<Tag> Tags = _tagRepository.GetAll();
            List<SelectListItem> SelectedTags = new List<SelectListItem>();
            
            foreach (Tag tag in Tags)
            {
                var selectList = new SelectListItem()
                {
                    Text = tag.Name,
                    Value = tag.Id.ToString(),
                    Selected = false
                };
                SelectedTags.Add(selectList);
            }
            var vm = new ManageTagViewModel()
            {
                PostId = post.Id,
                Tags = SelectedTags
            };

            return View(vm);
        }

        // POST ManageTags
        [HttpPost]
        public ActionResult ManageTags(List<SelectListItem> selectedTags, Post post)
        {
            try
            {
                _tagRepository.DeleteAllPostTags(post.Id);
                foreach(var item in selectedTags)
                {
                    int tagId = Int32.Parse(item.Value);
                   _tagRepository.AddPostTags(tagId, post.Id);
                }
                return RedirectToAction("Details", "Post", new {id = post.Id});
            }
            catch
            {
                return NotFound();
            }
        }
    }
}

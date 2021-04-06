using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TabloidMVC.Models.ViewModels
{
    public class ManageTagViewModel
    {
        public int PostId { get; set; }
        public List<Tag> SelectedTags { get; set; }
        public List<SelectListItem> Tags { get; set; }
    }
}

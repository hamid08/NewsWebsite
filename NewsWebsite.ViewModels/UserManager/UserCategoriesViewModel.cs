using NewsWebsite.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsWebsite.ViewModels.UserManager
{
    public class UserCategoriesViewModel
    {
        public UserCategoriesViewModel(List<TreeViewCategory> categories, string[] categoryIds)
        {
            Categories = categories;
            CategoryIds = categoryIds;
        }

        public List<TreeViewCategory> Categories { get; set; }
        public string[] CategoryIds { get; set; }
    }
}

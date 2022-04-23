using Press_Agency_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.ViewModels
{
    public class PostAndArticleTypeViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<ArticleType> ArticleTypes { get; set; }
    }
}
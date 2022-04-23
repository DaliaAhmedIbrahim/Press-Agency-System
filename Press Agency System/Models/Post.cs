using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Enter your name")]
        [Display(Name ="Writer Name")]
        public string EditorName { get; set; }

        [Required(ErrorMessage = "Enter article title")]
        [Display(Name = "Article Title")]
        public string ArticleTitle { get; set; }

        [Required(ErrorMessage = "Write the post")]
        [Display(Name = "Article Content")]
        public string ArticalBody { get; set; }

        public DateTime CreationDate { get; set; }
        
        public ArticleType ArticleType{ get; set; }

        [Required(ErrorMessage = "Enter the article type")]
        [Display(Name = "Article Type")] 
        public int ArticleTypeId { get; set; }

        [Display(Name ="Number of viewers")]
        public int ViewersNumber { get; set; }

        [Display(Name ="Question")]
        public string Question { get; set; }

        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Article Image")]
        public string Image { get; set; }

        public bool Accepted { get; set; }

        public int Likes { get; set; }
        public int DisLikes { get; set; }




    }
}
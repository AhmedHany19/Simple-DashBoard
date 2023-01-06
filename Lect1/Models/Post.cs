using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lect1.Models
{
    public class Post
    {
        [Display(Name ="#")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Category Title Cann't Be Empty")]
        public string Title { get; set; }
        public DateTime UpdateDate { get; set; }

        [ForeignKey("Category")]
        [Display(Name ="Category")]
        public int CatId { get; set; }
        [Required]
        [StringLength(200)]
        [AllowHtml]
        public string Body { get; set; }
        public string ImgPath { get; set; }
        [NotMapped]
        public HttpPostedFileBase PostImage { get; set; }


        // Navigation Properties
        public virtual Category Category { get; set; }


    }
}
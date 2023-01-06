using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lect1.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category title cann't be empty !!")]
        [Display(Name ="Category")]
        public string Title { get; set; }
        public DateTime UpdateDate { get; set; }

        // Navigation Property

        public virtual ICollection<Post> Posts { get; set; }

    }
}
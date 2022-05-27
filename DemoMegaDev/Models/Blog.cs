using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMegaDev.Models
{
    [Table("blog_posts", Schema = "md")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("creation_date")]
        [Display(Name = "Creation date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Required]
        [Column("last_updated")]
        [Display(Name = "Last updated")]
        [DataType(DataType.Date)]
        public DateTime LastUpdate { get; set; }

        [Required(ErrorMessage = "Please insert a headline")]
        public string Headline { get; set; }

        public string Excerpt { get; set; }

        [Required]
        public string Article { get; set; }

        [Column("schedule_date")]
        [Display(Name = "Schedule date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime ScheduleDate { get; set; }

        [Required(ErrorMessage = "Please insert a language")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The laguage shout be 2 cases long")]
        [Column("language_id")]
        [Display(Name = "Language")]
        public string LanguageId { get; set; }

        [Column("meta_description")]
        [Display(Name = "Meta description")]
        public string MetaDescription { get; set; }

        [Column("main_post")]
        public int? MainBlogId { get; set; }

        [Display(Name = "Parient blog")]
        public Blog MainBlog { get; set; }

        [Display(Name = "Title image")]
        public Image Image { get; set; }

        [Display(Name = "Title image")]
        [NotMapped]
        public IFormFile FormImage { get; set; }
    }
}

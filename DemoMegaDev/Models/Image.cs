using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMegaDev.Models
{
    [Table("blog_title_image", Schema = "md")]
    public class Image
    {
        [Key]
        [Column("blog_id")]
        [Display(Name = "Blog id")]
        public int BlogId { get; set; }

        [Column("title_image_file")]
        [Required]
        [Display(Name = "Image file")]
        public string File { get; set; }

        [Column("alt_title")]
        [Display(Name = "Alt title")]
        public string AltTitle { get; set; }

        [Column("last_change_date")]
        [Display(Name = "Last change date")]
        [Required]
        public DateTime LastChangeDate { get; set; }

        public Blog Blog { get; set; }
    }
}

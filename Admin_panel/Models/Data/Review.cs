using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class Review
    {
        [Key]
        public int pr_id {  get; set; }
        [Required(ErrorMessage ="Enter Product Review")]
        [Column(TypeName ="Varchar(max)")]
        public string pr_msg {  get; set; }
        [Required]
        public int prd_id {  get; set; }
        [ForeignKey("prd_id")]
        public virtual Product prod { get; set; }
        public string user_id {  get; set; }
        [ForeignKey("user_id")]
        public ApplicationUser user { get; set; }
        public DateTime r_date { get; set; }
        [NotMapped]
        public IEnumerable<Review> Rlist {  get; set; }
    }
}

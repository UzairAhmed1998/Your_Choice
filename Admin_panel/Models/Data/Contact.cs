using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class Contact
    {
        [Key]
        public int c_id { get; set; }
        [Required(ErrorMessage ="Enter Name")]
        [Column(TypeName ="Varchar(50)")]
        public string c_name { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        [Column(TypeName = "Varchar(50)")]
        [EmailAddress]
        public string c_email { get; set; }
        [Required(ErrorMessage = "Enter Message")]
        [Column(TypeName = "Varchar(max)")]
        public string c_msg { get; set; }
    }
}

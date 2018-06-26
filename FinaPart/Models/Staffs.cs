using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("Staff", Schema = "book")]
    public class Staffs
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
}
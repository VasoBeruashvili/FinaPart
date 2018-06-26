using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("Users", Schema = "book")]
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        [ForeignKey("staff")]
        public int? staff_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int? group_id { get; set; }

        
        [JsonProperty("staff")]
        public virtual Staffs staff { get; set; }
    }
}
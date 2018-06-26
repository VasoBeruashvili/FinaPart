using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("Currencies", Schema = "book")]
    public class Currencies
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("rate")]
        public double? Rate { get; set; }

        [Column("is_auto")]
        public int IsAuto { get; set; }
    }
}
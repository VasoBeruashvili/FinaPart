using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinaPart.Models
{
    [Table("SubContragents", Schema = "book")]
    public class SubContragent
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("contragent_id")]
        public int? ContragentId { get; set; }

        [Column("usr_column_501")]
        public string Name { get; set; }

        [Column("usr_column_502")]
        public string Address { get; set; }
    }
}
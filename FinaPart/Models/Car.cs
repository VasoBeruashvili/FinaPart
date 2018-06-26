using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaPart.Models
{
    [Table("Cars", Schema = "book")]
    public class Car
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("model")]
        public string Model { get; set; }

        [Column("num")]
        public string Num { get; set; }

        [Column("driver")]
        public string Driver { get; set; }

        [Column("fuel")]
        public double? Fuel { get; set; }

        [Column("driver_num")]
        public string DriverNum { get; set; }

        [Column("staff_id")]
        public int? StaffId { get; set; }

        [Column("type")]
        public int? Type { get; set; }

        [Column("trailer")]
        public string Trailer { get; set; }
    }
}

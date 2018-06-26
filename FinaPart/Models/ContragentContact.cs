using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinaPart.Models
{
    [Table("ContragentContacts", Schema = "book")]
    public class ContragentContact
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("contragent_id")]
        public int? ContragentId { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("tel")]
        public string Tel { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("position")]
        public string Position { get; set; }

        [Column("send_sms")]
        public bool SendSms { get; set; }
    }
}
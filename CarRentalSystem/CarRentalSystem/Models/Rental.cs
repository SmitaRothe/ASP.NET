using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalSystem.Models
{
   
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        public int CarId { get; set; }

        public int CustomerId { get; set; }
       
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? RentalDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString="{0:dd/MM/yyyy}")]
        public DateTime? ReturnDate { get; set; }

        public float TotalFee { get; set; }
        public string Comments { get; set; }
        public Car Car { get; set; }
        
        public Customer Customer { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalSystem.Models
{
    [Table("Car")]
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        [StringLength(30)]
        public string RegNum { get; set; }

        [Required]
        [StringLength(30)]
        public string Make { get; set; }

        [Required]
        [StringLength(30)]
        public string Model { get; set; }

        [Required]
        [StringLength(30)]
        public string CarYear { get; set; }

        [Required]
        [StringLength(30)]
        public string CarCategory { get; set; }

        [Required]
        public int PassengerCapacity { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public float RentalFee { get; set; }
        public byte[] Photo { get; set; }
        
        //public virtual Rental Rental { get; set; }
        //public virtual ICollection<Rental> Rentals { get; set; }

    }
}
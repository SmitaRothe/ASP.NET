using CarRentalSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalSystem.ViewModel
{
    public class CarViewModel
    {
        public int CarId { get; set; }

        [Display(Name = "Registration no")]
        public string RegNum { get; set; }
      
        public string Make { get; set; }

        public string Model { get; set; }

        [Display(Name = "Car Year")]
        public string CarYear { get; set; }

        [Display(Name = "Car Category")]
        public string CarCategory { get; set; }

        [Display(Name = "Capacity")]
        public int PassengerCapacity { get; set; }

        public bool IsAvailable { get; set; }

        [Display(Name = "Rental Fee")]
        public float RentalFee { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoDB { get; set; }

        //public virtual ICollection<Rental> Rentals { get; set; }
    }
}
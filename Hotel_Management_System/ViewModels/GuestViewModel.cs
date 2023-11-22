using Hotel_Management_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Hotel_Management_System.ViewModels
{
    public class GuestViewModel
    {
        public int GuestId { get; set; }
        [Required, StringLength(50), Display(Name = "Guest Name")]
        public string Guest_Name { get; set; }
        [Required, StringLength(15)]
        public string Contact_Number { get; set; }
        [Required]
        public HttpPostedFileBase Picture { get; set; }
        [Required, StringLength(150)]
        public string Address { get; set; }
        [Required, StringLength(50)]
        public string City { get; set; }
        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
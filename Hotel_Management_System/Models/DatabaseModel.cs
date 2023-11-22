using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hotel_Management_System.Models
{
    public enum RoomType
    {
        Standard = 1,
        Deluxe,
        Suite,
        Premium
    }
    public class Guest
    {
        public int GuestId { get; set; }
        [Required, StringLength(50), Display(Name ="Guest Name")]
        public string Guest_Name { get; set; }
        [Required, StringLength(15)]
        public string Contact_Number { get; set; }
        [Required, StringLength(150)]
        public string Picture { get; set; }
        [Required, StringLength(150)]
        public string Address { get; set; }
        [Required, StringLength(50)]
        public string City { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
    public class Reservation
    {
        [Key]
        public int Reservation_Id { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Check_in_Date { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Check_out_Date { get; set; }
        [Required]
        public int Room_Number { get; set; }
        [EnumDataType(typeof(RoomType))]
        public RoomType Room_Type { get; set; }
        public bool IsCancelled { get; set; }
        [Required, ForeignKey("Guest")]
        public int GuestId { get; set; }
        public virtual Guest Guest { get; set; }
    }
    public class HotelDbContext : DbContext
    {
        public HotelDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Guest> Guests { get; set; }
    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<HotelDbContext>
    {

        protected override void Seed(HotelDbContext context)
        {
            Guest b = new Guest { Guest_Name = "Ebong Himu", Contact_Number = "Humayun Ahmed", Picture = "1.jpg", Address = "Mirpur", City = "Dhaka" };
            b.Reservations.Add(new Reservation { Check_in_Date = DateTime.Parse("08-20-2023"), Check_out_Date = DateTime.Parse("08-20-2023"), Room_Number = 01728007783, Room_Type = RoomType.Premium, IsCancelled = false });
            context.Guests.Add(b);
            context.SaveChanges();
        }
    }
}
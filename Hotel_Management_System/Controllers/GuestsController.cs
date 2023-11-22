using Hotel_Management_System.Models;
using Hotel_Management_System.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Hotel_Management_System.Controllers
{
    [Authorize]
    public class GuestsController : Controller
    {
        private readonly HotelDbContext db = new HotelDbContext();
        // GET: Guests
        [AllowAnonymous]
        public async Task<ActionResult> Index(int pg = 1)
        {
            var data = await db.Guests.OrderBy(a => a.GuestId).ToPagedListAsync(pg, 5);
            return View(data);
        }
        public ActionResult Create()
        {

            GuestViewModel b = new GuestViewModel();
            b.Reservations.Add(new Reservation { });
            return View(b);
        }
        [HttpPost]
        public ActionResult Create(GuestViewModel data, string act = "")
        {
            if (act == "add")
            {
                data.Reservations.Add(new Reservation { });

                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Reservations.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var b = new Guest
                    {
                        Guest_Name = data.Guest_Name,
                        Contact_Number = data.Contact_Number,
                        Address = data.Address,
                        City = data.City
                        
                    };
                    string ext = Path.GetExtension(data.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Server.MapPath("~/Pictures/") + fileName;
                    data.Picture.SaveAs(savePath);
                    b.Picture = fileName;
                    foreach (var l in data.Reservations)
                    {

                        b.Reservations.Add(l);
                    }
                    db.Guests.Add(b);
                    db.SaveChanges();
                }
            }
            ViewBag.Act = act;
            return PartialView("_CreatePartial", data);
        }
        public ActionResult Edit(int id)
        {
            var c = db.Guests
                
                  .FirstOrDefault(x => x.GuestId == id);
            var g = new GuestEditModel
            {
                GuestId=c.GuestId,
                Guest_Name = c.Guest_Name,
                Contact_Number = c.Contact_Number,
                Address = c.Address,
                City = c.City,
                Reservations = c.Reservations.ToList()

            };
            ViewData["CurrentPic"] = db.Guests.First(x => x.GuestId == id).Picture;
            return View(g);

        }
        [HttpPost]
        public ActionResult Edit(GuestEditModel data, string act = "")
        {
            if (act == "add")
            {
                data.Reservations.Add(new Reservation { });
            }

            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Reservations.RemoveAt(index);
            }

            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var a = db.Guests.First(x => x.GuestId == data.GuestId);
                    a.Guest_Name = data.Guest_Name;
                    a.Contact_Number = data.Contact_Number;
                    a.Address = data.Address;
                    a.City = data.City;

                    if (data.Picture != null)
                    {
                        string ext = Path.GetExtension(data.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Server.MapPath("~/Pictures/") + fileName;
                        data.Picture.SaveAs(savePath);
                        a.Picture = fileName;
                    }
                    db.Reservations.RemoveRange(db.Reservations.Where(x => x.GuestId == data.GuestId).ToList());
                    foreach (var item in data.Reservations)
                    {
                        a.Reservations.Add(new Reservation
                        {
                            Reservation_Id = item.Reservation_Id,
                            Check_in_Date = item.Check_in_Date,
                            Check_out_Date = item.Check_out_Date,
                            Room_Type = item.Room_Type,
                            Room_Number = item.Room_Number,
                            IsCancelled = item.IsCancelled,

                        });
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewData["CurrentPic"] = db.Guests.First(x => x.GuestId == data.GuestId).Picture;
            return View(data);

        }
        public ActionResult Delete(int id)
        {
            var guest = new Guest { GuestId = id };
            db.Entry(guest).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new { success = true, deleted = id });
        }
    }
}
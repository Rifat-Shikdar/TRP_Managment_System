using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TRP_Managment_System.DTOs;
using TRP_Managment_System.EF;

namespace TRP_Managment_System.Controllers
{
    public class ProgramController : Controller
    {
        TRPManagmentSystemEntities db = new TRPManagmentSystemEntities();

        // GET: Program

        private static Program Convert(ProgramDTO p)
        {
            return new Program
            {
                ProgramId = p.ProgramId,
                ProgramName = p.ProgramName,
                TRPScore = p.TRPScore,
                ChannelId = p.ChannelId,
                AirTime = p.AirTime
            };
        }

        private static ProgramDTO Convert(Program p)
        {
            return new ProgramDTO
            {
                ProgramId = p.ProgramId,
                ProgramName = p.ProgramName,
                TRPScore = p.TRPScore,
                ChannelId = p.ChannelId,
                AirTime = p.AirTime,
                Channel = p.Channel
            };
        }

        private static List<ProgramDTO> Convert(List<Program> data)
        {
            var list = new List<ProgramDTO>();
            foreach (var p in data)
            {
                list.Add(Convert(p));
            }
            return list;
        }


        public ActionResult Index()
        {
            var programs = db.Programs.Include("Channel").ToList(); 
            return View(Convert(programs));
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var exobj = db.Programs.Include(p => p.Channel).FirstOrDefault(p => p.ProgramId == id);
            if (exobj == null)
            {
                return HttpNotFound();
            }
            return View(Convert(exobj));
        }

        // GET: Programs/Create
        public ActionResult Create()
        {
            // Get the list of channels to populate in a dropdown
            ViewBag.ChannelId = new SelectList(db.Channels, "ChannelId", "ChannelName");
            return View(new ProgramDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ProgramId,ProgramName,TRPScore,ChannelId,AirTime")] Program program)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Programs.Add(program);
        //        db.SaveChanges();
        //         TempData["Success"] = "Program added successfully.";
        //        return RedirectToAction("Index");
        //    }

        //    // If ModelState is invalid, repopulate the Channel dropdown
        //    ViewBag.ChannelId = new SelectList(db.Channels, "ChannelId", "ChannelName", program.ChannelId);
        //    return View(program);
        //}
        public ActionResult Create(ProgramDTO p)
        {
            if (ModelState.IsValid)
            {
                db.Programs.Add(Convert(p)); 
                db.SaveChanges();
                TempData["Success"] = "Program added successfully.";
                return RedirectToAction("Index");
            }

            ViewBag.ChannelId = new SelectList(db.Channels, "ChannelId", "ChannelName", p.ChannelId); // Repopulate dropdown
            return View(p); // Return the view with validation errors
        }


        // GET: Students/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var exobj = db.Programs.Find(id);
            if (exobj == null)
            {
                return HttpNotFound();
            }

            // Convert the Program entity to ProgramDTO
            var p = Convert(exobj);


            ViewBag.ChannelId = new SelectList(db.Channels, "ChannelId", "ChannelName", exobj.ChannelId);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ProgramId,ProgramName,TRPScore,ChannelId,AirTime")] Program program)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(program).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ChannelId = new SelectList(db.Channels, "ChannelId", "ChannelName", program.ChannelId);
        //    return View(program);

        //}
        public ActionResult Edit(ProgramDTO p)
        {
            if (ModelState.IsValid)
            {
                var exobj = db.Programs.Find(p.ProgramId);
                if (exobj == null) 
                    return HttpNotFound();

                // Convert DTO to Entity and update the program
                db.Entry(exobj).CurrentValues.SetValues(Convert(p));
                
                db.SaveChanges();
                TempData["Success"] = "Program updated successfully.";
                return RedirectToAction("Index");
            }

            ViewBag.ChannelId = new SelectList(db.Channels, "ChannelId", "ChannelName", p.ChannelId);
            return View(p); // Return with validation errors
        }

        // GET: Students/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var exobj = db.Programs.Include(p => p.Channel).FirstOrDefault(p => p.ProgramId == id);
            if (exobj == null)
            {
                return HttpNotFound();
            }
           

            return View(Convert(exobj));
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var exobj = db.Programs.Find(id);
            if (exobj == null)
            {
                TempData["Error"] = "Program not found.";
                return RedirectToAction("Index");
            }
            if (exobj.Channel != null && exobj.Channel.Programs.Count > 1)
            {
                TempData["Error"] = "Cannot delete the program because it has associated programs.";
                return RedirectToAction("Index");
            }
            db.Programs.Remove(exobj);
            db.SaveChanges();
            TempData["Success"] = "Program deleted successfully.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
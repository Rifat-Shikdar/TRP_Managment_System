using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRP_Managment_System.DTOs;
using TRP_Managment_System.EF;

namespace TRP_Managment_System.Controllers
{
    public class ChannelController : Controller
    {
        TRPManagmentSystemEntities db = new TRPManagmentSystemEntities();

        public static Channel Convert(ChannelDTO c)
        {
            return new Channel
            {
                ChannelId = c.ChannelId,
                ChannelName = c.ChannelName,
                EstablishedYear = c.EstablishedYear,
                Country = c.Country
            };
        }
        public static ChannelDTO Convert(Channel c)
        {
            return new ChannelDTO
            {
                ChannelId = c.ChannelId,
                ChannelName = c.ChannelName,
                EstablishedYear = c.EstablishedYear,
                Country = c.Country
            };
        }
        public static List<ChannelDTO> Convert(List<Channel> data)
        {
            var list = new List<ChannelDTO>();
            foreach (var c in data)
            {
                list.Add(Convert(c));
            }
            return list;
        }

        public ActionResult List() { 
        
            var data = db.Channels.ToList();
            return View(Convert(data));
        }

        //-----------------------------------------

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ChannelDTO());
        }
        [HttpPost]
        public ActionResult Create(ChannelDTO c)
        {
            //
            if (ModelState.IsValid)
            {

                db.Channels.Add(Convert(c));
                db.SaveChanges();
                TempData["Success"] = "Channel added successfully.";
                return RedirectToAction("List");
            }
            return View(c);

        }

        //-----------------------------------------------

        public ActionResult Details(int ChannelId)
        {
            var exobj = (from c in db.Channels.Include("Programs")
                         where c.ChannelId == ChannelId
                         select c).SingleOrDefault();
            //var data = exobj.Courses;
            var programs = exobj.Programs;
            return View(Convert(exobj));
        }

        //--------------------------------------------------

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var exobj = db.Channels.Find(Id);
            if (exobj == null) 
                return HttpNotFound();
            
            return View(Convert(exobj));
        }
        [HttpPost]
        public ActionResult Edit(ChannelDTO c)
        {
            var exobj = db.Channels.Find(c.ChannelId);
            if (exobj == null)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                db.Entry(exobj).CurrentValues.SetValues(Convert(c));
                db.SaveChanges();
                return RedirectToAction("List");

            }
               return View(c);
        }

        //--------------------------------------------------

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var exobj = db.Channels.Find(Id);
            if (exobj == null) 
                return HttpNotFound();

            return View(Convert(exobj));
        }
        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            var exobj = db.Channels.Find(Id);
            if (exobj == null) 
                return HttpNotFound();

            if (exobj.Programs.Count > 0)
            {
                TempData["Error"] = "Cannot delete a channel with associated programs.";
                return RedirectToAction("List");
            }


            if (dcsn.Equals("Yes"))
            {
               
                db.Channels.Remove(exobj);
                db.SaveChanges();
                TempData["Success"] = "Channel deleted successfully.";
            }
            return RedirectToAction("List");
        }

        // GET: Channel
        public ActionResult Index()
        {
            return View();
        }
    }
}
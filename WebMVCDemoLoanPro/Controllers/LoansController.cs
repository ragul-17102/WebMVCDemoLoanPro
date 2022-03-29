using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VehicleLoan.Models;

namespace VehicleLoan.Controllers
{
    public class LoansController : Controller
    {
        private AppDBContext db = new AppDBContext();

        // GET: Loans
        //[Route("mydata/index")]
        public ActionResult Index()
        {
            ViewBag.ID =Convert.ToInt32(Session["CustomerID"]);
            var loans = db.Loans.Include(l => l.Customers);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return View("Create");
                //return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create()
        {
            ViewBag.Name = Session["UserID"].ToString();
           
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Loan loan)
        {
            loan.CustomerID = Convert.ToInt32(Session["CustomerID"]);
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", loan.CustomerID);
            return View(loan);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult DashBoard()
        {
            if (Session["CustomerID"] != null)
            {
                ViewBag.Name = Session["UserID"].ToString();
                return View();
            }
            else
                return RedirectToAction("Login", "Customers");
        } 

    }
}

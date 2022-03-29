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
    public class AdminController : Controller
    {
        private AppDBContext db = new AppDBContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", loan.CustomerID);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanID,CustomerID,VehicleBrand,Model,OnRoadPrice,Status")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewLoans");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", loan.CustomerID);
            return View(loan);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
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


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            using (AppDBContext db = new AppDBContext())
            {
                var obj = db.Admins.Where(a => a.AdminName.Equals(admin.AdminName) && a.Password.Equals(admin.Password)).FirstOrDefault();



                if (obj != null)
                {
                   
                    Session["AdminName"] = obj.AdminName.ToString();
                    

                    return RedirectToAction("DashBoard", "Admin");
                }
                else
                {
                    ViewBag.Message = "user not found for given Email and Password";
                    return View();
                }
            }
        }
        public ActionResult DashBoard()
        {
            if (Session["AdminName"] != null)
            {
                ViewBag.Name = Session["AdminName"].ToString();
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        public ActionResult ViewLoans()
        {
            return View(db.Loans.ToList());
        }


        // GET: Loans/Delete/5
        public ActionResult DeleteLoan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLoanConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
            db.SaveChanges();
            return RedirectToAction("ViewLoans");
        }

        // GET: Loans/Details/5
        public ActionResult DetailsLoan(int? id)
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
    }
}

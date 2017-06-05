using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MainProject.Models;
using System.Collections.Generic;

namespace MainProject.Controllers
{
    [Authorize(Roles = "Parent,Child")]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            List<Account> accounts;
            //List<Transaction> transactions;
            int rcpId = 0;

            if (User.IsInRole("Admin"))
            {
                accounts = db.Accounts.ToList();
            }
            else if (User.IsInRole("Parent"))
            {
                accounts = db.Accounts
                                .Where(x => x.Owner == User.Identity.Name).ToList();
            }
            else
            {
                accounts = db.Accounts
                                .Where(x => x.Recipient == User.Identity.Name).ToList();
                foreach (Account a in accounts)
                {
                    rcpId = a.ID;
                }
            }
            //transactions = db.Transactions
            //                    .Where(x => x.Account.Owner == User.Identity.Name).ToList();
            //double balance = 0;
            //foreach (Transaction b in transactions)
            //{
            //    balance = balance + b.DisplayInterest();
            //}
            if (User.IsInRole("Child"))
            {
                Account childAccount = db.Accounts.Find(rcpId);
                string url = "Accounts/Details/" + rcpId;
                return Redirect(url);
            }
            else
            {
                return View(accounts);
            }
            //return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            string own = account.Owner;
            string rcp = account.Recipient;

            if (User.IsInRole("Child"))
            {
                if (!User.Identity.Name.Equals(rcp))
                {
                    return HttpNotFound();
                }
            }
            if (User.IsInRole("Parent"))
            {
                if (!User.Identity.Name.Equals(own))
                {
                    return HttpNotFound();
                }
            }

            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Owner,Recipient,Name,OpenDate,InterestRate")] Account account)
        {
            int instOwnRcpOfOther = db.Accounts
                                        .Where(p => p.Recipient == account.Owner)
                                        .Count();
            if (instOwnRcpOfOther > 0)
            {
                ModelState.AddModelError("Owner", "An owner cannot be a recipient of another account.");
            }


            int instRcpOwnOfOther = db.Accounts
                                        .Where(p => p.Owner == account.Recipient)
                                        .Count();
            if (instRcpOwnOfOther > 0)
            {
                ModelState.AddModelError("Recipient", "A recipient cannot be a owner of another account.");
            }


            int instRcpMultiple = db.Accounts
                                        .Where(p => p.Recipient == account.Recipient)
                                        .Count();
            if (instRcpMultiple > 0)
            {
                ModelState.AddModelError("Recipient", "A member cannot be recipient of multiple accounts.");
            }

            if (User.IsInRole("Parent"))
            {
                if (!account.Owner.Equals(User.Identity.Name))
                {
                    ModelState.AddModelError("Owner", "A owner cannot create account for some other owner.");
                }
            }
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Owner,Recipient,Name,OpenDate,InterestRate")] Account account)
        {
            int instOwnRcpOfOther = db.Accounts
                                        .Where(p => p.Recipient == account.Owner)
                                        .Where(p => p.ID != account.ID)
                                        .Count();
            if (instOwnRcpOfOther > 0)
            {
                ModelState.AddModelError("Owner", "An owner cannot be a recipient of another account.");
            }


            int instRcpOwnOfOther = db.Accounts
                                        .Where(p => p.Owner == account.Recipient)
                                        .Where(p => p.ID != account.ID)
                                        .Count();
            if (instRcpOwnOfOther > 0)
            {
                ModelState.AddModelError("Recipient", "A recipient cannot be a owner of another account.");
            }


            int instRcpMultiple = db.Accounts
                                        .Where(p => p.Recipient == account.Recipient)
                                        .Where(p => p.ID != account.ID)
                                        .Count();
            if (instRcpMultiple > 0)
            {
                ModelState.AddModelError("Recipient", "A member cannot be recipient of multiple accounts.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
    }
}

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MainProject.Models;

namespace MainProject.Controllers
{
    [Authorize(Roles = "Parent")]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Where(t => t.Account.Owner == User.Identity.Name);
            //var transactions = db.Transactions.Include(t => t.Account);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            string own = transaction.Account.Owner;

            if (!User.Identity.Name.Equals(own))
            {
                return HttpNotFound();
            }
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name), "ID", "Name");
            // ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner");

            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TransactionDate,Amount,Note,AccountId")] Transaction transaction)
        //public ActionResult Create([Bind(Include = "ID,TransactionDate,Amount,Note,Name")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //if (User.IsInRole("Admin"))
            //{
            //    ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", transaction.AccountId);
            //}
            //else 
            //{ 
            //    ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name).ToList(), "Id", "Owner", transaction.AccountId);
            //}

            //ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name).ToList(), "Id", "Owner", transaction.AccountId);
            ViewBag.AccountId = new SelectList(db.Accounts.ToList(), "Id", "Owner", transaction.AccountId);
            //ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", transaction.AccountId);

            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name), "ID", "Name", transaction.AccountId);
            //ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", transaction.AccountId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TransactionDate,Amount,Note,AccountId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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

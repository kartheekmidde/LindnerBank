using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MainProject.Models;
using System.Collections.Generic;

namespace MainProject.Controllers
{
    [Authorize(Roles ="Parent,Child")]
    public class WishListItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishListItems
        public ActionResult Index()
        {
            //var wishListItems= db.WishListItems;
            List<WishListItem> wishListItems;

            if (User.IsInRole("Parent")) { 
            wishListItems = db.WishListItems
                                        .Where(x => x.Account.Owner == User.Identity.Name)
                                        .Include(w => w.Account)
                                        .ToList();
            } else
            {
             wishListItems = db.WishListItems
                                        .Where(x => x.Account.Recipient == User.Identity.Name)
                                        .Include(w => w.Account)
                                        .ToList();
            }
            return View(wishListItems);
        }

        // GET: WishListItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListItem wishListItem = db.WishListItems.Find(id);
            if (wishListItem == null)
            {
                return HttpNotFound();
            }
            return View(wishListItem);
        }

        // GET: WishListItems/Create
        public ActionResult Create()
        {
            //ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name), "ID", "Owner");
            if (User.IsInRole("Parent"))
            {
                ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name), "ID", "Name");
            }
            else
            {
                ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Recipient == User.Identity.Name), "ID", "Name");
            }
            //ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name), "ID", "Owner");
            //ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner");
            return View();
        }

        // POST: WishListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateAdded,Cost,Description,Link,Purchased,AccountId")] WishListItem wishListItem)
        {
            if (ModelState.IsValid)
            {
                db.WishListItems.Add(wishListItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", wishListItem.AccountId);
            return View(wishListItem);
        }

        // GET: WishListItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListItem wishListItem = db.WishListItems.Find(id);
            if (wishListItem == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Parent"))
            {
                ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Owner == User.Identity.Name), "ID", "Name", wishListItem.AccountId);
            }
            else
            {
                ViewBag.AccountId = new SelectList(db.Accounts.Where(x => x.Recipient == User.Identity.Name), "ID", "Name", wishListItem.AccountId);
            }
            //ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", wishListItem.AccountId);
            return View(wishListItem);
        }

        // POST: WishListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateAdded,Cost,Description,Link,Purchased,AccountId")] WishListItem wishListItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "ID", "Owner", wishListItem.AccountId);
            return View(wishListItem);
        }

        // GET: WishListItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListItem wishListItem = db.WishListItems.Find(id);
            if (wishListItem == null)
            {
                return HttpNotFound();
            }
            string own = wishListItem.Account.Owner;
            string rcp = wishListItem.Account.Recipient;
            if (User.IsInRole("Parent"))
            {
                if (!User.Identity.Name.Equals(own))
                {
                    return HttpNotFound();
                }
            }
            if (User.IsInRole("Child"))
            {
                if (!User.Identity.Name.Equals(rcp))
                {
                    return HttpNotFound();
                }
            }
            return View(wishListItem);
        }

        // POST: WishListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishListItem wishListItem = db.WishListItems.Find(id);
            db.WishListItems.Remove(wishListItem);
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

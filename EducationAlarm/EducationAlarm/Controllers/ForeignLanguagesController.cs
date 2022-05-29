using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EducationAlarmDb;

namespace EducationAlarm.Controllers
{
    public class ForeignLanguagesController : Controller
    {
        private AlarmContext db = new AlarmContext();

        // GET: ForeignLanguages
        public ActionResult Index()
        {
            return View(db.ForeignLanguages.ToList());
        }
        public ActionResult WordIndex(int id)
        {
            var words = db.ForeignWords.Where(x => x.ForeignLanguageId == id);
            return View(words);
        }

        // GET: ForeignLanguages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForeignLanguage foreignLanguage = db.ForeignLanguages.Find(id);
            if (foreignLanguage == null)
            {
                return HttpNotFound();
            }
            return View(foreignLanguage);
        }

        // GET: ForeignLanguages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForeignLanguages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Language")] ForeignLanguage foreignLanguage)
        {
            if (ModelState.IsValid)
            {
                db.ForeignLanguages.Add(foreignLanguage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foreignLanguage);
        }

        [HttpGet]
        public ActionResult AddWords()
        {
            var languages = db.ForeignLanguages.ToList();
            ViewBag.ForeignLanguageId = new SelectList(languages, "ForeignLanguageId", "ForeignLanguage.Language");
            return View();
        }
        public ActionResult AddWords(ForeignWord word)
        {
            db.ForeignWords.Add(new ForeignWord { ForeignLanguageId = word.ForeignLanguageId, Word = word.Word, Definintion = word.Definintion });
            db.SaveChanges();
            return RedirectToAction("WordIndex",new { id=word.ForeignLanguageId});
        }

        // GET: ForeignLanguages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForeignLanguage foreignLanguage = db.ForeignLanguages.Find(id);
            if (foreignLanguage == null)
            {
                return HttpNotFound();
            }
            return View(foreignLanguage);
        }

        // POST: ForeignLanguages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ForeignLanguageId,Language")] ForeignLanguage foreignLanguage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foreignLanguage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(foreignLanguage);
        }

        // GET: ForeignLanguages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForeignLanguage foreignLanguage = db.ForeignLanguages.Find(id);
            if (foreignLanguage == null)
            {
                return HttpNotFound();
            }
            return View(foreignLanguage);
        }

        // POST: ForeignLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ForeignLanguage foreignLanguage = db.ForeignLanguages.Find(id);
            db.ForeignLanguages.Remove(foreignLanguage);
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

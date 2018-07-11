using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTask.Models;
using System.IO;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {

        public string keyWord;
        public string TextBody;
        static List<Sentence> Sentences;
        public static List<Sentence> OldSentences; 

        public HomeController()
        {
            keyWord = String.Empty;
            TextBody = String.Empty;
        }
        public ActionResult Index()
        {
            using(var context = new SentenceContext())
            {
                context.Database.CreateIfNotExists();
                OldSentences = context.Sentences.ToList();
            }
            ViewBag.OldSentences = OldSentences.OrderBy(x=>x.DateTime).Reverse();

            return View();
        }

        [HttpPost]
        public void Import(string reportName, HttpPostedFileBase file)
        {
            Console.WriteLine(file);
            var dd = file.ToString();

            BinaryReader b = new BinaryReader(file.InputStream);
            byte[] binData = b.ReadBytes(file.ContentLength);
            TextBody = System.Text.Encoding.UTF8.GetString(binData);
            keyWord = reportName;
            CreateSentences();
        }

        public void Save()
        {
            using(var context = new SentenceContext())
            {
                context.Sentences.AddRange(Sentences);
                context.SaveChanges();
            }
        }
        
        public void CreateSentences()
        {
            var allSentences = TextBody.Split('!', '.', '?');

            Sentences = new List<Sentence>();
            foreach (var s in allSentences)
            {
                string[] words = s.Split(' ');
                if (words.Contains(keyWord))
                {

                    int count = words.Where(x => x == keyWord).Count();
                    Sentences.Add(new Sentence()
                    {
                        Text = string.Join(" ", words.Select(x => new string(x.Reverse().ToArray()))),
                        Count = count,
                        DateTime = DateTime.Now
                    });
                }
            }
            ViewBag.NewSentences = Sentences;
            Save();

        }
        public JsonResult GetData()
        {
            return Json(Sentences.Select(x=>x.Text), JsonRequestBehavior.AllowGet);
        }
    }
}
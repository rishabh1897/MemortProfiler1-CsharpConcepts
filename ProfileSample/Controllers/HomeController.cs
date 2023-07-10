using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new ProfileSampleEntities())
            {
                var sources = context.ImgSources.Take(20).Select(x => x.Id).ToList();
                var model = sources.Select(id =>
                {
                    var item = context.ImgSources.Find(id);
                    return new ImageModel()
                    {
                        Name = item.Name,
                        Data = item.Data
                    };
                }).ToList();
                return View(model);
            }
        }

        public ActionResult Convert()
        {
            var files = GetImgFiles();
            using (var context = new ProfileSampleEntities())
            {
                files.ToList().ForEach(file =>
                {
                    var buff = ReadFilesBytes(file);
                    saveImgToDb(context, file, buff);
                });
            }

            return RedirectToAction("Index");
        }
        private string[] GetImgFiles()
        {
            var path = Server.MapPath("~/Content/Img");
            return Directory.GetFiles(path, "*.jpg");
        }
        private byte[] ReadFilesBytes(string file)
        {

            return System.IO.File.ReadAllBytes(file);
        }
        private void saveImgToDb(ProfileSampleEntities context, string file, byte[] buff)
        {
            var filename = Path.GetFileName(file);
            var entity = new ImgSource()
            {
                Name = filename,
                Data = buff
            };
            context.ImgSources.Add(entity);
            context.SaveChanges();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
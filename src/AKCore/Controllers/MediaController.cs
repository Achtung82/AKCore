﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AKCore.DataModel;
using AKCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AKCore.Controllers
{
    [Route("Media")]
    [Authorize(Roles = "SuperNintendo,Editor")]
    public class MediaController : Controller
    {
        private static readonly string[] ImageExtensions = { "jpg", "bmp", "gif", "png","svg" };
        //private static readonly string[] VideoExtensions = { "mp4", "avi"};
        private static readonly string[] DocumentExtensions = { "pdf", "docx", "doc" };
        private readonly IHostingEnvironment _hostingEnv;

        public MediaController(IHostingEnvironment env)
        {
            _hostingEnv = env;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Filuppladdning";

            //var medias = PopulateList(1, out int totalPages, "", "Image");
            var model = new MediaModel
            {
                MediaFiles = new List<Media>(),
                Folders = GetFacets(),
                TotalPages = 1,
                CurrentPage = 1
            };

            return View(model);
        }

        [Route("MediaList")]
        public ActionResult MediaList(SearchModel search)
        {
            var type = "";
            if (search.Tag=="Document" || search.Tag == "Image") {
                type = search.Tag;
                search.Tag = "";
            }

            var model = new MediaModel();
            if(string.IsNullOrWhiteSpace(search.Tag) && string.IsNullOrWhiteSpace(search.SearchPhrase)) {
                model.Folders = GetFacets();
            }
            else
            {
                var medias = PopulateList(search.Page < 1 ? 1 : search.Page, out int totalPages, search.SearchPhrase ?? "", type, search.Tag);
                if (totalPages < search.Page) search.Page = totalPages;
                model.MediaFiles = medias;
                model.TotalPages = totalPages;
                model.CurrentPage = search.Page < 1 ? 1 : search.Page;
            }
            

            return PartialView("Partials/MediaList",model);
        }

        [Route("MediaPickerList")]
        public ActionResult MediaPickerList(SearchModel search)
        {
            var medias = PopulateList(search.Page < 1 ? 1 : search.Page, out int totalPages, search.SearchPhrase ?? "", search.Type, search.Tag);
            if (totalPages < search.Page) search.Page = totalPages;
            var model = new MediaModel
            {
                MediaFiles = medias,
                TotalPages = totalPages,
                CurrentPage = search.Page < 1 ? 1 : search.Page
            };
            return PartialView("_MediaPickerList", model);
        }

        private IDictionary<string, int> GetFacets()
        {
            using (var db = new AKContext(_hostingEnv))
            {
                var facetes = db.Medias
                    .GroupBy(x => x.Tag)
                    .Select(x => new
                    {
                        Tag = x.Key,
                        Count = x.Count()
                    });

                return facetes.ToDictionary(z=>z.Tag,z=>z.Count);
            }
        }

        private List<Media> PopulateList(int page, out int totalPages,string searchPhrase, string type = "", string tag = "")
        {
            var pagesize = 12;
            using (var db = new AKContext(_hostingEnv))
            {
                var searched=db.Medias
                    .Where(x=>x.Name.Contains(searchPhrase))
                    .Where(x => string.IsNullOrWhiteSpace(type) || x.Type == type)
                    .Where(x => string.IsNullOrWhiteSpace(tag) || x.Tag == tag)
                    .OrderByDescending(x=>x.Created);
                totalPages = ((searched.Count()-1) / pagesize) +1;
                if (totalPages < page) page = totalPages;
                return searched.Skip((page - 1)* pagesize).Take(pagesize).ToList();
            }
        }
        [Route("UploadFile")]
        public ActionResult UploadFile(MediaModel model)
        {
            var file = model.UploadFile;
            var ext=Path.GetExtension(file.FileName).ToLower();
            var isImage = ImageExtensions.FirstOrDefault(x => ext.EndsWith(x)) != null;
            var isDocument = DocumentExtensions.FirstOrDefault(x => ext.EndsWith(x)) != null;
            if (!(isImage || isDocument) || string.IsNullOrWhiteSpace(model.Tag))
            {
                return Json(new { success = false, message = "Filen har fel format" });
            }

            var filename = ContentDispositionHeaderValue
                .Parse(file.ContentDisposition)
                .FileName
                .Trim('"');
            var filepath = _hostingEnv.WebRootPath + $@"\media\{filename}";


            using (var db = new AKContext(_hostingEnv))
            {
                if (db.Medias.FirstOrDefault(x => x.Name == filename) != null)
                {
                    return Json(new {success = false, message = "Filen finns redan uppladdad"});
                }
                using (var fs = System.IO.File.Create(filepath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var mediaFile = new Media
                {
                    Name = filename,
                    Created = DateTime.Now,
                    Tag = model.Tag
                };
                mediaFile.Type = isImage ? "Image" : "Document";
                db.Medias.Add(mediaFile);
                db.SaveChanges();
            }

            return Json(new {success = true});
        }
        [Route("RemoveFile")]
        public ActionResult RemoveFile(string filename)
        {
            var filepath = _hostingEnv.WebRootPath + $@"\media\{filename}";

            using (var db = new AKContext(_hostingEnv))
            {
                var file = db.Medias.FirstOrDefault(x => x.Name == filename);
                if (file == null)
                {
                    return Json(new { success = false, message = "Filen finns ej" });
                }
                try
                {
                    System.IO.File.Delete(filepath);
                }
                catch
                {
                    return Json(new { success = false, message = "Misslyckades ta bort filen" });
                }
                db.Medias.Remove(file);
                db.SaveChanges();
            }

            return Json(new { success = true });
        }
    }
}
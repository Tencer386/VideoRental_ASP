using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using VR_FE_API.Models;

namespace VR_FE_API.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync("Movies").Result;

            IEnumerable<Movie> movies = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;
            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Movies/{id}").Result;
            var movie = response.Content.ReadAsAsync<Movie>().Result;

            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PostAsJsonAsync("Movies", movie).Result;

                //we will refer to this in the Index.cshtml of the Movie so alertify can display the message
                TempData["SuccessMessage"] = "Record added successfully";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Movies/{id}").Result;
            var movie = response.Content.ReadAsAsync<Movie>().Result;

            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Movie movie)
        {
            try
            {
                HttpResponseMessage response = WebClient.ApiClient.PutAsJsonAsync($"Movies/{id}", movie).Result;

                TempData["SuccessMessage"] = "Record updated successfully";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = WebClient.ApiClient.GetAsync($"Movies/{id}").Result;
            var movie = response.Content.ReadAsAsync<Movie>().Result;

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                HttpResponseMessage response = WebClient.ApiClient.DeleteAsync($"Movies/{id}").Result;

                TempData["SuccessMessage"] = "Record deleted successfully";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach(var file in files)
            {
                file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), file.FileName));
            }

            return Json("File uploaded Successfully.");
        }
    }
}

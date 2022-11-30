
using Newtonsoft.Json;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class PReviewController : Controller
    {
        // GET: Review
        [HttpGet]
        public ActionResult GetToDoList()
        {
            List<ToDo> TD = new List<ToDo>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var responseTask = client.GetAsync("ToDo/GetTaskList");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get back student object
                    var readTask = response.Content.ReadAsAsync<List<ToDo>>();
                    readTask.Wait();
                    TD = readTask.Result;


                }
            }
            return View(TD);
        }
        public ActionResult GetReviewList()
        {
            List<TaskReview> TR = new List<TaskReview>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var responseTask = client.GetAsync("Review/GetTaskReviewList");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get back student object
                    var readTask = response.Content.ReadAsAsync<List<TaskReview>>();
                    readTask.Wait();
                    TR = readTask.Result;


                }
            }
            return View(TR);
        }



        [HttpGet]
        public ActionResult GetTaskReviewbytheirID(int ToDoId)
        {
            List<TaskReview> TR = new List<TaskReview>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET - Get a student by the Student Id
                var responseTask = client.GetAsync("Review/GetTaskReviewbytheirID?ToDoId=" + ToDoId.ToString());
                // HTTP Get with id and name parameters
                //var responseTask = client.GetAsync(String.Format("getstudentByname/?id={0}&name={1}", StdId, StdName));
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get back a single student object
                    var readTask = response.Content.ReadAsAsync<List<TaskReview>>();
                    readTask.Wait();
                    TR = readTask.Result;
                }
            }
            return View(TR);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( TaskReview Tr)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44328/api/Review");

                    var response = await client.PostAsJsonAsync("Review/PostReview", Tr);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetToDoList");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(Tr);
        }
        public async Task<ActionResult> Edit(int ReviewID)
        {
            if (ReviewID == 0)
            {
                return View();
            }
            TaskReview Tr = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/Review/");

                var result = await client.GetAsync("GetReviewById?ReviewID=" + ReviewID.ToString());

                if (result.IsSuccessStatusCode)
                {
                    Tr = await result.Content.ReadAsAsync<TaskReview>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (Tr == null)
            {
                return View();
            }
            return View(Tr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TaskReview Tr)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44328/api/Review/");
                    var response = await client.PutAsJsonAsync("Put", Tr);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetReviewList");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("GetReviewList");
            }
            return View(Tr);
        }

        public async Task<ActionResult> Delete(string ReviewID)
        {
            if (ReviewID == null)
            {
                return View();
            }
            TaskReview Tr = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/Review/");

                var result = await client.GetAsync("GetReviewById?ReviewID=" + ReviewID);

                if (result.IsSuccessStatusCode)
                {
                    Tr = await result.Content.ReadAsAsync<TaskReview>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (Tr == null)
            {
                return View();
            }
            return View(Tr);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string ReviewID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/Review/");

                var response = await client.DeleteAsync("DeleteReview/?ReviewID="+ ReviewID);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetReviewList");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }

        //public ActionResult Create()
        //{
        //    //// Call the GetGradeList method of the API
        //    //List<int> gl = GetGradeList();
        //    //// Make a SelectList of the Gradelist and store it in a ViewBag
        //    //ViewBag.grdList = new SelectList(gl);

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(TaskReview TR)
        //{

        //    String res = "";
        //    if (ModelState.IsValid)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("https://localhost:44378/api/Review/");
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));

        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            //HTTP POST
        //            var responseTask = client.PostAsJsonAsync(("PostReview"), TR);

        //            responseTask.Wait();

        //            HttpResponseMessage response = responseTask.Result;
        //            if (response.IsSuccessStatusCode)
        //            {

        //                var readTask = response.Content.ReadAsStringAsync();
        //                readTask.Wait();
        //                res = readTask.Result;
        //            }


        //        }


        //        //If successfully Inserted the Student record
        //        if (Convert.ToInt32(res) > 0)

        //            return RedirectToAction("GetReviewList");
        //        else
        //            // Something went wrong in the adding the record.
        //            // Check the exception Log.
        //            // Post back the student to the Edit View itself.
        //            return View(TR);
        //        //return RedirectToAction("GetReviewList");
        //    }

        //    else
        //        //return RedirectToAction("GetReviewList");
        //        return View(TR);
        //    //return RedirectToAction("GetTaskReviewbytheirID");
        //}



        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Movie movie = db.Movies.Find(id);
        //    if (movie == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(movie);
        //}

        //public ActionResult EditReview(int ReviewID)
        //{
        //    TaskReview T = new TaskReview();
        //    // Get the Student object and pass it to the view, for Confirmation
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:44378/api/Review/");
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //HTTP GET - Get a student by the Student Id
        //        var responseTask = client.GetAsync("GetReviewById?ReviewId=" + ReviewID.ToString());

        //        //responseTask.Wait();

        //        HttpResponseMessage response = responseTask.Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Get back a single student object
        //            var readTask = response.Content.ReadAsAsync<TaskReview>();
        //            readTask.Wait();
        //            T = readTask.Result;
        //        }
        //        return View(T);
        //        //HttpResponseMessage response = GlobalVariable.webApiClient.GetAsync("GetReviewById?ReviewID=" + ReviewID.ToString()).Result;
        //        //return View(response.Content.ReadAsAsync<TaskReview>().Result);
        //    }
        //}

        //[HttpPost]
        //// Post back from the Edit Student View
        //public ActionResult EditReview(TaskReview T)
        //{
        //    String res = "";
        //    if (ModelState.IsValid)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("http://localhost:44378/api/Review/");
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));

        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            //HTTP PUT, pass the Student Id and the Student Object
        //            var responseTask = client.PutAsJsonAsync(String.Format("Put/" + T.ReviewId), T);
        //            responseTask.Wait();

        //            HttpResponseMessage response = responseTask.Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var readTask = response.Content.ReadAsStringAsync();
        //                readTask.Wait();
        //                res = readTask.Result;
        //            }

        //        }
        //        // If successfully Updated the Student record
        //        if (Convert.ToInt32(res) > 0)
        //            return RedirectToAction("GetTaskReviewbytheirID");
        //        else
        //            // Something went wrong in the adding the record.
        //            // Check the exception Log.
        //            // Post back the student to the Edit View itself.
        //            return View(T);
        //    }
        //    else
        //        return View(T);
        //}

        //public ActionResult DeleteReview(int ReviewID)
        //{
        //    TaskReview T = new TaskReview();
        //    // Get the Student object and pass it to the view, for Confirmation
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://localhost:44378/api/Review/");
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //HTTP GET - Get a student by the Student Id
        //        var responseTask = client.GetAsync("GetReviewById?ReviewId=" + ReviewID.ToString());
        //        // HTTP Get with id and name parameters
        //        //var responseTask = client.GetAsync(String.Format("getstudentByname/?id={0}&name={1}", StdId, StdName));
        //        responseTask.Wait();

        //        HttpResponseMessage response = responseTask.Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Get back a single student object
        //            var readTask = response.Content.ReadAsAsync<TaskReview>();
        //            readTask.Wait();
        //            T = readTask.Result;
        //        }
        //        return View(T);
        //    }
        //}

        //// Post back from the DeleteConfirmation View
        //[HttpPost, ActionName("DeleteReview")]
        //public ActionResult DeleteStudentConfirmed(int ReviewID)
        //{
        //    String res = "";
        //    if (ModelState.IsValid)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("https://localhost:44378/api/Review/");
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));

        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            //HTTP PUT, pass the Student Id and the Student Object
        //            var responseTask = client.DeleteAsync(String.Format("DeleteReview/?id={0}", ReviewID));
        //            responseTask.Wait();

        //            HttpResponseMessage response = responseTask.Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var readTask = response.Content.ReadAsStringAsync();
        //                readTask.Wait();
        //                res = readTask.Result;
        //            }

        //        }
        //        // If successfully Deleted the Student record
        //        if (Convert.ToInt32(res) > 0)
        //            return RedirectToAction("GetStudents");
        //        else
        //            // Something went wrong in the adding the record.
        //            // Check the exception Log.
        //            // Post back the student to the Delete View itself.
        //            return View();
        //    }
        //    else
        //        return View();
        //}



        //public ActionResult Edit(int id)
        //{
        //    TaskReview T = null;

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:64189/api/Review");
        //        //HTTP GET
        //        var responseTask = client.GetAsync("Review/?id=" + id.ToString());
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<TaskReview>();
        //            readTask.Wait();

        //            T = readTask.Result;
        //        }
        //    }

        //    return View(T);
        //}

        //public ActionResult Delete(int id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:64189/api/Review");

        //        //HTTP DELETE
        //        var deleteTask = client.DeleteAsync("DeleteReview/" + id.ToString());
        //        deleteTask.Wait();

        //        var result = deleteTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {

        //            return RedirectToAction("Index");
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}


   

    
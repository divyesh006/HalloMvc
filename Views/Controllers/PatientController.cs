using HalloDoc.Entities.Patient;
using HalloDoc_mvc.Entities.DataModels;
using HalloDoc_mvc.Entities.Patient;
using HalloDoc_mvc.Models;
using HalloDoc_mvc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
/*using AspNetCore; */

namespace HalloDoc_mvc.Controllers
{
    public class PatientController : Controller

    {
        private readonly HalloDocContext _context;
        public IPatient _patient;
        private IHostEnvironment _hostEnvironment;
        private readonly IEmailSender _emailSender;

        public PatientController(HalloDocContext context, IPatient patient, IHostEnvironment hostEnvironment, IEmailSender emailSender)
        {
            _context = context;
            _patient = patient;
            _hostEnvironment = hostEnvironment;
            _emailSender = emailSender;
        }

        [Route("patientlogin")]
        public IActionResult PatientLogin()
        {
            return View();
        }

        [Route("patientlogin")]
        [HttpPost]
        public IActionResult PatientLogin(AspNetUser model)
        {
            var user = _patient.GetByEmailAndPassword(model.Email, model.PasswordHash);
            if (user != null)
            {
                var userCheck = _patient.GetByUserEmail(model.Email);
                HttpContext.Session.SetString("userID", userCheck.UserId.ToString());
                return RedirectToAction("PatientDashboard", "Patient");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Invalid email or Password");
                return View("PatientLogin",model);
            }
        }

        [Route("/Patient/PatientForm/CheckEmail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _patient.DoesEmailExists(email);
            return Json(new { exists = emailExists });
        }

        [Route("/patientprofile/UserCheckEmail/{email}")]
        [HttpGet]
        public IActionResult UserCheckEmail(string email)
        {
            var userId = HttpContext.Session.GetString("userID");
            var userCheck = _patient.DoesEmailExistsForOtherUser(email, userId);
            return Json(new { exists = userCheck });
        }

        public IActionResult PatientForgot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientForgot(AspNetUser model)
        {
            var user = _patient.GetAspByEmail(model.Email);
            if(user == null)
            {
                ModelState.AddModelError(String.Empty, "Email Is Not Exists");
                return View();
            }
            var resetToken = GenerateToken();
            var resetLink = Url.Action("ResetPassword", "Patient", new { email = model.Email, token = resetToken }, Request.Scheme);
            SendPasswordResetEmail(model.Email, resetLink);
            return RedirectToAction("patientlogin");

        }

        private void SendPasswordResetEmail(string email, string resetLink)
        {
            var subject = "Password Reset Request";
            var message = $"Please click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

            _emailSender.SendEmailAsync(email, subject, message);
        }

        private string GenerateToken()
        {
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            string token = Convert.ToBase64String(tokenBytes);
            return token;
        }

        public IActionResult PatientSubmit()
        {
            return View();
        }

        public IActionResult PatientForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientForm(PatientForm model)
        {
           
            
                var data = _patient.PatientForm(model);
                if (data == "PasswordNotMatch")
                {
                    ModelState.AddModelError(String.Empty, "Password Not Match");
                    return View();
                }
                else if (data == "RegionNotFound")
                {
                    ModelState.AddModelError(String.Empty, "We are not available in Your City");
                    return View();
                }
                else if (data == "Yes")
                {
                    return RedirectToAction("Index", "Home");
                }
            
            
            return View();
        }

        public IActionResult FamilyForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FamilyForm(FamilyForm model)
        {
            var form = _patient.FamilyForm(model);

            if (form == "RegionNotFound")
            {
                ModelState.AddModelError(String.Empty, "We are not available in Your City");
                return View();
            }
            else if (form == "Yes")
            {
                var user = _patient.GetAspByEmail(model.PatientEmail);
                if (user == null)
                {
                    var subject = "Create Account";
                    var message = "Create Account : https://localhost:44367/Patient/CreateAccount";

                    _emailSender.SendEmailAsync(model.PatientEmail, subject, message);
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("viewdocument/{id}")]
        public IActionResult ViewDocument(string id)
        {

            var userId = HttpContext.Session.GetString("userID");
            if (userId == null)
            {
                return RedirectToAction("patientlogin");

            }
            ViewBag.requestId = id;
            ViewBag.file = _patient.GetFilesByRequestId(id);
            var user = _patient.GetByUserId(userId);
            ViewBag.username = user.FirstName + " " + user.LastName;
           
            return View();
        }

        [HttpPost]
        public IActionResult FileUpload(FileUpload model)
        {
            if (model.file != null && model.file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", model.file.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.file.CopyTo(stream);

                    RequestWiseFile file = new RequestWiseFile()
                    {
                        RequestId = model.RequestId,
                        FileName = model.file.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    _patient.AddRequestWiseFile(file);
                }
            }

            return RedirectToAction("viewDocument", new { id = model.RequestId });
        }

        public IActionResult Download(int id)
        {
            var filename = _patient.GetFilesById(id);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename.FileName);
            return File(System.IO.File.ReadAllBytes(filePath), "multipart/form-data", System.IO.Path.GetFileName(filePath));

        }

        public IActionResult ConciergeForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConciergeForm(ConciergeForm model)
        {
            var Form = _patient.ConciergeForm(model);

            if (Form == "RegionNotFound")
            {
                ModelState.AddModelError(String.Empty, "We are not available in Your City");
                return View();
            }
            else if (Form == "Yes")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

        public IActionResult BusinessForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BusinessForm(BusinessForm model)
        {
            var form = _patient.BusinessForm(model);
            if (form == "RegionNotFound")
            {
                ModelState.AddModelError(String.Empty, "We are not available in Your City");
                return View();
            }
            else if (form == "Yes")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

        [Route("dashboard")]
        public IActionResult PatientDashboard()
        {
            var userID = HttpContext.Session.GetString("userID");
            if (userID == null)
            {
                return RedirectToAction("patientlogin");

            }
            List<Request> userData = _patient.GetRequestByUserID(userID);
            var user = _patient.GetByUserId(userID);
            ViewBag.RequestFile = _patient.GetAllRequestWiseFile();
            ViewBag.username = user.FirstName + " " + user.LastName;
            Dictionary<int, int> requestIdCounts = new Dictionary<int, int>();
            foreach (var request in userData)
            {
                int count = _patient.GetRequestWiseFileCount(request.RequestId);
                requestIdCounts.Add(request.RequestId, count);
            }
            ViewBag.RequestIdCounts = requestIdCounts;
            return View(userData);
        }

        [Route("dashboard/patient-request")]
        public IActionResult DashboardMeRequest()
        {
            var userId = HttpContext.Session.GetString("userID");
            if (userId == null)
            {
                return RedirectToAction("patientlogin");

            }
            var user = _patient.GetByUserId(userId);
            ViewBag.username = user.FirstName + " " + user.LastName;
            var users = new DashboardRequest();
            users.user = user;
            int month = DateTime.ParseExact(user.StrMonth, "MMM", CultureInfo.InvariantCulture).Month;
            String strMonth = month.ToString("D2");
            ViewBag.DateOfBirth = user.IntYear + "-" + strMonth + "-" + user.IntDate;
            return View(users);
        }

        [Route("dashboard/patient-request")]
        [HttpPost]
        public IActionResult DashboardMeRequest(DashboardRequest model)
        {
            var userId = HttpContext.Session.GetString("userID");
            var data = _patient.ForMeRequest(model,userId);
            if (data == "RegionNotFound")
            {
                ModelState.AddModelError(String.Empty, "We are not available in Your City");
                return View();
            }
            else if (data == "Yes")
            {
                return RedirectToAction("PatientDashboard","Patient");
            }
            return View();
            
        }

        [Route("dashboard/family-friend-request")]
        public IActionResult DashboardSomeoneRequest()
        {
            var userId = HttpContext.Session.GetString("userID");
            if (userId == null)
            {
                return RedirectToAction("patientlogin");

            }
            var user = _patient.GetByUserId(userId);
            ViewBag.username = user.FirstName + " " + user.LastName;
            return View();
        }

        [Route("dashboard/family-friend-request")]
        [HttpPost]
        public IActionResult DashboardSomeoneRequest(DashboardRequest model)
        {
            var userId = HttpContext.Session.GetString("userID");
            var data = _patient.ForMeRequest(model, userId);
            if (data == "RegionNotFound")
            {
                ModelState.AddModelError(String.Empty, "We are not available in Your City");
                return View();
            }
            else if (data == "Yes")
            {
                return RedirectToAction("PatientDashboard", "Patient");
            }
            return View();

        }

        [Route("patientprofile")]
        public IActionResult PatientProfile()
        {
            var userId = HttpContext.Session.GetString("userID");
            if (userId == null)
            {
                return RedirectToAction("patientlogin");
            }
            var user = _patient.GetByUserId(userId);
            var profile = new UpdateProfile();
            profile.user = user;
            ViewBag.username = user.FirstName + " " + user.LastName;
            int month = DateTime.ParseExact(user.StrMonth, "MMM", CultureInfo.InvariantCulture).Month;
            int date = (int)user.IntDate;
            String strDate = date.ToString("D2");
            String strMonth = month.ToString("D2");
            ViewBag.DateofBirth = user.IntYear + "-" + strMonth + "-" + strDate;
            return View(profile);
        }

        public IActionResult Update(UpdateProfile model)
        {
            
            var userId = HttpContext.Session.GetString("userID");

            var user = _patient.GetByUserId(userId);

            var aspUser = _patient.GetAspByEmail(model.Email);           

            var data = _patient.UpdateUser(model, user, aspUser);

            if (data == "RegionNotFound")
            {
                ModelState.AddModelError(String.Empty, "We are not available in Your City");
                return RedirectToAction("patientprofile");
            }
            else if (data == "yes")
            {
                return RedirectToAction("patientprofile");
            }
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult PatientLogout()
        {
            HttpContext.Session.Remove("userID");
            return View("patientlogin");
        }


        [HttpPost]
        public IActionResult CreateAccount(CreateAccount model)
        {
            var data = _patient.CreateAcconut(model);
            if (data == "EmailExists")
            {
                ModelState.AddModelError(String.Empty, "Email Already Exists");
                return View();
            }
            else if(data == "PasswordNotMatch")
            {
                ModelState.AddModelError(String.Empty, "Password and Confirm Password Does Not Match");
                return View();
            }
            else if (data == "Yes") 
            {
                return RedirectToAction("patientlogin");
            }
            return View();
        }

        [Route("resetpassword/{email}")]
        public IActionResult ResetPassword(string email,ResetPassword model)
        {
            var user = _patient.GetAspByEmail(email);
            var password = user.PasswordHash;
            var users = new ResetPassword();
            users.Email = email;
            if (model.PasswordHash == null)
            {
                ModelState.AddModelError(String.Empty, "Please Enter a Password");
                return View();
            }
            if (password == model.PasswordHash)
            {
                ModelState.AddModelError(String.Empty, "Please Enter a Different Password");
                return View();
            }
            if (model.PasswordHash == model.ConfirmPasswordHash) 
            {
                _patient.ResetPassword(user, model.PasswordHash);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Password And ConfirmPassword Not Match");
                return View();
            }
            
            return RedirectToAction("patientlogin");
        }

        [Route("reviewagreement")]
        public IActionResult ReviewAgreement()
        {
            return View();
        }
    }

}


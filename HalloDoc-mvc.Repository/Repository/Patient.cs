using HalloDoc.Entities.Patient;
using HalloDoc_mvc.Entities.DataModels;
using HalloDoc_mvc.Entities.Patient;
using HalloDoc_mvc.Models;
using HalloDoc_mvc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_mvc.Repository.Repository
{
    public class Patient : IPatient
    {
        private readonly HalloDocContext _context;

        public Patient(HalloDocContext context)
        {
            _context = context;
        }
        public string CreateAcconut(CreateAccount model)
        {
            var emailExists = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);

            if (emailExists != null)
            {
                return "EmailExists";
            }

            if (model.PasswordHash != model.ConfirmPasswordHash)
            {
                return "PasswordNotMatch";
            }


            var user = _context.Requests.FirstOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                var newAspNetUser = new AspNetUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = user.PhoneNumber,
                };

                _context.AspNetUsers.Add(newAspNetUser);
                _context.SaveChanges();


                var userCheck = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);
                var newUser = new User
                {
                    AspNetUserId = userCheck.Id.ToString(),
                    Email = userCheck.Email,
                    Mobile = user.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    FirstName = user.FirstName,
                    LastName = user.LastName,


                };
                _context.Users.Add(newUser);
                _context.SaveChanges();

            }
            var users = _context.RequestClients.FirstOrDefault(u => u.Email == model.Email);
            if (users != null)
            {
                if (model.PasswordHash == model.ConfirmPasswordHash)
                {
                    var newAspNetUser = new AspNetUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PasswordHash = model.PasswordHash,
                        CreatedDate = DateTime.Now,
                    };

                    _context.AspNetUsers.Add(newAspNetUser);
                    _context.SaveChanges();

                    var userCheck = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);
                    var newUser = new User
                    {
                        AspNetUserId = userCheck.Id.ToString(),
                        Email = userCheck.Email,
                        Mobile = users.PhoneNumber,
                        CreatedDate = DateTime.Now,
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        State = users.State,
                        Street = users.Street,
                        City = users.City,
                        RegionId = users.RegionId,
                        ZipCode = users.ZipCode,
                        StrMonth = users.StrMonth,
                        IntDate = users.IntDate,
                        IntYear = users.IntYear,
                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                }
                var checkingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                List<RequestClient> clients = _context.RequestClients.ToList();
                foreach (var patient in clients)
                {
                    if (model.Email == patient.Email)
                    {
                        var patientCheck = _context.Requests.FirstOrDefault(u => u.RequestId == patient.RequestId);
                        patientCheck.PatientAccountId = checkingUser.UserId.ToString();
                        patientCheck.UserId = checkingUser.UserId;
                        _context.SaveChanges();
                    }
                }
            }

            
            //var patient = _context.RequestClients.FirstOrDefault(u => u.Email == model.Email);






            return "Yes";
        }
        public string PatientForm(PatientForm model)
        {
            /*if(ModelState)*/
            var emailExists = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);


            if (emailExists == null)
            {
                if (model.PasswordHash == model.ConfirmPasswordHash)
                {
                    var newAspNetUser = new AspNetUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PasswordHash = model.PasswordHash,
                        PhoneNumber = model.Mobile,
                        CreatedDate = DateTime.Now
                    };

                    _context.AspNetUsers.Add(newAspNetUser);
                    _context.SaveChanges();

                    emailExists = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email);

                    var newUser = new User
                    {
                        AspNetUserId = emailExists.Id.ToString(),
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Mobile = model.Mobile,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        ZipCode = model.ZipCode,
                        CreatedDate = DateTime.Now,
                        StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                        IntYear = model.BirthDate.Year,
                        IntDate = model.BirthDate.Day,

                    };

                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                }
                else
                {
                    return "PasswordNotMatch";
                }
            }

            var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            var regionCheck = _context.Regions.FirstOrDefault(u => u.Name == model.State);

            if (regionCheck == null)
            {
                return "RegionNotFound";
            }

            var newRequest = new Request
            {
                RequestTypeId = 1,
                UserId = userCheck.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Status = 1,
                PatientAccountId = userCheck.UserId.ToString(),
            };

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            var request = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.Email);

            var newRequestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                Email = model.Email,
                RegionId = regionCheck.RegionId,
                Address = model.Street + ' ' + model.City + ' ' + model.State,
                Notes = model.Symptomos,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                IntYear = model.BirthDate.Year,
                IntDate = model.BirthDate.Day,
            };

            _context.RequestClients.Add(newRequestClient);
            _context.SaveChanges();

            if (model.file != null && model.file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", model.file.FileName);
                using (var stream = File.Create(filePath))
                {
                    model.file.CopyTo(stream);
                    var user = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.Email);
                    RequestWiseFile file = new RequestWiseFile()
                    {
                        RequestId = user.RequestId,
                        FileName = model.file.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    _context.RequestWiseFiles.Add(file);
                    _context.SaveChanges();
                }
            }
            return "Yes";
        }


        public String ConciergeForm(ConciergeForm model)
        {
            var conciergeCheck = _context.Concierges.FirstOrDefault(u => u.Email == model.ConciergeEmail);

            if (conciergeCheck == null)
            {
                var newConcierge = new Concierge
                {
                    Email = model.ConciergeEmail,
                    Mobile = model.ConciergeMobile,
                    ConciergeName = model.ConciergeFirstName + " " + model.ConciergeLastName,
                };

                _context.Concierges.Add(newConcierge);
                _context.SaveChanges();
            }

            var regionCheck = _context.Regions.FirstOrDefault(u => u.Name == model.State);

            if (regionCheck == null)
            {
                return "RegionNotFound";
            }

            /*var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.PatientEmail);*/


            var newRequest = new Request
            {
                RequestTypeId = 3,
                FirstName = model.ConciergeFirstName,
                LastName = model.ConciergeLastName,
                PhoneNumber = model.ConciergeMobile,
                Email = model.ConciergeEmail,
                Status = 1,
                CreatedDate = DateTime.Now,
                RelationName = "Concierge",
                /* PatientAccountId = userCheck.UserId.ToString(),*/
            };
            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            var request = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.ConciergeEmail);

            var newRequestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.PatientFirstName,
                LastName = model.PatientLastName,
                PhoneNumber = model.PatientMobile,
                Email = model.PatientEmail,
                RegionId = regionCheck.RegionId,
                Address = model.Street + ' ' + model.City + ' ' + model.State,
                Notes = model.Symptomos,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                IntYear = model.BirthDate.Year,
                IntDate = model.BirthDate.Day,
            };

            _context.RequestClients.Add(newRequestClient);
            _context.SaveChanges();

            conciergeCheck = _context.Concierges.FirstOrDefault(u => u.Email == model.ConciergeEmail);


            var newRequestConcierge = new RequestConcierge
            {
                RequestId = request.RequestId,
                ConciergeId = conciergeCheck.ConciergeId,
            };

            _context.RequestConcierges.Add(newRequestConcierge);
            _context.SaveChanges();

            return "Yes";

        }

        public string FamilyForm(FamilyForm model)
        {
            var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.FamilyEmail);
            var patientCheck = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.PatientEmail);
            var regionCheck = _context.Regions.FirstOrDefault(u => u.Name == model.State);

            if (regionCheck == null)
            {
                return "RegionNotFound";
            }
            /* var userPatient = _context.Users.FirstOrDefault(u => u.Email == model.PatientEmail);*/
            var newRequest = new Request
            {
                RequestTypeId = 2,
                FirstName = model.FamilyFirstName,
                LastName = model.FamilyLastName,
                PhoneNumber = model.FamilyMobile,
                Email = model.FamilyEmail,
                Status = 1,
                CreatedDate = DateTime.Now,
                RelationName = model.Relation,
                /*PatientAccountId = userPatient.UserId.ToString(),*/
            };

            if (userCheck != null)
            {
               /* newRequest.UserId = userCheck.UserId;*/
                newRequest.CreatedUserId = userCheck.UserId;
                /* newRequest.PatientAccountId = patientCheck.Id.ToString();*/
            }

            var patientExistsCheck = _context.Users.FirstOrDefault(u => u.Email == model.PatientEmail);
            if (patientExistsCheck != null)
            {
                newRequest.PatientAccountId = patientExistsCheck.UserId.ToString();
                newRequest.UserId = patientExistsCheck.UserId;
            }


            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            var request = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.FamilyEmail);

            var newRequestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.PatientFirstName,
                LastName = model.PatientLastName,
                PhoneNumber = model.PatientMobile,
                Email = model.PatientEmail,
                RegionId = regionCheck.RegionId,
                Address = model.Street + ' ' + model.City + ' ' + model.State,
                Notes = model.Symptomos,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                IntYear = model.BirthDate.Year,
                IntDate = model.BirthDate.Day,
            };

            _context.RequestClients.Add(newRequestClient);
            _context.SaveChanges();

            if (model.file != null && model.file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", model.file.FileName);
                using (var stream = File.Create(filePath))
                {
                    model.file.CopyTo(stream);
                    var user = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.FamilyEmail);
                    RequestWiseFile file = new RequestWiseFile()
                    {
                        RequestId = user.RequestId,
                        FileName = model.file.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    _context.RequestWiseFiles.Add(file);
                    _context.SaveChanges();
                }
            }

            return "Yes";


        }

        public string BusinessForm(BusinessForm model)
        {
            var userCheck = _context.Users.FirstOrDefault(u => u.Email == model.BusinessEmail);
            var regionCheck = _context.Regions.FirstOrDefault(u => u.Name == model.State);
            if (regionCheck == null)
            {
                return "RegionNotFound";
            }
            var userPatient = _context.Users.FirstOrDefault(u => u.Email == model.PatientEmail);
            var newRequest = new Request
            {
                RequestTypeId = 4,
                FirstName = model.BusinessFirstName,
                LastName = model.BusinessLastName,
                PhoneNumber = model.BusinessMobile,
                Email = model.BusinessEmail,
                Status = 1,
                CreatedDate = DateTime.Now,
                RelationName = "Business",
            };

            if (userCheck != null)
            {
                newRequest.UserId = userCheck.UserId;
                newRequest.CreatedUserId = userCheck.UserId;
            }

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            var request = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.BusinessEmail);


            var newRequestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.PatientFirstName,
                LastName = model.PatientLastName,
                PhoneNumber = model.PatientMobile,
                Email = model.PatientEmail,
                RegionId = regionCheck.RegionId,
                Address = model.Street + ' ' + model.City + ' ' + model.State,
                Notes = model.Symptomos,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                IntYear = model.BirthDate.Year,
                IntDate = model.BirthDate.Day,
            };

            _context.RequestClients.Add(newRequestClient);
            _context.SaveChanges();

            return "Yes";
        }

        public string ForMeRequest(DashboardRequest model, string userId)
        {
            var userCheck = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId);

            var regionCheck = _context.Regions.FirstOrDefault(u => u.Name == model.State);

            if (regionCheck == null)
            {
                return "RegionNotFound";
            }

            var newRequest = new Request
            {
                RequestTypeId = 1,
                UserId = userCheck.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                CreatedDate = DateTime.Now,
                Email = model.Email,
                Status = 1,
                PatientAccountId = userCheck.UserId.ToString(),
            };

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            var request = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.Email);

            var newRequestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                Email = model.Email,
                RegionId = regionCheck.RegionId,
                Address = model.Street + ' ' + model.City + ' ' + model.State,
                Notes = model.Symptomos,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month),
                IntYear = DateTime.Now.Year,
                IntDate = DateTime.Now.Day,
            };

            _context.RequestClients.Add(newRequestClient);
            _context.SaveChanges();

            if (model.file != null && model.file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", model.file.FileName);
                using (var stream = File.Create(filePath))
                {
                    model.file.CopyTo(stream);
                    var user = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.Email);
                    RequestWiseFile file = new RequestWiseFile()
                    {
                        RequestId = user.RequestId,
                        FileName = model.file.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    _context.RequestWiseFiles.Add(file);
                    _context.SaveChanges();
                }
            }
            return "Yes";

        }

        public string ForSomeoneRequest(DashboardRequest model, string userId)
        {
            var userCheck = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId);

            var regionCheck = _context.Regions.FirstOrDefault(u => u.Name == model.State);

            if (regionCheck == null)
            {
                return "RegionNotFound";
            }

            var newRequest = new Request
            {
                RequestTypeId = 2,
                UserId = userCheck.UserId,
                FirstName = userCheck.FirstName,
                LastName = userCheck.LastName,
                PhoneNumber = userCheck.Mobile,
                CreatedDate = DateTime.Now,
                Email = userCheck.Email,
                Status = 1,
                RelationName = model.Relation,
            };

            _context.Requests.Add(newRequest);
            _context.SaveChanges();

            var request = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == userCheck.Email);

            var newRequestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                Email = model.Email,
                RegionId = regionCheck.RegionId,
                Address = model.Street + ' ' + model.City + ' ' + model.State,
                Notes = model.Symptomos,
                State = model.State,
                City = model.City,
                Street = model.Street,
                ZipCode = model.ZipCode,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month),
                IntYear = DateTime.Now.Year,
                IntDate = DateTime.Now.Day,
            };

            _context.RequestClients.Add(newRequestClient);
            _context.SaveChanges();

            if (model.file != null && model.file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", model.file.FileName);
                using (var stream = File.Create(filePath))
                {
                    model.file.CopyTo(stream);
                    var user = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(u => u.Email == model.Email);
                    RequestWiseFile file = new RequestWiseFile()
                    {
                        RequestId = user.RequestId,
                        FileName = model.file.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    _context.RequestWiseFiles.Add(file);
                    _context.SaveChanges();
                }
            }
            return "Yes";
        }

        public AspNetUser GetByEmailAndPassword(string email, string password)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }

        public bool DoesEmailExists(string email)
        {
            return _context.AspNetUsers.Any(u => u.Email == email);
        }

        public bool DoesEmailExistsForOtherUser(string email, string userId)
        {
            return _context.Users.Any(u => u.UserId.ToString() != userId && u.Email == email);
        }

        public List<RequestWiseFile> GetFilesByRequestId(string requestId)
        {
            return _context.RequestWiseFiles.Where(u => u.RequestId.ToString() == requestId).ToList();
        }

        public User GetByUserId(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId);
        }

        public void AddRequestWiseFile(RequestWiseFile file)
        {
            _context.RequestWiseFiles.Add(file);
            _context.SaveChanges();
        }

        public RequestWiseFile GetFilesById(int fileId)
        {
            return _context.RequestWiseFiles.FirstOrDefault(u => u.RequestWiseFileId == fileId);
        }
    }
}

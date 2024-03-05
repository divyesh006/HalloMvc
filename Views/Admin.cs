using HalloDoc_mvc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_mvc.Repository.IRepository;
using HalloDoc_mvc.Entities.Admin;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_mvc.Repository.Repository
{
    public class Admin : IAdmin
    {
        private readonly HalloDocContext _context;

        public Admin(HalloDocContext context)
        {
            _context = context;
        }

        public AdminDashboard AdminDashboard()
        {
            AdminDashboard DashBoardData = new AdminDashboard();
            DashBoardData.New = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 1).Count();
            DashBoardData.Pending = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 2).Count();
            DashBoardData.Active = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 4 || u.Request.Status == 5).Count();
            DashBoardData.Conclude = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 6).Count();
            DashBoardData.Close = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 3 || u.Request.Status == 7 || u.Request.Status == 8).Count();
            DashBoardData.Unpaid = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 9).Count();
            DashBoardData.region = _context.Regions.ToList();
            return DashBoardData;
        }

        public PatientData ViewCase(int id)
        {
            var patient = _context.RequestClients.FirstOrDefault(u => u.RequestId == id);
            var requester = _context.Requests.FirstOrDefault(u => u.RequestId == id);

            PatientData data = new PatientData()
            {
                client = patient,
                request = requester,
            };
            
            return data;
        }

        public List<NewRequestData> NewRequest()
        {
            var combinedData = (from request in _context.Requests
                                join client in _context.RequestClients
                                on request.RequestId equals client.RequestId
                                where request.Status == 1
                                select new NewRequestData
                                {
                                    FirstName = request.FirstName,
                                    RequestId = request.RequestId,
                                    CreatedDate = request.CreatedDate,
                                    RequestTypeId = request.RequestTypeId,
                                    IntDate = client.IntDate,
                                    RequesterPhoneNumber = request.PhoneNumber,
                                    CombinedId = client.RequestId,
                                    State = client.State,
                                    Address = client.Address,
                                    ClientPhoneNumber = client.PhoneNumber,
                                    StrMonth = client.StrMonth,
                                    IntYear = client.IntYear,
                                    ClientFirstName = client.FirstName,
                                    ClientLastName = client.LastName,
                                    Notes = client.Notes,
                                    RelationName = request.RelationName,
                                    Email = client.Email,
                                }).ToList();

            return combinedData;
        }

        public NewStatusData NewTable()
        {
            NewStatusData data = new NewStatusData();

            data.Clients = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 1).ToList();

            data.newCount = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 1).Count();

            data.Region = _context.Regions.ToList();

            return data;
        }

        public PendingStatusData PendingTable()
        {
            PendingStatusData data = new PendingStatusData();

            data.Clients = _context.RequestClients.Include(u => u.Request).Where(u => u.Request.Status == 2).ToList();

            data.Physicians = _context.Physicians.ToList();

            return data;
        }

        public List<Region> RegionData()
        {
            List<Region> regions = new List<Region>();
            regions = _context.Regions.ToList();
            return regions;
        }

        public ViewNotes GetNotes(int RequestId)
        {
            ViewNotes notes = new ViewNotes();
            notes.Notes = _context.RequestNotes.FirstOrDefault(u => u.RequestId == RequestId);
            notes.RequestId = RequestId;
            return notes;
        }

        public void UpdateAdminNote(ViewNotes newAdminNote)
        {
            var notes = _context.RequestNotes.FirstOrDefault(u => u.RequestId == newAdminNote.RequestId);
            notes.AdminNotes = newAdminNote.AdminNotes;
            _context.RequestNotes.Update(notes);
            _context.SaveChanges();
        }

        public CancelCase CancelCaseView(int RequestId)
        {
            CancelCase cancelCase = new CancelCase();
            cancelCase.RequestId = RequestId;
            var patient = _context.RequestClients.FirstOrDefault(u => u.RequestId == RequestId);
            cancelCase.PatientName = patient.FirstName + " " + patient.LastName;
            cancelCase.Case = _context.CaseTags.ToList();
            return cancelCase;
        }

        public void CancelCaseInsert(CancelCase model)
        {
            RequestStatusLog statusLog = new RequestStatusLog
            {
                RequestId = model.RequestId,
                Notes = model.AdditionalNotes,
                CreatedDate = DateTime.Now,
                Status = 3,
            };

            _context.RequestStatusLogs.Add(statusLog);
            _context.SaveChanges();

            var CancelReason = _context.CaseTags.FirstOrDefault(u => u.Name.Trim().ToLower() == model.Reason.Trim().ToLower());

            var patient = _context.Requests.FirstOrDefault(u => u.RequestId == model.RequestId);
            patient.Status = 3;
            patient.CaseTag = model.Reason;

            _context.Requests.Update(patient);
            _context.SaveChanges();
        }

        public List<Physician> GetProvidersByRegion(int RegionId)
        {
            List<Physician> regionWisePhsician = _context.Physicians.Where(u => u.RegionId == RegionId).ToList();
            return regionWisePhsician;
        }

        public AssignCase AssignCaseView(int RequestId)
        {
            AssignCase assignCase = new AssignCase();
            assignCase.RequestId = RequestId;
            assignCase.region = _context.Regions.ToList();
            return assignCase;
        }

        public void AssignCaseInsert(AssignCase model)
        {
            RequestStatusLog statusLog = new RequestStatusLog()
            {
                RequestId = model.RequestId,
                Notes = model.Description,
                CreatedDate = DateTime.Now,
                Status = 2,
                TransToPhysicianId = model.PhysicianId,
            };

            _context.RequestStatusLogs.Add(statusLog);
            _context.SaveChanges();

            var patient = _context.Requests.FirstOrDefault(u => u.RequestId == model.RequestId);
            patient.Status = 2;
            patient.PhysicianId = model.PhysicianId;
            patient.AcceptedDate = DateTime.Now;

            _context.Requests.Update(patient);
            _context.SaveChanges();
        }

        public BlockCase BlockCaseView(int RequestId)
        {
            BlockCase blockCase = new BlockCase();
            blockCase.RequestId = RequestId;

            var patient = _context.RequestClients.FirstOrDefault(u => u.RequestId == RequestId);
            blockCase.PatientName = patient.FirstName + ' ' + patient.LastName;

            return blockCase;
        }

        public void BlockCaseInsert(BlockCase model)
        {
            var patient = _context.RequestClients.FirstOrDefault(u => u.RequestId == model.RequestId);
            BlockRequest block = new BlockRequest
            {
                RequestId = model.RequestId.ToString(),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                CreatedDate = DateTime.Now,
                Reason = model.Reason,
            };
            _context.BlockRequests.Add(block);
            _context.SaveChanges();

            var request = _context.Requests.FirstOrDefault(u => u.RequestId == model.RequestId);
            request.Status = 0;
            _context.Requests.Update(request);
            _context.SaveChanges();
        }

        public ViewUplaod ViewUplaod(int RequestId)
        {
            ViewUplaod view = new ViewUplaod();
            view.RequestId = RequestId;

            var patient = _context.RequestClients.FirstOrDefault(u => u.RequestId == RequestId);
            var request = _context.Requests.FirstOrDefault(u => u.RequestId == RequestId);

            view.patientName = patient.FirstName + ' ' + patient.LastName;
            view.ConfirmationNumber = request.ConfirmationNumber;

            view.Requestfiles = _context.RequestWiseFiles.Where(u => u.RequestId == RequestId).ToList();

            return view;
        }
    }
}

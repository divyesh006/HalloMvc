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

        public List<Region> RegionData()
        {
            List<Region> regions = new List<Region>();
            regions = _context.Regions.ToList();
            return regions;
        }
    }
}

using HalloDoc_mvc.Entities.DataModels;
using HalloDoc_mvc.Entities.Admin;
using HalloDoc_mvc.Models;
using HalloDoc_mvc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using HalloDoc_mvc.Entities.DataModels;
using HalloDoc_mvc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Policy;

namespace HalloDoc_mvc.Controllers

{

    public class AdminController : Controller
    {


        public IAdmin _admin;
        private readonly HalloDocContext _context;

        public AdminController(HalloDocContext context, IAdmin admin)
        {
            _context = context;
            _admin = admin;
        }
        [Route("admin-dashboard")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult NewTable()
        {
            NewStatusData _newStatusData = _admin.NewTable();
           /* List<NewRequestData> newRequest = _admin.NewRequest();*/
            return PartialView("_NewTablePartial", _newStatusData);
        }

        //[Route("admin-dashboard/assigncase/{id}")]
        public IActionResult AssignCase(int id)
        {
            List<Region> regionData = _admin.RegionData();
            return PartialView("_AssignCasePartial",regionData);
        }

        [Route("admin-dashboard/view-doucument/{id}")]
        public IActionResult ViewCase(int id) 
        {
            PatientData patient = _admin.ViewCase(id);
            return View(patient); 
        }

        public IActionResult PendingTable()
        {
            return PartialView("_PendingTablePartial");
        }

        public IActionResult ActiveTable()
        {
            return PartialView("_ActiveTablePartial");
        }

        public IActionResult ConcludeTable()
        {
            return PartialView("_ConcludeTablePartial");
        }

        public IActionResult ToCloseTable()
        {
            return PartialView("_ToCloseTablePartial");
        }

        public IActionResult UnpaidTable()
        {
            return PartialView("_UnpaidTablePartial");
        }
    }
}

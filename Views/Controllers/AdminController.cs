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
            //List<Region> regionData = _admin.RegionData();
            AdminDashboard Data = _admin.AdminDashboard();
            return View(Data);
        }

        public IActionResult NewTable()
        {
            NewStatusData _newStatusData = _admin.NewTable();
           /* List<NewRequestData> newRequest = _admin.NewRequest();*/
            return PartialView("_NewTablePartial", _newStatusData);
        }

        public IActionResult SendLink()
        {
            return PartialView("_sendLinkPartial");
        }

        //[Route("admin-dashboard/assigncase/{id}")]
        public IActionResult AssignCase(int id)
        {
            AssignCase assignCase = _admin.AssignCaseView(id);
            return PartialView("_AssignCasePartial", assignCase);
        }

        public IActionResult AssignCaseInsert(AssignCase model)
        {
            _admin.AssignCaseInsert(model);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult GetProvidersByRegion(int RegionId)
        {
            var providers = _admin.GetProvidersByRegion(RegionId);
            return Json(providers);
        }

        public IActionResult CancelCase(int id)
        {
            CancelCase cancelCase = _admin.CancelCaseView(id);
            return PartialView("_CancelCasePartial", cancelCase);
        }   

        [HttpPost]
        public IActionResult CancelCaseInsert(CancelCase model)
        {
            _admin.CancelCaseInsert(model);
            return RedirectToAction("AdminDashboard");
        }

        public IActionResult BlockCase(int id)
        {
            BlockCase blockCase = _admin.BlockCaseView(id);
            return PartialView("_BlockCasePartial", blockCase);
        }

        public IActionResult BlockCaseInsert(BlockCase model)
        {
            _admin.BlockCaseInsert(model);
            return RedirectToAction("AdminDashboard");
        }

        [Route("admin-dashboard/view-doucument/{id}")]
        public IActionResult ViewCase(int id) 
        {
            PatientData patient = _admin.ViewCase(id);
            return View(patient); 
        }

        public IActionResult ViewNotes(int id)
        {
            ViewNotes Notes = _admin.GetNotes(id);
            return View(Notes);
        }

        public IActionResult AdminNotesUpdate(ViewNotes model)
        {
            _admin.UpdateAdminNote(model);
            return RedirectToAction("ViewNotes", new { id = model.RequestId });
        }

        public IActionResult PendingTable()
        {
            PendingStatusData _pendingStatusData = _admin.PendingTable();
            return PartialView("_PendingTablePartial", _pendingStatusData);
        }

        public IActionResult ViewUpload(int id)
        {
            ViewUplaod ViewUploadData = _admin.ViewUplaod(id);
            return View(ViewUploadData);
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

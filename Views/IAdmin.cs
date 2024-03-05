using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HalloDoc_mvc.Entities.Admin;
using HalloDoc_mvc.Entities.DataModels;

namespace HalloDoc_mvc.Repository.IRepository
{
    public interface IAdmin
    {
        public List<NewRequestData> NewRequest();

        public PatientData ViewCase(int id);

        public NewStatusData NewTable();

        public PendingStatusData PendingTable();

        public List<Region> RegionData();

        public ViewNotes GetNotes(int RequestId);

        public void UpdateAdminNote(ViewNotes newAdminNote);

        public CancelCase CancelCaseView(int RequestId);

        public void CancelCaseInsert(CancelCase model);

        public List<Physician> GetProvidersByRegion(int RegionId);

        public AssignCase AssignCaseView(int RequestId);

        public void AssignCaseInsert(AssignCase model);

        public BlockCase BlockCaseView(int RequestId);

        public void BlockCaseInsert(BlockCase model);

        public AdminDashboard AdminDashboard();

        public ViewUplaod ViewUplaod(int RequestId);
    }
}

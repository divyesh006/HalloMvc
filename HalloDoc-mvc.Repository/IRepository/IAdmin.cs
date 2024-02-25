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

        public List<Region> RegionData();
    }
}

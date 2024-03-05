using HalloDoc_mvc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_mvc.Entities.Admin
{
    public class PendingStatusData
    {
        public List<RequestClient> Clients { get; set; }

        public List<Physician> Physicians { get; set; }

    } 
}

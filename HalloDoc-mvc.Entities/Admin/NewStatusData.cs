using HalloDoc_mvc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_mvc.Entities.Admin
{
    public class NewStatusData
    {
        public List<RequestClient> Clients { get; set; }

        public int newCount { get; set; }

        public List<Region> Region { get; set; }

    } 
}

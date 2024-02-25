using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_mvc.Entities.DataModels;

namespace HalloDoc_mvc.Entities.Admin
{
    public class NewRequestData
    {
        public string FirstName { get; set; }

        public int RequestId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int RequestTypeId { get; set; }

        public int? IntDate { get; set; }

        public string RequesterPhoneNumber { get; set; }

        public int? CombinedId { get; set; }

        public string State { get; set; }

        public string Address { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string StrMonth { get; set; }

        public int? IntYear { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string Notes { get; set; }

        public string RelationName { get; set; }

        public string Email { get; set; }

       
    }


}

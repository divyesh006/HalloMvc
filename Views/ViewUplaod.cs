using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_mvc.Entities.DataModels;
using Microsoft.AspNetCore.Http;

namespace HalloDoc_mvc.Entities.Admin
{
    public class ViewUplaod
    {
       

        public int? RequestId { get; set; }

        public string? patientName { get; set; }

        public string? ConfirmationNumber { get; set; }

        public List<RequestWiseFile>? Requestfiles { get; set; }

        public IFormFile? file { get; set; }

        public string? month { get; set; }

        public int? year { get; set; }

        public int? date { get; set; }
    }
}

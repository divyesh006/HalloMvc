
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace HalloDoc.Entities.Patient
{
    public class FileUplaod
    {
        public int RequestId { get; set; }
        public IFormFile file { get; set; }

    }
}
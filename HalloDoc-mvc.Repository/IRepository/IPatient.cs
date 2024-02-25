using HalloDoc.Entities.Patient;
using HalloDoc_mvc.Entities.DataModels;
using HalloDoc_mvc.Entities.Patient;
using HalloDoc_mvc.Models;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_mvc.Repository.IRepository
{
    public interface IPatient
    {
        string CreateAcconut(CreateAccount model);

        string PatientForm(PatientForm model);

        string FamilyForm(FamilyForm model);

        string ConciergeForm(ConciergeForm model);

        string BusinessForm(BusinessForm model);

        string ForMeRequest(DashboardRequest model, string userId);

        string ForSomeoneRequest(DashboardRequest model, string userId);

        AspNetUser GetByEmailAndPassword(string email, string password);

        bool DoesEmailExists(string email);

        bool DoesEmailExistsForOtherUser(string email, string userId);

        List<RequestWiseFile> GetFilesByRequestId(string requestId);

        User GetByUserId(string userId);

        void AddRequestWiseFile(RequestWiseFile file);

        RequestWiseFile GetFilesById(int fileId);

    }
}

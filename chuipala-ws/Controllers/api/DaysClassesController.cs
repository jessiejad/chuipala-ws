using chuipala_ws.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace chuipala_ws.Controllers
{
    //[Authorize]
    public class DaysClassesController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DaysClasses
        public IEnumerable<ClassDTO> Get()
        {
            // En rapport avec l'user qui demande
            //var UserID = User.Identity.GetUserId().ToString();
            var UserID = "4e5eb817-f51a-4c62-a5fe-650c08032cb2";

            List<ClassDTO> ldto = new List<ClassDTO>();
            
            var user = db.Users.Find(UserID);

            if (user == null)
            {
                return ldto;
            }

            IEnumerable<Class> classes;

            if (user.IsProfessor)
            {
                classes = db.Classes.Where(x => x.ProfessorID == UserID);
            } else
            {
                classes = (from @class in db.Classes
                           join gc in db.GroupClasses on @class.ClassID equals gc.ClassID
                           join @group in db.Groups on gc.GroupID equals @group.GroupID
                           join gs in db.GroupStudents on @group.GroupID equals gs.GroupID
                           where (gs.StudentID == UserID)
                           select @class).ToList();
            }

            foreach (Class @class in db.Classes)
            {
                if(@class.StartDateTime.Date != DateTime.Now.Date)
                {
                    continue;
                }
                var nbAbsences = db.AbsenceClasses.Where(x => x.ClassID == @class.ClassID).Count(); ;
                var nbDelays = db.Delays.Where(x => x.ClassID == @class.ClassID).Count();

                var isProfessorAbsent = db.AbsenceClasses.Where(x => x.ClassID == @class.ClassID && x.Absence.UserID == @class.ProfessorID).Any();
                var isProfessorLate = db.Delays.Where(x => x.ClassID == @class.ClassID && x.UserID == @class.ProfessorID).Any();

                ldto.Add(new ClassDTO {
                    ClassID = @class.ClassID,
                    StartTime = @class.StartDateTime.ToString("H:mm"),
                    EndTime = @class.EndDateTime.ToString("H:mm"),
                    SubjectLabel = @class.SubjectLabel,
                    ProfessorFullName = @class.ProfessorIdentity,
                    NbAbsences = nbAbsences,
                    NbDelays = nbDelays,
                    IsProfessorAbsent = isProfessorAbsent,
                    IsProfessorLate = isProfessorLate
                });
            }
           
            return ldto;

        }
    }
}

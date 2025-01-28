using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AttendanceAPI;

namespace AttendanceAPI.Controllers
{
    public class SubjectsController : ApiController
    {
        private AttendanceDBEntities db = new AttendanceDBEntities();

        // GET: api/Subjects
        public IQueryable<Subjects> GetSubjects()
        {
            return db.Subjects;
        }

        // GET: api/Subjects/5
        [ResponseType(typeof(Subjects))]
        public IHttpActionResult GetSubjects(int id)
        {
            Subjects subjects = db.Subjects.Find(id);
            if (subjects == null)
            {
                return NotFound();
            }

            return Ok(subjects);
        }
        public async Task<IHttpActionResult> GetTeacher(int idTeacher)
        {
            var user = await db.Subjects
                 .FirstOrDefaultAsync(u => u.TeacherID == idTeacher);

            if (user == null)
                return Ok(new { message = "Предмет не найден" });

            return Ok(new { username = user.SubjectID + " " + user.SubjectName});
        }
        // PUT: api/Subjects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubjects(int id, Subjects subjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subjects.SubjectID)
            {
                return BadRequest();
            }

            db.Entry(subjects).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Subjects
        [ResponseType(typeof(Subjects))]
        public IHttpActionResult PostSubjects(Subjects subjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subjects.Add(subjects);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subjects.SubjectID }, subjects);
        }

        // DELETE: api/Subjects/5
        [ResponseType(typeof(Subjects))]
        public IHttpActionResult DeleteSubjects(int id)
        {
            Subjects subjects = db.Subjects.Find(id);
            if (subjects == null)
            {
                return NotFound();
            }

            db.Subjects.Remove(subjects);
            db.SaveChanges();

            return Ok(subjects);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectsExists(int id)
        {
            return db.Subjects.Count(e => e.SubjectID == id) > 0;
        }
    }
}
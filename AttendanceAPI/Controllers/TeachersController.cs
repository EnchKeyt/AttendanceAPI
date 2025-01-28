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
    public class TeachersController : ApiController
    {
        private AttendanceDBEntities db = new AttendanceDBEntities();
        private readonly AttendanceDBEntities _context;
        // GET: api/Teachers
        public IQueryable<Teachers> GetTeachers()
        {
            return db.Teachers;
        }

        // GET: api/Teachers/5
        [ResponseType(typeof(Teachers))]
        public IHttpActionResult GetTeachers(int id)
        {
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return NotFound();
            }

            return Ok(teachers);
        }
        public async Task<IHttpActionResult> GetTeacher(string login, string password)
        {
            var user = await db.Teachers
                 .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            if (user == null )
                return Ok(new { message = "Неверные учетные данные" });

            return Ok(new { message = "Авторизация успешна",  username = user.TeacherID + " " + user.FirstName +" " + user.LastName});
        }
        // PUT: api/Teachers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeachers(int id, Teachers teachers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teachers.TeacherID)
            {
                return BadRequest();
            }

            db.Entry(teachers).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeachersExists(id))
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

        // POST: api/Teachers
        [ResponseType(typeof(Teachers))]
        public IHttpActionResult PostTeachers(Teachers teachers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(teachers);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teachers.TeacherID }, teachers);
        }

  
        // DELETE: api/Teachers/5
        [ResponseType(typeof(Teachers))]
        public IHttpActionResult DeleteTeachers(int id)
        {
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(teachers);
            db.SaveChanges();

            return Ok(teachers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeachersExists(int id)
        {
            return db.Teachers.Count(e => e.TeacherID == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusReservation.Models;

namespace BusReservation.Controllers
{
    public class AdminController : ApiController
    {
        [HttpPost]
        [Route("api/Admin/Login")]
        public HttpResponseMessage Login([FromBody] Admin admin)
        {
            try
            {
                using (busreservationEntities db = new busreservationEntities())
                {
                    var data = db.Admins.Where(c => c.AdminId == admin.AdminId).FirstOrDefault();
                    if (data.Password == admin.Password)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Successfully Logged In As Admin");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Credentials");
                    }
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}

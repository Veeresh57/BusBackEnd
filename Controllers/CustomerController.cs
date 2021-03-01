using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusReservation.Models;

namespace BusReservation.Controllers
{
    public class CustomerController : ApiController
    {
        [HttpPost]
        [Route("api/Customer/Register")]
        public HttpResponseMessage Register([FromBody] Customer customer)
        {
            try
            {
                using (busreservationEntities db = new busreservationEntities())
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, customer);
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        [HttpPost]
        [Route("api/Customer/Login")]
        public HttpResponseMessage Login([FromBody] Customer customer)
        {
            try
            {
                using (busreservationEntities db = new busreservationEntities())
                {
                    var data = db.Customers.Where(c => c.CustomerId == customer.CustomerId).FirstOrDefault();
                    if (data.Password == customer.Password)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Successfully Logged In");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Credentials");
                    }
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        [HttpPut]
        [Route("api/Customer/PutCustomer/{CustomerId}")]
        public HttpResponseMessage PutCustomer(int CustomerId, [FromBody] Customer customer)
        {
            try
            {
                using (busreservationEntities db=new busreservationEntities())
                {
                    var data = db.Customers.Where(c=>c.CustomerId==customer.CustomerId).FirstOrDefault();
                    if(data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Not found");
                    }
                    else
                    {
                        data.DateOfBirth = customer.DateOfBirth;
                        data.EmailId = customer.EmailId;
                        data.MobileNumber = customer.MobileNumber;
                        data.Password = customer.Password;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusReservation.Models;
using System.Web.Http.Cors;

namespace BusReservation.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                    byte[] encpwd = new byte[customer.Password.Length];
                    encpwd = System.Text.Encoding.UTF8.GetBytes(customer.Password);
                    string epwd = Convert.ToBase64String(encpwd);
                    customer.Password = epwd;
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
                    var data = db.Customers.Where(c => c.EmailId == customer.EmailId).FirstOrDefault();
                    byte[] encpwd = new byte[customer.Password.Length];
                    encpwd = System.Text.Encoding.UTF8.GetBytes(customer.Password);
                    string epwd = Convert.ToBase64String(encpwd);
                    customer.Password = epwd;
                    if (data.Password == customer.Password)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, data.CustomerId);
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
        [HttpGet]
        [Route("api/Customer/GetCustomer/{Password}")]
        public HttpResponseMessage GetCustomer(string Password)
        {
            using (busreservationEntities db = new busreservationEntities())
            {
                byte[] encpwd = new byte[Password.Length];
                encpwd = System.Text.Encoding.UTF8.GetBytes(Password);
                string epwd = Convert.ToBase64String(encpwd);
                string Pwd = Password;
                Password = epwd;
                var data = db.Customers.Where(c=>c.Password==Password).FirstOrDefault();
                if (data != null)
                {
                    data.Password = Pwd;
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
                }
            }
        }
        [HttpGet]
        [Route("api/Customer/GetCust/{CustomerId}")]
        public HttpResponseMessage GetCust(int CustomerId)
        {
            using (busreservationEntities db = new busreservationEntities())
            {
                var data = db.Customers.Find(CustomerId);
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found");
                }
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

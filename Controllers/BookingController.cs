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
    public class BookingController : ApiController
    {
        [HttpGet]
        [Route("api/Booking/GetBookingDetails")]
        public HttpResponseMessage GetBookingDetails()
        {
            using (busreservationEntities db = new busreservationEntities())
            {
                var data = db.Bookings.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
        }
        [HttpGet]
        [Route("api/Booking/GetBookingDetails/{CustomerId}")]
        public HttpResponseMessage GetBookingDetails(int CustomerId)
        {

            using (busreservationEntities db = new busreservationEntities())
            {
                var data = db.GetBookbyCid(CustomerId).ToList();
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
        [HttpPost]
        [Route("api/Booking/Book")]
        public HttpResponseMessage Book([FromBody] Booking booking)
        {
            try
            {
                using (busreservationEntities db = new busreservationEntities())
                {
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, booking);
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        
        [HttpDelete]
        public HttpResponseMessage Cancel(int CustomerId, int TicketId)
        {
            try
            {
                using (busreservationEntities db = new busreservationEntities())
                {
                    var ticket = db.Bookings.Find(TicketId);
                    if(ticket.CustomerId==CustomerId)
                    {
                        db.Bookings.Remove(ticket);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, ticket);
                    }
                    else 
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cancelation Process Failed");
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

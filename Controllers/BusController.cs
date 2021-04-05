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
    public class BusController : ApiController
    {
        [HttpGet]
        [Route("api/Bus/GetBuses")]
        public HttpResponseMessage GetBuses()
        {
            using (busreservationEntities db=new busreservationEntities())
            {
                var data = db.Buses.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
        }
        [HttpGet]
        [Route("api/Bus/GetBuses/{BusId}")]
        public HttpResponseMessage GetBuses(int BusId)
        {
            using (busreservationEntities db = new busreservationEntities())
            {
                var data = db.Buses.Find(BusId);
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Bus ID");
                }

            }
        }
        [HttpGet]
        public HttpResponseMessage Search(string Start,string Destination,DateTime DateOfJourney)
        {
            using (busreservationEntities db=new busreservationEntities())
            {
                var data = db.GetBus(Start, Destination, DateOfJourney).ToList();
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bus Not Available");
                }
            }
        }
        
        [HttpPut]
        [Route("api/Bus/EditBus/{BusId}")]
        public HttpResponseMessage EditBus(int BusId,[FromBody] Bus bus)
        {
            try
            {
                using (busreservationEntities db = new busreservationEntities())
                {
                    var data = db.Buses.Find(BusId);
                    if (data == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bus Not found");
                    }
                    else
                    {
                        data.Start = bus.Start;
                        data.Destination = bus.Destination;
                        data.DateOfJourney = bus.DateOfJourney;
                        data.Availability = bus.Availability;
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
        [HttpPost]
        [Route("api/Bus/AddBus")]
        public HttpResponseMessage AddBus([FromBody] Bus bus)
        {
            try
            {
                using (busreservationEntities db=new busreservationEntities())
                {
                    db.Buses.Add(bus);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "New Bus Added");
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
        [HttpDelete]
        [Route("api/Bus/DeleteBus/{BusId}")]
        public HttpResponseMessage DeleteBus(int BusId)
        {
            using (busreservationEntities db=new busreservationEntities())
            {
                var data = db.Buses.Find(BusId);
                if (data == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bus Not Found");
                }
                else
                {
                    db.Buses.Remove(data);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
        }
    }
}

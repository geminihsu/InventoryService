﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InventoryService.Controllers.DbUtil;
using InventoryService.Models;

namespace InventoryService.Controllers
{
    public class LocationsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

      
        [Route("api/Locations")]
        public HttpResponseMessage Get()
        {
            var zoneCode = LocationRepository.GetAllLocation();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, zoneCode);
            return response;
        }

     
        [Route("~/api/Locations/{location:int}")]
        public HttpResponseMessage GetItemsByLocation(int location)
        {
            var zoneCode = LocationRepository.GetZoneByLocation(location.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, zoneCode);
            return response;
        }

       

        // POST: api/Locations
        [ResponseType(typeof(Location))]
        public IHttpActionResult PostLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LocationExists(location.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = location.Code }, location);
        }

        // DELETE: api/Locations/5
        [ResponseType(typeof(Location))]
        public IHttpActionResult DeleteLocation(string id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(string id)
        {
            return db.Locations.Count(e => e.Code == id) > 0;
        }
    }
}
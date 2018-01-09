using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class LocationRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();




        //Get all model Items from DB
        public static List<Location> GetAllLocation()
        {
            var query = from model in db.Locations orderby model.ZoneCode
                        select model;
            return query.ToList();
        }

   

        //Query Zone By location
        public static List<Location> GetZoneByLocation(string location)
        {
           
            if (location.Length == 1)
            {
                location = "00" + location;
            }else
            if (location.Length == 2)
            {
                location = "0" + location;
            }

            var query = from code in db.Locations
                        where code.Code.Contains(location)
                        select code;
            return query.ToList();
        }




        //Insert one Location into model table
        public static List<Location> InsertLocation(Location e)
        {
            db.Locations.Add(e);
            db.SaveChanges();
            return GetAllLocation();
        }

        //Insert more than one model into model table
        public static List<Location> InsertLocations(List<Location> e)
        {
            db.Locations.AddRange(e);
            db.SaveChanges();
            return GetAllLocation();
        }

        //update one model into model table
        public static List<Location> UpdateLocations(Location e)
        {
            var query = (from data in db.Locations
                        where data.Code.Contains(e.Code)
                        select data).SingleOrDefault();
            query.Code = e.Code;
            query.ZoneCode = e.ZoneCode;

            db.SaveChanges();
            return GetAllLocation();
        }


        //update more than one item into Inventory table
        public static List<Location> UpdateLocations(List<Location> e)
        {
            foreach (Location i in e)
            {
                var query = (from data in db.Locations
                             where data.Code.Contains(i.Code)
                             select data).SingleOrDefault();
                query.Code = i.Code;
                query.ZoneCode = i.ZoneCode;

            }

            db.SaveChanges();
            return GetAllLocation();
        }

        //delete one item from Inventory table
        public static List<Location> DeleteModel(Location e)
        {
            var query = (from data in db.Locations
                         where data.Code.Contains(e.Code)
                         select data).SingleOrDefault();
            db.Locations.Remove(query);
            db.SaveChanges();
            return GetAllLocation();
        }

        //delete more than one item from Inventory table
        public static List<Location> DeleteInventory(List<Location> e)
        {
          
            db.Locations.RemoveRange(e);
            db.SaveChanges();
            return GetAllLocation();
        }
    }


}
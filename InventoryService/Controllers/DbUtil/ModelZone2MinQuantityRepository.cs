using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ModelZone2MinQuantityRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();


        //Get all model Items from DB
        public static List<ModelZoneMap> GetAllModels()
        {
            var query = from model in db.ModelZoneMaps
                        select model;

            return query.ToList();
        }
        //Get all model Items from DB
        public static List<ModelZoneMap> GetAllModelsQty()
        {
            var query = from model in db.ModelZoneMaps
                        select model;

            var result = new List<ModelZoneMap>();

            foreach (ModelZoneMap i in query)
            {

                var zone2 = (from inventory in db.InventoryIns
                            where inventory.Location.Equals(i.Zone2Code) && inventory.ModelNo.Equals(i.Model)
                             select inventory).ToList();

                if (zone2.Count < i.Z2MinQty)
                {
                    //Query Zone 1 Quantity
                    var zone1 = (from inventory in db.InventoryIns
                                where inventory.Location.Equals(i.Zone1Code) && inventory.ModelNo.Equals(i.Model)
                                 select inventory).ToList();

                    if (zone1.Count < i.Z2MinQty)
                        continue;
                    else
                    {
                        i.Z2CurtQty = i.Z2MinQty - zone2.Count;
                        result.Add(i);
                    }
                }
                
            }

            return result;
        }



        //Query model Items By modelNo
        public static ModelZoneMap GetModel(String modelNo)
        {
            var query = from model in db.ModelZoneMaps
                        where model.Model.Contains(modelNo)
                        select model;
            return query.SingleOrDefault();
        }


        //Insert one model into model table
        public static List<ModelZoneMap> InsertInventory(ModelZoneMap e)
        {
            db.ModelZoneMaps.Add(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //Insert more than one model into model table
        public static List<ModelZoneMap> InsertInventory(List<ModelZoneMap> e)
        {
            db.ModelZoneMaps.AddRange(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //update one model into model table
        public static List<ModelZoneMap> UpdateInventory(ModelZoneMap e)
        {
            var model1 = (from model in db.ModelZoneMaps
                        where model.Model.Contains(model.Model)
                        select model).SingleOrDefault();
            /*model1.MODELNO = e.MODELNO;
            model1.VERSION = e.VERSION;
            model1.FG = e.FG;
            model1.MODEL1 = e.MODEL1;
            model1.SOLE = e.SOLE;
            model1.DESC = e.DESC;
            model1.FP_DATE = e.FP_DATE;
            model1.LABOR_W = e.LABOR_W;
            model1.ViewFile= e.ViewFile;
            model1.SPFile = e.SPFile;
            model1.Commercial = e.Commercial;
            model1.Brand = e.Brand;*/
           

            db.SaveChanges();
            return GetAllModels();
        }


        //update more than one item into Inventory table
        public static List<ModelZoneMap> UpdateReportRecord(List<ModelZoneMap> e)
        {
            foreach (ModelZoneMap i in e)
            {
                var model1 = (from model in db.ModelZoneMaps
                              where model.Model.Equals(model.Model)
                              select model).SingleOrDefault();
                model1.Model = i.Model;
                model1.FG = i.FG;

            }

            db.SaveChanges();
            return GetAllModels();
        }

        //delete one item from Inventory table
        public static List<ModelZoneMap> DeleteModel(ModelZoneMap e)
        {
            var model1 = (from model in db.ModelZoneMaps
                          where model.Model.Contains(model.Model)
                          select model).SingleOrDefault();
            db.ModelZoneMaps.Remove(model1);
            db.SaveChanges();
            return GetAllModels();
        }

        //delete more than one item from Inventory table
        public static List<ModelZoneMap> DeleteInventory(List<ModelZoneMap> e)
        {
          
            db.ModelZoneMaps.RemoveRange(e);
            db.SaveChanges();
            return GetAllModels();
        }
    }


}
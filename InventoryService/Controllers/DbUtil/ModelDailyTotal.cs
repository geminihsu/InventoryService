using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ModelDailyTotal
    {
        private static FGInventoryEntities db = new FGInventoryEntities();


        //Get all model Items from DB
        public static List<DailyTotal> GetAllModels()
        {
            var query = from model in db.DailyTotals
                        select model;

            return query.ToList();
        }
        //Get all model Items from DB
      

        //Query model Items By modelNo
        public static DailyTotal GetModel(String modelNo)
        {
            var query = from model in db.DailyTotals
                        where model.ModelNo.Contains(modelNo)
                        select model;
            return query.SingleOrDefault();
        }


        //Insert one model into model table
        public static List<DailyTotal> InsertInventory(DailyTotal e)
        {
            db.DailyTotals.Add(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //Insert more than one model into model table
        public static List<DailyTotal> InsertInventory(List<DailyTotal> e)
        {
            db.DailyTotals.AddRange(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //update one model into model table
        public static List<DailyTotal> UpdateInventory(DailyTotal e)
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
        public static List<DailyTotal> UpdateReportRecord(List<DailyTotal> e)
        {
            foreach (DailyTotal i in e)
            {
                var model1 = (from model in db.DailyTotals
                              where model.ModelNo.Equals(model.ModelNo)
                              select model).SingleOrDefault();
             
            }

            db.SaveChanges();
            return GetAllModels();
        }

        //delete one item from Inventory table
        public static List<DailyTotal> DeleteModel(DailyTotal e)
        {
            var model1 = (from model in db.DailyTotals
                          where model.ModelNo.Contains(model.ModelNo)
                          select model).SingleOrDefault();
            db.DailyTotals.Remove(model1);
            db.SaveChanges();
            return GetAllModels();
        }

        //delete more than one item from Inventory table
        public static List<DailyTotal> DeleteInventory(List<DailyTotal> e)
        {
          
            db.DailyTotals.RemoveRange(e);
            db.SaveChanges();
            return GetAllModels();
        }
    }


}
using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ReportsRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        

        //Get all model Items from DB
        public static List<Report> GetAllModels()
        {
            var query = from model in db.Reports
                        select model;
            return query.ToList();
        }



        //Query model Items By modelNo
        public static Report GetModel(String modelNo)
        {
            var query = from model in db.Reports
                        where model.Model.Contains(modelNo)
                        select model;
            return query.SingleOrDefault();
        }


        //Insert one model into model table
        public static List<Report> InsertInventory(Report e)
        {
            db.Reports.Add(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //Insert more than one model into model table
        public static List<Report> InsertInventory(List<Report> e)
        {
            db.Reports.AddRange(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //update one model into model table
        public static List<Report> UpdateInventory(Report e)
        {
            var model1 = (from model in db.Reports
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
        public static List<Report> UpdateReportRecord(List<Report> e)
        {
            foreach (Report i in e)
            {
                var model1 = (from model in db.Reports
                              where model.Model.Equals(model.Model)
                              select model).SingleOrDefault();
                model1.Model = i.Model;
                model1.FG = i.FG;
                model1.MinQuantity = i.MinQuantity;

            }

            db.SaveChanges();
            return GetAllModels();
        }

        //delete one item from Inventory table
        public static List<Report> DeleteModel(Report e)
        {
            var model1 = (from model in db.Reports
                          where model.Model.Contains(model.Model)
                          select model).SingleOrDefault();
            db.Reports.Remove(model1);
            db.SaveChanges();
            return GetAllModels();
        }

        //delete more than one item from Inventory table
        public static List<Report> DeleteInventory(List<Report> e)
        {
          
            db.Reports.RemoveRange(e);
            db.SaveChanges();
            return GetAllModels();
        }
    }


}
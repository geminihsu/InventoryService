using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ModelsRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        // Typed lambda expression for Select() method. 
        private static readonly Expression<Func<MODEL, ModelDto>> AsModelDto =
            x => new ModelDto
            {
                ModelNo = x.MODELNO,
                Version = x.VERSION,
                FG = x.FG,
                Model = x.MODEL1,
                Desc = x.DESC
            };


        //Get all model Items from DB
        public static List<MODEL> GetAllModels()
        {
            var query = from model in db.MODELs
                        select model;
            return query.ToList();
        }

        //Get all model Items from DB and display ModelDto format
        public static List<ModelDto> GetAllModelDto()
        {
            var query = from model in db.MODELs
                        select model;

            var items = new List<ModelDto>();

            foreach (MODEL x in query)
            {
                ModelDto m = new ModelDto
                {
                    ModelNo = x.MODELNO,
                    Version = x.VERSION,
                    FG = x.FG,
                    Model = x.MODEL1,
                    Desc = x.DESC
                };

                items.Add(m);
            }
                return items.ToList();
        }

        //Query model Items By modelNo
        public static MODEL GetModel(String modelNo)
        {
            var query = from model in db.MODELs
                        where model.MODELNO.Contains(modelNo)
                        select model;
            return query.SingleOrDefault();
        }


        //Query model Items By modelNo and display ModelDto format
        public static List<ModelDto> GetModelDtoByModeNo(String modelNo)
        {
            var query = from model in db.MODELs
                        where model.MODELNO.Contains(modelNo)
                        select model;

            var items = new List<ModelDto>();

            foreach (MODEL x in query)
            {
                ModelDto m = new ModelDto
                {
                    ModelNo = x.MODELNO,
                    Version = x.VERSION,
                    FG = x.FG,
                    Model = x.MODEL1,
                    Desc = x.DESC
                };

                items.Add(m);
            }
            return items.ToList();
        }


        //Insert one model into model table
        public static List<MODEL> InsertInventory(MODEL e)
        {
            db.MODELs.Add(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //Insert more than one model into model table
        public static List<MODEL> InsertInventory(List<MODEL> e)
        {
            db.MODELs.AddRange(e);
            db.SaveChanges();
            return GetAllModels();
        }

        //update one model into model table
        public static List<MODEL> UpdateInventory(MODEL e)
        {
            var model1 = (from model in db.MODELs
                        where model.MODELNO.Contains(model.MODELNO)
                        select model).SingleOrDefault();
            model1.MODELNO = e.MODELNO;
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
            model1.Brand = e.Brand;
           

            db.SaveChanges();
            return GetAllModels();
        }


        //update more than one item into Inventory table
        public static List<MODEL> UpdateInventory(List<MODEL> e)
        {
            foreach (MODEL i in e)
            {
                var model1 = (from model in db.MODELs
                              where model.MODELNO.Contains(model.MODELNO)
                              select model).SingleOrDefault();
                model1.MODELNO = i.MODELNO;
                model1.VERSION = i.VERSION;
                model1.FG = i.FG;
                model1.MODEL1 = i.MODEL1;
                model1.SOLE = i.SOLE;
                model1.DESC = i.DESC;
                model1.FP_DATE = i.FP_DATE;
                model1.LABOR_W = i.LABOR_W;
                model1.ViewFile = i.ViewFile;
                model1.SPFile = i.SPFile;
                model1.Commercial = i.Commercial;
                model1.Brand = i.Brand;

            }

            db.SaveChanges();
            return GetAllModels();
        }

        //delete one item from Inventory table
        public static List<MODEL> DeleteModel(MODEL e)
        {
            var model1 = (from model in db.MODELs
                          where model.MODELNO.Contains(model.MODELNO)
                          select model).SingleOrDefault();
            db.MODELs.Remove(model1);
            db.SaveChanges();
            return GetAllModels();
        }

        //delete more than one item from Inventory table
        public static List<MODEL> DeleteInventory(List<MODEL> e)
        {
          
            db.MODELs.RemoveRange(e);
            db.SaveChanges();
            return GetAllModels();
        }
    }


}
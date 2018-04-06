using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ModelInfoRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        //Get all Model Items from DB
        public static List<ModelInfo> GetAllModel()
        {
            var query = from inventory in db.ModelInfoes
                        select inventory;
            return query.ToList();
        }

        //Query Model record By Seq
        public static ModelInfo GetInventory(int itemSeq)
        {
            var query = from inventory in db.ModelInfoes
                        where inventory.Seq == itemSeq
                        select inventory;
            return query.SingleOrDefault();
        }

        //Query Model record By ModelNo
        public static ModelInfo GetModelByNo(String modelNo)
        {
            var query = from inventory in db.ModelInfoes
                        where inventory.ModelNo.Equals(modelNo)
                        select inventory;
            return query.SingleOrDefault();
        }


        //Insert one item into Model table
        public static List<ModelInfo> InsertModel(ModelInfo e)
        {
            db.ModelInfoes.Add(e);
            db.SaveChanges();
            return GetAllModel();
        }

        //Insert more than one item into Model table
        public static List<ModelInfo> InsertModel(List<ModelInfo> e)
        {
            db.ModelInfoes.AddRange(e);
            db.SaveChanges();
            return GetAllModel();
        }

        //update one item into Model table
        public static List<ModelInfo> UpdateInventoryModel(ModelInfo e)
        {
            var item = (from inventory in db.ModelInfoes
                        where inventory.Seq == e.Seq
                       select inventory).SingleOrDefault();
            item.ModelNo = e.ModelNo;
            item.Description = e.Description;
            db.SaveChanges();
            return GetAllModel();
        }


        //update more than one item into Inventory table
        public static List<ModelInfo> UpdateModel(List<ModelInfo> e)
        {
            foreach (ModelInfo i in e)
            {
                var item = (from inventory in db.ModelInfoes
                            where inventory.Seq == i.Seq
                            select inventory).SingleOrDefault();
                item.ModelNo = i.ModelNo;
                item.Description = i.Description;

            }

            db.SaveChanges();
            return GetAllModel();
        }

        //delete one item from Inventory table
        public static List<ModelInfo> DeleteModel(ModelInfo e)
        {
            var item = (from inventory in db.ModelInfoes
                        where inventory.Seq == e.Seq
                        select inventory).SingleOrDefault();
            db.ModelInfoes.Remove(item);
            db.SaveChanges();
            return GetAllModel();
        }

        //delete more than one item from Inventory table
        public static List<ModelInfo> DeleteModel(List<ModelInfo> e)
        {
          
            db.ModelInfoes.RemoveRange(e);
            db.SaveChanges();
            return GetAllModel();
        }

        public static void DeleteModel()
        {
            var query = from inventory in db.ModelInfoes
                        select inventory;

            foreach (ModelInfo i in query)
            {
                db.ModelInfoes.Remove(i);

            }
          
            db.SaveChanges();
        }
    }


}
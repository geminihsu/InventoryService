using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class InventoryRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        //Get all inventory Items from DB
        public static List<InventoryIn> GetAllInventory()
        {
            var query = from inventory in db.InventoryIns
                        select inventory;
            return query.ToList();
        }

        //Query inventory Items By Seq
        public static InventoryIn GetInventory(int itemSeq)
        {
            var query = from inventory in db.InventoryIns
                        where inventory.Seq == itemSeq
                        select inventory;
            return query.SingleOrDefault();
        }


        //Query inventory Items By Location
        public static List<InventoryIn> SearchInventoryByLocation(string location)
        {
            if (location.Length == 1)
                location  = "00" + location;

            if (location.Length == 2)
                location = "0" + location;
            var query = from inventory in db.InventoryIns
                        where inventory.Location.Equals(location)
                        select inventory;
            return query.ToList();
        }

        //Query inventory Items By model
        public static List<InventoryIn> SearchInventoryByModel(string modelNo)
        {
            var query = from inventory in db.InventoryIns
                        where inventory.ModelNo.Equals(modelNo) orderby inventory.SN
                        select inventory;
            return query.ToList();
        }

        //Insert one item into Inventory table
        public static List<InventoryIn> InsertInventory(InventoryIn e)
        {
            db.InventoryIns.Add(e);
            db.SaveChanges();
            return GetAllInventory();
        }

        //Insert more than one item into Inventory table
        public static List<InventoryIn> InsertInventory(List<InventoryIn> e)
        {
            db.InventoryIns.AddRange(e);
            db.SaveChanges();
            return GetAllInventory();
        }

        //update one item into Inventory table
        public static List<InventoryIn> UpdateInventory(InventoryIn e)
        {
            var item = (from inventory in db.InventoryIns
                       where inventory.Seq == e.Seq
                       select inventory).SingleOrDefault();
            item.SN = e.SN;
            item.Date = e.Date;
            item.Location = e.Location;
            item.ModelNo = e.ModelNo;

            db.SaveChanges();
            return GetAllInventory();
        }


        //update more than one item into Inventory table
        public static List<InventoryIn> UpdateInventory(List<InventoryIn> e)
        {
            foreach (InventoryIn i in e)
            {
                var item = (from inventory in db.InventoryIns
                            where inventory.SN == i.SN
                            select inventory).SingleOrDefault();
                item.SN = i.SN;
                item.Date = i.Date;
                item.Location = i.Location;
                item.ModelNo = i.ModelNo;

            }

            db.SaveChanges();
            return GetAllInventory();
        }

        //delete one item from Inventory table
        public static List<InventoryIn> DeleteInventory(InventoryIn e)
        {
            var item = (from inventory in db.InventoryIns
                        where inventory.Seq == e.Seq
                        select inventory).SingleOrDefault();
            db.InventoryIns.Remove(item);

            db.SaveChanges();
            return GetAllInventory();
        }

        //delete one item from Inventory table
        public static List<InventoryIn> DeleteInventory(History e)
        {
            var item = (from inventory in db.InventoryIns
                        where inventory.Seq == e.Seq
                        select inventory).SingleOrDefault();
            db.InventoryIns.Remove(item);

            db.SaveChanges();
            return GetAllInventory();
        }

        //delete more than one item from Inventory table
        public static List<InventoryIn> DeleteInventory(List<Shipping> e)
        {

            foreach (Shipping x in e)
            {

                var item = (from inventory in db.InventoryIns
                            where inventory.SN.Contains(x.SN)
                            select inventory).SingleOrDefault();
                db.InventoryIns.Remove(item);
            }
           
            db.SaveChanges();
            return GetAllInventory();
        }

        //delete more than one item from Inventory table
        public static List<InventoryIn> DeleteInventory(List<History> e)
        {

            foreach (History x in e)
            {

                var item = (from inventory in db.InventoryIns
                            where inventory.SN.Contains(x.SN)
                            select inventory).SingleOrDefault();
                db.InventoryIns.Remove(item);
            }

            db.SaveChanges();
            return GetAllInventory();
        }
    }


}
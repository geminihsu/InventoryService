using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ShippingRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        //Get all inventory Items from DB
        public static List<Shipping> GetAllShippingHistory()
        {
            var query = from inventory in db.Shippings
                        select inventory;
            return query.ToList();
        }

        //Query Shipping record By Seq
        public static Shipping GetInventory(int itemSeq)
        {
            var query = from inventory in db.Shippings
                        where inventory.Seq == itemSeq
                        select inventory;
            return query.SingleOrDefault();
        }


        //Query Shipping histoty By Date
        public static List<Shipping> SearchShippingByDate(DateTime datetime)
        {
            var query = from inventory in db.Shippings
                        where inventory.Date == datetime
                        select inventory;
            return query.ToList();
        }

        //Query inventory Items By model
        public static List<Shipping> SearchhShippingByModel(string modelNo)
        {
            var query = from inventory in db.Shippings
                        where inventory.ModelNo.Contains(modelNo) orderby inventory.SN
                        select inventory;
            return query.ToList();
        }

        //Insert one item into Inventory table
        public static List<Shipping> InsertInventory(Shipping e)
        {
            db.Shippings.Add(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //Insert more than one item into Inventory table
        public static List<Shipping> InsertInventory(List<Shipping> e)
        {
            db.Shippings.AddRange(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //update one item into Inventory table
        public static List<Shipping> UpdateInventory(Shipping e)
        {
            var item = (from inventory in db.Shippings
                        where inventory.Seq == e.Seq
                       select inventory).SingleOrDefault();
            item.SN = e.SN;
            item.Date = e.Date;
            item.Location = e.Location;
            item.ModelNo = e.ModelNo;

            db.SaveChanges();
            return GetAllShippingHistory();
        }


        //update more than one item into Inventory table
        public static List<Shipping> UpdateInventory(List<Shipping> e)
        {
            foreach (Shipping i in e)
            {
                var item = (from inventory in db.Shippings
                            where inventory.Seq == i.Seq
                            select inventory).SingleOrDefault();
                item.SN = i.SN;
                item.Date = i.Date;
                item.Location = i.Location;
                item.ModelNo = i.ModelNo;

            }

            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //delete one item from Inventory table
        public static List<Shipping> DeleteInventory(Shipping e)
        {
            var item = (from inventory in db.Shippings
                        where inventory.Seq == e.Seq
                        select inventory).SingleOrDefault();
            db.Shippings.Remove(item);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //delete more than one item from Inventory table
        public static List<Shipping> DeleteInventory(List<Shipping> e)
        {
          
            db.Shippings.RemoveRange(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }
    }


}
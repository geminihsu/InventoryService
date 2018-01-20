using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class HistoryRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        //Get all inventory Items from DB
        public static List<History> GetAllShippingHistory()
        {
            var query = from inventory in db.Histories
                        select inventory;
            return query.ToList();
        }

        //Query Shipping record By Seq
        public static History GetInventory(int itemSeq)
        {
            var query = from inventory in db.Histories
                        where inventory.Seq == itemSeq
                        select inventory;
            return query.SingleOrDefault();
        }


        //Query Shipping histoty By Date
        public static List<History> SearchShippingByDate(DateTime datetime)
        {
            var query = from inventory in db.Histories
                        where inventory.Date == datetime
                        select inventory;
            return query.ToList();
        }

        //Query inventory Items By model
        public static List<History> SearchShippingByModel(string modelNo)
        {
            var query = from inventory in db.Histories
                        where inventory.ModelNo.Equals(modelNo) orderby inventory.SN
                        select inventory;
            return query.ToList();
        }

        //Query inventory Items By salesOrder
        public static List<History> SearchShippingBySalesOrder(string salesOrder)
        {
            var query = from inventory in db.Histories
                        where inventory.SalesOrder.Equals(salesOrder)
                        orderby inventory.SN
                        select inventory;
            return query.ToList();
        }


        //Insert one item into Inventory table
        public static List<History> InsertInventory(History e)
        {
            db.Histories.Add(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //Insert more than one item into Inventory table
        public static List<History> InsertInventory(List<History> e)
        {
            db.Histories.AddRange(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //update one item into Inventory table
        public static List<History> UpdateInventory(History e)
        {
            var item = (from inventory in db.Histories
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
        public static List<History> UpdateInventory(List<History> e)
        {
            foreach (History i in e)
            {
                var item = (from inventory in db.Histories
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
        public static List<History> DeleteInventory(History e)
        {
            var item = (from inventory in db.Histories
                        where inventory.Seq == e.Seq
                        select inventory).SingleOrDefault();
            db.Histories.Remove(item);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //delete more than one item from Inventory table
        public static List<History> DeleteInventory(List<History> e)
        {
          
            db.Histories.RemoveRange(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }
    }


}
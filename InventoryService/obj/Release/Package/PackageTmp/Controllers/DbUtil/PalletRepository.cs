using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class PalletRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();



        //Get all Pallet Items from DB
        public static List<Pallet> GetAllPallets()
        {
            var query = from pallet in db.Pallets
                        orderby pallet.ShippedDate ascending
                        select pallet;
            return query.ToList();
        }



        //Query Pallet Items By salesorder
        public static Pallet GetPalletBySalesOrder(String salesorder)
        {
            var query = from pallet in db.Pallets
                        where pallet.SalesOrder.Equals(salesorder)
                        select pallet;
            return query.SingleOrDefault();
        }


        //Query Pallet Items By Date
        public static List<Pallet> GetPalletByDate(DateTime datetime)
        {
            var query = from pallet in db.Pallets
                        where pallet.ShippedDate >= datetime
                        select pallet;
            return query.ToList();
        }



        //Insert one model into model table
        public static List<Pallet> InsertPallet(Pallet e)
        {
            db.Pallets.Add(e);
            db.SaveChanges();
            return GetAllPallets();
        }

        //Insert more than one model into model table
        public static List<Pallet> InsertPallet(List<Pallet> e)
        {
            db.Pallets.AddRange(e);
            db.SaveChanges();
            return GetAllPallets();
        }

        //update one Pallet into Pallet table
        public static List<Pallet> UpdatePallet(Pallet e)
        {
            var model1 = (from pallet in db.Pallets
                          where pallet.SalesOrder.Contains(e.SalesOrder) & pallet.ItemID.Equals(e.ItemID)
                          select pallet).SingleOrDefault();
            model1.ItemID = e.ItemID;
            model1.SalesOrder = e.SalesOrder;
            model1.ShipCity = e.ShipCity;
            model1.ShippedDate = e.ShippedDate;
            model1.ShipState = e.ShipState;
            model1.ShipVia = e.ShipVia;
            model1.TrackingNo = e.TrackingNo;
            db.SaveChanges();
            return GetAllPallets();
        }



    }

}
using InventoryService.Common;
using InventoryService.DTOs;
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

        //Query Shipping histoty By SN
        public static List<History> SearchShippingBySN(String SN)
        {
            var query = from inventory in db.Histories
                        where inventory.SN.Equals(SN)
                        select inventory;
            return query.ToList();
        }


        //Query Shipping histoty By Date
        public static List<History> SearchShippingByDate(DateTime datetime)
        {
            var query = from inventory in db.Histories
                        where inventory.ShippedDate == datetime
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

        //Query Daily Shipping
        public static List<DailyShippingDto> SearchDailyShipping(DateTime datetime)
        {

            var tomorrow = datetime.AddDays(1);

            var query = from inventory in db.Histories
                        where inventory.ShippedDate >= datetime && inventory.ShippedDate < tomorrow
                        orderby inventory.CreatedDate ascending, inventory.SalesOrder ascending
                        group inventory by inventory.SalesOrder;

            var result = new List<DailyShippingDto>();

            foreach (var inventory in query)
            {
                var record = inventory.FirstOrDefault();

                Nullable<System.DateTime> CreatedDate = record.CreatedDate;
                string SO = record.SalesOrder;
                Dictionary<string, int> hash = new Dictionary<string, int>();
                foreach (var item in inventory)
                {
                    string ItemID = item.ModelNo;
                    ModelInfo model = ModelInfoRepository.GetModelByNo(ItemID);
                    string FG = model.Description;
                    string Qty = inventory.Count().ToString();
                    string Pro = item.TrackingNo;
                    string BillTo = item.BillTo;
                    String CustPo = item.CustPoNo;
                    string ShipVia = item.ShipVia;
                    string ShipState = item.ShipState;
                    string ShipCity = item.ShipCity;
                    System.DateTime ShippedDate = item.ShippedDate;

                    var shippingDto = new DailyShippingDto();

                    shippingDto.CreatedDate = CreatedDate;
                    shippingDto.SO = SO;
                    shippingDto.ItemID = item.ModelNo;

                    //Match each SO Qty not each model Qty
                    shippingDto.Qty = inventory.Count().ToString();
                    shippingDto.FG = FG;
                    shippingDto.SN = item.SN;
                    shippingDto.TrackingNo = Pro;
                    shippingDto.ShipVia = ShipVia;
                    shippingDto.BillTo = item.BillTo;
                    shippingDto.CustPo = CustPo;
                    shippingDto.ShipState = ShipState;
                    shippingDto.ShipCity = ShipCity;
                    shippingDto.ShippingDate = ShippedDate;
                    result.Add(shippingDto);

                }



                var pallet = from Pallet in db.Pallets
                             where Pallet.ShippedDate >= datetime && Pallet.SalesOrder.Equals(SO)
                             select Pallet;


                foreach (var item in pallet)
                {
                    var shippingDto2 = new DailyShippingDto();
                    shippingDto2.CreatedDate = item.CreatedDate;
                    shippingDto2.SO = item.SalesOrder;
                    shippingDto2.ItemID = item.ItemID;
                    shippingDto2.FG = item.FG;
                    shippingDto2.Qty = item.Qty.ToString();
                    shippingDto2.TrackingNo = item.TrackingNo;
                    shippingDto2.BillTo = item.BillTo;
                    shippingDto2.CustPo = item.CustPoNo;
                    shippingDto2.ShipVia = item.ShipVia;
                    shippingDto2.ShipState = item.ShipState;
                    shippingDto2.ShipCity = item.ShipCity;
                    shippingDto2.ShippingDate = item.ShippedDate;
                    result.Add(shippingDto2);
                }

            }
            //List<DailyShippingDto> SortedList = result.OrderByDescending(o => o.CreatedDate).ToList();
            return result;
        }

        //Query SN Items from History and Inventory Table
        public static List<FGSNRecordDto> SearchSNHistory(string serialNo)
        {
            var inventory = InventoryRepository.SearchInventoryBySNList(serialNo);
            var shipping = SearchShippingBySN(serialNo);

            var result = new List<FGSNRecordDto>();

            //add record from inventory table
            foreach (var item in inventory)
            {
                var record = new FGSNRecordDto();
                record.SN = item.SN;

                if (item.Location.Equals("333"))
                    record.Status = "Return";
                else
                    record.Status = "Received";
                record.Date = item.Date;
                record.Location = item.Location;
                record.ContainNo = item.ContainerNo;
                record.ZoneCode = LocationHelper.MapZoneCode(item.Location);
                result.Add(record);
            }

            //add record from history table
            foreach (var item in shipping)
            {
                var record = new FGSNRecordDto();
                record.SN = item.SN;
                record.Status = "Shipped";
                record.Date = item.ShippedDate;
                record.ContainNo = item.ContainerNo;
                record.SalesOrder = item.SalesOrder;
                result.Add(record);
            }

            List<FGSNRecordDto> SortedList = result.OrderBy(o => o.Date).ToList();

            return SortedList;
        }

        //Insert one item into History table
        public static List<History> InsertInventory(History e)
        {
            db.Histories.Add(e);
            db.SaveChanges();
            return GetAllShippingHistory();
        }

        //Insert more than one item into History table
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
            item.ShippedDate = e.ShippedDate;
            item.CreatedDate = e.CreatedDate;
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
                item.CreatedDate = i.CreatedDate;
                item.ShippedDate = i.ShippedDate;
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
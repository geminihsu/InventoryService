﻿using InventoryService.Common;
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
            var result = new List<ModelZoneMap>();

            foreach (ModelZoneMap i in query)
            {

                var zone2 = (from inventory in db.InventoryIns
                             where inventory.Location.Equals(i.Zone2Code) && inventory.ModelNo.Equals(i.Model)
                             select inventory
                             ).ToList();


                i.Z2CurtQty = (i.Z2MaxQty - zone2.Count);

                result.Add(i);
            }
            return result;

        }

        //Get all model Items from DB
        public static List<ModelZoneMap> GetAllInfoByModel(String modelNo)
        {
            var query = from model in db.ModelZoneMaps
                        where model.Model.Equals(modelNo)
                        select model;

            return query.ToList();
        }

        //Get replenishment table 
        public static List<ModelZoneMap> GetAllModelsQty()
        {
            
            var updateModelZone = GetDailyReportByModelsAndLocation();
            
            //Updated which zone 1 location for different model
            UpdateInventory(updateModelZone);

            var query = from model in db.ModelZoneMaps
                        orderby model.FG ascending
                        select model;

            var result = new List<ModelZoneMap>();

            foreach (ModelZoneMap i in query)
            {
                if (!DailyModelHelper.isShowModel(i.Model) || i.Zone2Code.Equals("111"))
                    continue;
                //Query Zone 2 Quantity
                var zone2 = (from inventory in db.InventoryIns
                             where inventory.Location.Equals(i.Zone2Code) && inventory.ModelNo.Equals(i.Model)
                             select inventory).ToList();

                if (zone2.Count < i.Z2MinQty || (zone2.Count() == 0 && DailyModelHelper.isShowModel(i.Model)))
                {
                    //Query Zone 1 Quantity
                    var zone1 = (from inventory in db.InventoryIns
                                 where inventory.Location.Equals(i.Zone1Code) && inventory.ModelNo.Equals(i.Model)
                                 select inventory).ToList();

                    if (zone1.Count <= 0)
                        i.Zone1Code = "N/A";
                    else
                        i.Z2CurtQty = (i.Z2MaxQty - zone2.Count);

                    result.Add(i);
                }

            }

       
            return result;

        }

        //Get all model Zone2fifo qty from DB
        public static List<FGDailyReplenishment> GetAllModelsZoneQty(int zoneCode)
        {
            var query = (from model in db.ModelZoneMaps
                         orderby model.Model descending
                         select model);
            var updateModelZone1 = new List<ModelZoneMap>();

            var result = new List<FGDailyReplenishment>();

            Dictionary<string, int> duplicatedModel = new Dictionary<string, int>();
            Dictionary<string, int> hashtable = new Dictionary<string, int>();
        
            foreach (ModelZoneMap i in query)
            {

                if (duplicatedModel.ContainsKey(i.Model))
                    continue;
                else
                    duplicatedModel.Add(i.Model,1);

                var inventoryQty = (from inventory in db.InventoryIns
                                    where inventory.ModelNo.Equals(i.Model)
                                    group inventory by new { inventory.SN, inventory.Location }
                                    into g
                                    orderby g.FirstOrDefault().SN
                                    let item = (from ig in g orderby ig.SN select ig).FirstOrDefault()
                                    select item)
                                    ;



                foreach (var inventory in inventoryQty)
                {

                    if (LocationHelper.MapZoneCode(inventory.Location) != zoneCode)
                        continue;

                 
                    var daily = new FGDailyReplenishment();
                    daily.ModelNo = i.Model;
                    daily.Description = i.FG;
                    daily.Location = inventory.Location;
                    daily.Qty += 1;



                    if (daily.Location != null)
                    {
                        if (!hashtable.ContainsKey(daily.ModelNo + daily.Location))
                        {
                            hashtable.Add(daily.ModelNo + daily.Location, daily.Qty);

                        }
                        else
                        {
                            daily.Qty = (int)hashtable[daily.ModelNo + daily.Location] + (int)daily.Qty;
                            hashtable[daily.ModelNo + daily.Location] = daily.Qty;
                        }


                    }

                }

            }

            Dictionary<string, string> duplicated = new Dictionary<string, string>();
            foreach (KeyValuePair<string, int> entry in hashtable)
            {
                var daily = new FGDailyReplenishment();
                var key = (string)entry.Key;
                if (duplicated.ContainsKey(key.Substring(0, 6)))
                    continue;
                duplicated.Add(key.Substring(0, 6), key.Substring(6));
                daily.ModelNo = key.Substring(0, 6);
                daily.Location = key.Substring(6);
                daily.Qty = (int)entry.Value;
                result.Add(daily);
            }


            return result;
        }


        //Get all model daily report from DB
        public static List<FGDailyReportDto> GetDailyReportByModels(DateTime date)
        {
            var query = (from model in db.ModelZoneMaps
                         orderby model.Brand ascending, model.Category
                         select model);

            var result = new List<FGDailyReportDto>();

            int modelTotal = 0;
            int zone1Count = 0;
            int zone2Count = 0;
            int zone1Received = 0;

            var yesterday = date.AddDays(-1);
            var tomorrow = date.AddDays(1);

            var modelDailyTotal = new List<DailyTotal>();

            foreach (ModelZoneMap i in query)
            {
                zone1Received = 0;
                FGDailyReportDto m = new FGDailyReportDto();
                m.ModelNo = i.Model;
                m.ModelFG = i.FG;
                m.Brand = i.Brand;
                m.Category = i.Category;

                var shipped = (from ship in db.Histories
                               where ship.ModelNo.Equals(i.Model) && ship.ShippedDate >= date && ship.ShippedDate < tomorrow
                               select ship).ToList();

                var shippedA = new List<Pallet>();

                if (m.ModelNo.Equals("135416"))
                {

                    shippedA = (from ship in db.Pallets
                                where ship.ItemID.Equals("135416A") && ship.ShippedDate >= date && ship.ShippedDate < tomorrow
                                select ship).ToList();
                }

                var afterShipped = (from ship in db.Histories
                                    where ship.ModelNo.Equals(i.Model) && ship.ShippedDate >= tomorrow
                                    select ship).ToList();

                m.Shipped = shipped.Count + shippedA.Count; ;

                int receivedShip = 0;

                int afterShippingScanItem = afterShipped.Count;

                foreach (var ship in afterShipped)
                {
                    if (ship.ScanDate != null && ship.ScanDate.Date == date.Date)
                    {
                        receivedShip++;
                    }
                    else if (ship.ScanDate.Date > date.Date)
                    {
                        afterShippingScanItem--;
                    }
                }

                var receivedItemByDate = (from inventory in db.InventoryIns
                                          where inventory.ModelNo.Equals(i.Model) && !inventory.Location.Equals(Constants.ZONE_CODE_RETURN.ToString()) && inventory.Date >= date && inventory.Date < tomorrow
                                          select inventory
                                          ).ToList();

                zone1Received = receivedShip + receivedItemByDate.Count();


                int rts = 0;
                int showroom = 0;
                int unshippable = 0;
                foreach (var inventory in receivedItemByDate)
                {
                    if (inventory.Location.Equals(Constants.ZONE_CODE_3_A.ToString()) || inventory.Location.Equals(Constants.ZONE_CODE_3_B.ToString()) || inventory.Location.Equals(Constants.ZONE_CODE_3_B.ToString()) || inventory.Location.Equals(Constants.ZONE_CODE_3_C.ToString()))
                        rts++;
                    else if (inventory.Location.Equals(Constants.ZONE_CODE_5_ONE.ToString()))
                        unshippable++;
                    else if (inventory.Location.Equals(Constants.ZONE_CODE_4_ONE.ToString()))
                        showroom++;
                }

        
                var receivedCurrent = (from inventory in db.InventoryIns
                                       where inventory.ModelNo.Equals(i.Model) && inventory.Date < date
                                       join code in db.Locations on inventory.Location equals code.Code
                                       orderby code.ZoneCode ascending, inventory.SN.Substring(6, 10) ascending
                                       group inventory by code.ZoneCode);
                modelTotal = 0;
                zone1Count = 0;
                zone2Count = 0;

                int count = receivedCurrent.Count();
                foreach (var inventory in receivedCurrent)
                {
                    Console.WriteLine(inventory.Count());

                    //mapped all values
                    if (inventory.Key == 1)
                        zone1Count = inventory.Count();
                    else if (inventory.Key == 2)
                        zone2Count = inventory.Count();
                    else if (inventory.Key == 3)
                        m.ReturnItem = inventory.Count();
                    else if (inventory.Key == 4)
                        m.ShowRoom = inventory.Count();
                    else if (inventory.Key == 5)
                        m.Rework = inventory.Count();
                    else if (inventory.Key == 6)
                        m.QC = inventory.Count();
                    else if (inventory.Key == 7)
                        m.Scrapped = inventory.Count();
                    else if (inventory.Key == 9)
                        continue;

                    modelTotal += inventory.Count();
                }



                if (showroom != 0)
                    m.ShowRoom += showroom;
                if (unshippable != 0)
                    m.Rework += unshippable;

                int afterSippedCnt = afterShippingScanItem - receivedShip;



                m.Received = zone1Received;
                m.OnHand = m.Received + zone1Count + zone2Count + afterSippedCnt - showroom - unshippable;

                m.Total = modelTotal + m.Received + afterSippedCnt;



                m.Previous = m.Total + m.Shipped - m.Received;


                if (m.Total > 0 || DailyModelHelper.isShowModel(m.ModelNo))
                {
                    result.Add(m);

                    DailyTotal dailyTotal = new DailyTotal();
                    dailyTotal.ModelNo = m.ModelNo;
                    dailyTotal.Date = date;
                    dailyTotal.Total = modelTotal;
                    modelDailyTotal.Add(dailyTotal);
                }

            }

            List<FGDailyReportDto> SortedList = result.OrderBy(o => o.Brand).ToList();



            ModelDailyTotal.InsertInventory(modelDailyTotal);
            return SortedList;
        }

        //Query model Items By modelNo
        public static ModelZoneMap GetModel(String modelNo)
        {
            var query = from model in db.ModelZoneMaps
                        where model.Model.Contains(modelNo)
                        select model;
            return query.SingleOrDefault();

        }

        //Get all model daily replenishment report from DB
        public static List<ModelZoneMap> GetDailyReportByModelsAndLocation()
        {

            var query = (from model in db.ModelZoneMaps
                         orderby model.Model descending
                         select model);
            var updateModelZone1 = new List<ModelZoneMap>();

            foreach (ModelZoneMap i in query)
            {
                var inventoryQty = (from inventory in db.InventoryIns
                                    where inventory.ModelNo.Equals(i.Model)
                                    group inventory by inventory.Location into g
                                    let count = g.Count()
                                    orderby count descending
                                    select new { Location = g.Key, Count = count }
                                    );

                var daily = new FGDailyReplenishment();
                foreach (var inventory in inventoryQty)
                {
                    if (LocationHelper.MapZoneCode(inventory.Location) != 1)
                        continue;

                    daily.ModelNo = i.Model;
                    daily.Description = i.FG;
                    daily.Location = inventory.Location;
                    daily.Qty = inventory.Count;

                    break;
                }

                if (daily.Location != null)
                {
                    i.Zone1Code = daily.Location;
                    updateModelZone1.Add(i);
                }
            }



            return updateModelZone1;
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
                          where model.Model.Equals(e.Model)
                          select model);

            foreach (ModelZoneMap i in model1)
            {

                i.Zone1Code = e.Zone1Code;
            }


            db.SaveChanges();
            return GetAllModels();
        }

        //update one model into model table
        public static List<ModelZoneMap> UpdateInventory(List<ModelZoneMap> e)
        {
            foreach (ModelZoneMap i in e)
            {
                var model1 = (from model in db.ModelZoneMaps
                              where model.Model.Equals(i.Model) && model.Zone2Code.Equals(i.Zone2Code)
                              select model);

                foreach (ModelZoneMap s in model1)
                {

                    s.Zone1Code = i.Zone1Code;
                }
            }
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
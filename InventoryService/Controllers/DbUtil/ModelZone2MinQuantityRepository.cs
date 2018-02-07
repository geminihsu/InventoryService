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

            return query.ToList();
        }
        //Get all model Items from DB
        public static List<ModelZoneMap> GetAllModelsQty()
        {
            var query = from model in db.ModelZoneMaps
                        select model;

            var result = new List<ModelZoneMap>();

            foreach (ModelZoneMap i in query)
            {
                //Query Zone 2 Quantity
                var zone2 = (from inventory in db.InventoryIns
                            where inventory.Location.Equals(i.Zone2Code) && inventory.ModelNo.Equals(i.Model)
                             select inventory).ToList();

                if (zone2.Count < i.Z2MinQty)
                {
                    //Query Zone 1 Quantity
                    var zone1 = (from inventory in db.InventoryIns
                                where inventory.Location.Equals(i.Zone1Code) && inventory.ModelNo.Equals(i.Model)
                                 select inventory).ToList();

                    if (zone1.Count < i.Z2MinQty)
                        continue;
                    else
                    {
                        i.Z2CurtQty = i.Z2MinQty - zone2.Count;
                        result.Add(i);
                    }
                }
                
            }

            return result;
        }

        //Get all model daily report from DB
        public static List<FGDailyReportDto> GetDailyReportByModels(DateTime date)
        {
            var query = from model in db.ModelZoneMaps
                        orderby model.FG, model.Model descending
                        select model;
            var result = new List<FGDailyReportDto>();

            int modelTotal = 0;
            int zone1Count = 0;
            int zone2Count = 0;

            var yesterday = date.AddDays(-1);
            var tomorrow = date.AddDays(1);

            var modelDailyTotal = new List<DailyTotal>();

            foreach (ModelZoneMap i in query)
            {

                FGDailyReportDto m = new FGDailyReportDto();
                m.ModelNo = i.Model;
                m.ModelFG = i.FG;

                var previous = (from prev in db.DailyTotals
                                where prev.ModelNo.Equals(i.Model) && prev.Date >= yesterday && prev.Date < date
                                select prev).SingleOrDefault();

                var shipped = (from ship in db.Histories
                               where ship.ModelNo.Equals(i.Model) && ship.Date >= date && ship.Date < tomorrow
                               select ship).ToList();
               
                if (previous == null)
                    m.Previous = 0;
                else
                    m.Previous = previous.Total;

                m.Shipped = shipped.Count;

                /*var receivedCurrent = (from inventory in db.InventoryIns
                                       where inventory.ModelNo.Equals(i.Model) && inventory.Date >= date
                                       group inventory by inventory.Location);*/

                var receivedCurrent = (from inventory in db.InventoryIns
                                       join code in db.Locations on inventory.Location equals code.Code
                                       orderby code.ZoneCode ascending, inventory.SN.Substring(6, 10) ascending
                                       where inventory.ModelNo.Equals(i.Model) /*&& inventory.Date >= date*/
                                       group inventory by code.ZoneCode);
                modelTotal = 0;
                zone1Count = 0;
                zone2Count = 0;
                foreach (var inventory in receivedCurrent)
                {
                    Console.WriteLine(inventory.Count());

                    //mapped all values
                    if (inventory.Key == 1)
                        zone1Count = inventory.Count();
                    if (inventory.Key == 2)
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
                    modelTotal += inventory.Count();
                }

                m.Received = zone1Count;
                m.OnHand = zone1Count + zone2Count;

                m.Total = modelTotal;

                if (m.Previous == 0)
                    m.Previous = m.Total + m.Shipped;

                if (m.Total > 0)
                {
                    result.Add(m);

                    DailyTotal dailyTotal = new DailyTotal();
                    dailyTotal.ModelNo = m.ModelNo;
                    dailyTotal.Date = date;
                    dailyTotal.Total = modelTotal;
                    modelDailyTotal.Add(dailyTotal);
                }

            }

            ModelDailyTotal.InsertInventory(modelDailyTotal);
            return result;
        }

        //Query model Items By modelNo
        public static ModelZoneMap GetModel(String modelNo)
        {
            var query = from model in db.ModelZoneMaps
                        where model.Model.Contains(modelNo)
                        select model;
            return query.SingleOrDefault();
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
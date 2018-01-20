using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class CustOrderRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();

        //Get all order Items from DB
        public static List<CustOrder> GetAllOrder()
        {
            var query = from order in db.CustOrders
                        select order;
            return query.ToList();
        }


        //Query order Items By SalesOrder
        public static List<CustOrder> SearchOrderBySalesOrder(string salesOrder)
        {
            var query = from order in db.CustOrders
                        where order.Sales_Order.Equals(salesOrder)
                        select order;
            return query.ToList();
        }


        //Insert one item into Inventory table
        public static List<CustOrder> InsertOrder(CustOrder e)
        {
            db.CustOrders.Add(e);
            db.SaveChanges();
            return GetAllOrder();
        }

        //Insert more than one item into Inventory table
        public static List<CustOrder> InsertOrder(List<CustOrder> e)
        {
            db.CustOrders.AddRange(e);
            db.SaveChanges();
            return GetAllOrder();
        }

        //update one item into Order table
        public static List<CustOrder> UpdateOrder(CustOrder e)
        {
            var item = (from order in db.CustOrders
                        where order.Sales_Order.Equals(e.Sales_Order)
                        where order.Item_ID.Equals(e.Item_ID)
                        select order).SingleOrDefault();

            item.Sales_Order = e.Sales_Order;
            item.Bill_to = e.Bill_to;
            item.Customer_PO = e.Customer_PO;
            item.Date = e.Date;
            item.Description = e.Description;
            item.Quantity = e.Quantity;
            item.Item_ID= e.Item_ID;
            item.Ship_to_Address_Line_One = e.Ship_to_Address_Line_One;
            item.Ship_to_City = e.Ship_to_City;
            item.Ship_to_Country = e.Ship_to_Country;
            item.Ship_to_State = e.Ship_to_State;
            item.Ship_to_Zipcode = e.Ship_to_Zipcode;
            item.Ship_Via = e.Ship_Via;
            item.Closed = e.Closed;

            db.SaveChanges();
            return GetAllOrder();
        }


        //update more than one item into Order table
        public static List<CustOrder> UpdateOrder(List<CustOrder> e)
        {
            foreach (CustOrder i in e)
            {
                var item = (from order in db.CustOrders
                            where order.Sales_Order.Equals(i.Sales_Order)
                            where order.Item_ID.Equals(i.Item_ID)
                            select order).SingleOrDefault();

                item.Sales_Order = i.Sales_Order;
                item.Bill_to = i.Bill_to;
                item.Customer_PO = i.Customer_PO;
                item.Date = i.Date;
                item.Description = i.Description;
                item.Quantity = i.Quantity;
                item.Item_ID = i.Item_ID;
                item.Ship_to_Address_Line_One = i.Ship_to_Address_Line_One;
                item.Ship_to_City = i.Ship_to_City;
                item.Ship_to_Country = i.Ship_to_Country;
                item.Ship_to_State = i.Ship_to_State;
                item.Ship_to_Zipcode = i.Ship_to_Zipcode;
                item.Ship_Via = i.Ship_Via;
                item.Closed = i.Closed;

            }

            db.SaveChanges();
            return GetAllOrder();
        }

        //delete one item from Order table
        public static List<CustOrder> DeleteOrder(CustOrder e)
        {
            var item = (from order in db.CustOrders
                        where order.Sales_Order.Equals(e.Sales_Order)
                        where order.Item_ID.Equals(e.Item_ID)
                        select order).SingleOrDefault();

            db.CustOrders.Remove(item);

            db.SaveChanges();
            return GetAllOrder();
        }

        //delete more than one item from Order table
        public static List<CustOrder> DeleteOrder(List<CustOrder> e)
        {

            foreach (CustOrder x in e)
            {

                var item = (from order in db.CustOrders
                            where order.Sales_Order.Equals(x.Sales_Order)
                            where order.Item_ID.Equals(x.Item_ID)
                            select order).SingleOrDefault();
                db.CustOrders.Remove(item);
            }
           
            db.SaveChanges();
            return GetAllOrder();
        }
    }


}
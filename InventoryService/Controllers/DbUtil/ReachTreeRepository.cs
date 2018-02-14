﻿using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ReachTreeRepository
    {
        //Get Data from PeachTree
        public static void retrievePeachTree()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["PeachreeSNOConnectionString"].ToString();
            DataTable dt = new DataTable();
            OdbcConnection cn; //= new OdbcConnection(connString);


            dt = new DataTable(); //reset

            //Peachtree

            using (cn = new OdbcConnection(connString))
            {
                string sqlCheck = @"
                        SELECT        LineItem.ItemID, LineItem.ItemDescription, CONVERT(JrnlRow.Quantity, SQL_INTEGER) AS QTY, JrnlHdr.Reference, JrnlRow.RowDate
                        FROM            JrnlRow, LineItem, JrnlHdr
                        WHERE        JrnlRow.ItemRecordNumber = LineItem.ItemRecordNumber AND JrnlRow.PostOrder = JrnlHdr.PostOrder AND (JrnlRow.Quantity > 0) AND (JrnlHdr.Module <> 'R') AND (JrnlRow.RowDate = '2017-12-04') and (LineItem.ItemID like 'T%') ";
                OdbcCommand sqlCmd = new OdbcCommand(sqlCheck, cn);
                //                sqlCmd.Parameters.Add("?", OdbcType.VarChar).Value = (rec.ServiceId.ToString() + "-" + rec.OrderId.ToString());
                OdbcDataAdapter adapter = new OdbcDataAdapter(sqlCmd);
                adapter.Fill(dt);
            }

            var sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                var arr = row.ItemArray.Select(i => i.ToString()).ToArray();
                var line = String.Join(",", arr);
                sb.Append(line + "\r\n");
            }
            var str = sb.ToString();
            Console.WriteLine(str);
        }


        //Get Data from PeachTree
        public static List<CustOrder> getSalesOrderInfo(String _salesOrder)
        {
            String salesOrder = _salesOrder;
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["PeachreeSNOConnectionString"].ToString();
            DataTable order = new DataTable();
            DataTable orderItem = new DataTable();
            OdbcConnection cn; //= new OdbcConnection(connString);

            var result = new List<CustOrder>();


            order = new DataTable(); //reset
            orderItem = new DataTable(); //reset
            //Peachtree

            using (cn = new OdbcConnection(connString))
            {
                string sqlCheck = @"
                        SELECT        Reference, CONVERT(TransactionDate, SQL_CHAR) AS TDate, Description,  ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZIP, ShipToCountry, ShipVia, CustomerInvoiceNo ,POSOisClosed
                        FROM          JrnlHdr
                        WHERE         Reference = '" + salesOrder + "' AND (JrnlHdr.Module <> 'R')";
                OdbcCommand sqlCmd = new OdbcCommand(sqlCheck, cn);
                //                sqlCmd.Parameters.Add("?", OdbcType.VarChar).Value = (rec.ServiceId.ToString() + "-" + rec.OrderId.ToString());
                OdbcDataAdapter adapter = new OdbcDataAdapter(sqlCmd);
                adapter.Fill(order);
            }

            using (cn = new OdbcConnection(connString))
            {
                string sqlCheck = @"
                        SELECT        ItemID, ItemDescription, SUM(QTY) AS QTY
                        FROM          (SELECT        LineItem.ItemID, LineItem.ItemDescription, CONVERT(JrnlRow.Quantity, SQL_INTEGER) AS QTY
                        FROM            JrnlRow, LineItem, JrnlHdr
                        WHERE        JrnlRow.ItemRecordNumber = LineItem.ItemRecordNumber 
                                     AND JrnlRow.PostOrder = JrnlHdr.PostOrder 
                                     AND(JrnlHdr.Reference = '" + salesOrder + "') AND(JrnlRow.Quantity > 0) AND(JrnlHdr.Module <> 'R')) view1 GROUP BY ItemID, ItemDescription";
                OdbcCommand sqlCmd = new OdbcCommand(sqlCheck, cn);
                //                sqlCmd.Parameters.Add("?", OdbcType.VarChar).Value = (rec.ServiceId.ToString() + "-" + rec.OrderId.ToString());
                OdbcDataAdapter adapter = new OdbcDataAdapter(sqlCmd);
                adapter.Fill(orderItem);
            }

            String orderNo = "";
            DateTime date = new DateTime();
            String description = "";
            String billTo = "";
            String ShipToAddress1 = "";
            String ShipToAddress2 = "";
            String ShipToCity = "";
            String ShipToState = "";
            String ShipToZIP = "";
            String ShipToCountry = "";
            String ShipVia = "";
            String CustomerInvoiceNo = "";
            String POSOisClosed = "";

            var sb = new StringBuilder();
            foreach (DataRow row in order.Rows)
            {
                var arr = row.ItemArray.Select(i => i.ToString()).ToArray();
                orderNo = arr[0];
                date = Convert.ToDateTime(arr[1]); ;
                description = arr[2];
                billTo = arr[3];
                ShipToAddress1 = arr[4];
                ShipToAddress2 = arr[5];
                ShipToCity = arr[6];
                ShipToState = arr[7];
                ShipToZIP = arr[8];
                ShipToCountry = arr[9];
                ShipVia = arr[10];
                CustomerInvoiceNo = arr[11];
                POSOisClosed = arr[12];
            }

            var sbItem = new StringBuilder();
            foreach (DataRow row in orderItem.Rows)
            {
                CustOrder orderInfo = new CustOrder();
                var arr = row.ItemArray.Select(i => i.ToString()).ToArray();
                orderInfo.Sales_Order = orderNo;
                orderInfo.Description = arr[1];
                orderInfo.Bill_to = billTo;
                orderInfo.Date = date;
                orderInfo.Ship_to_Address_Line_One = ShipToAddress1;
                orderInfo.Ship_to_City = ShipToCity;
                orderInfo.Ship_to_State = ShipToState;
                orderInfo.Ship_to_Zipcode = ShipToZIP;
                orderInfo.Ship_to_Country = ShipToCountry;
                orderInfo.Ship_Via = ShipVia;
                orderInfo.Customer_PO = CustomerInvoiceNo;
                orderInfo.Closed = POSOisClosed.Equals("1") ? true : false; ;

                orderInfo.Item_ID = arr[0];
                orderInfo.Quantity = Convert.ToInt32(arr[2]);

                result.Add(orderInfo);
            }

            return result;
        }

      /*  //Get Data from PeachTree
        public static List<CustOrder> copyCustOrderFromPeachTree()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["PeachreeSNOConnectionString"].ToString();
            DataTable dt = new DataTable();
            OdbcConnection cn; //= new OdbcConnection(connString);


            dt = new DataTable(); //reset

            //Peachtree

            using (cn = new OdbcConnection(connString))
            {
                string sqlCheck = @"
                        SELECT         Reference, CONVERT(TransactionDate, SQL_CHAR) AS TDate, Description, ShipToName, ShipToAddress1, ShipToAddress2, ShipToCity, ShipToState, ShipToZIP, ShipToCountry, ShipVia, CustomerInvoiceNo, POSOisClosed
                        FROM            JrnlHdr
                        WHERE          (TDate > '2018-01-01')
                        ORDER BY TDate DESC
                        ";
                OdbcCommand sqlCmd = new OdbcCommand(sqlCheck, cn);
                //                sqlCmd.Parameters.Add("?", OdbcType.VarChar).Value = (rec.ServiceId.ToString() + "-" + rec.OrderId.ToString());
                OdbcDataAdapter adapter = new OdbcDataAdapter(sqlCmd);
                adapter.Fill(dt);
            }

            var sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                var arr = row.ItemArray.Select(i => i.ToString()).ToArray();
                var line = String.Join(",", arr);
                sb.Append(line + "\r\n");
            }
        }*/
    }


}
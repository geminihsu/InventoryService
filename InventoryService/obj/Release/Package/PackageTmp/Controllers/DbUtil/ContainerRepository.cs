using InventoryService.DTOs;
using InventoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InventoryService.Controllers.DbUtil
{
    public class ContainerRepository
    {
        private static FGInventoryEntities db = new FGInventoryEntities();



        //Get all model Items from DB
        public static List<Container> GetAllContainers()
        {
            var query = from container in db.Containers
                        orderby container.Date ascending
                        where container.Close == false
                        select container;
            return query.ToList();
        }



        //Query Container Items By containerNo
        public static Container GetContainer(String containerNo)
        {
            var query = from container in db.Containers
                        where container.ContainerNo.Contains(containerNo)

                        select container;
            return query.SingleOrDefault();
        }


        //Query Container Items By Date
        public static List<Container> GetContainerByDate(DateTime datetime)
        {
            var query = from container in db.Containers
                        where container.Date == datetime
                        select container;
            return query.ToList();
        }

        //Insert one model into model table
        public static List<Container> InsertContainer(Container e)
        {
            db.Containers.Add(e);
            db.SaveChanges();
            return GetAllContainers();
        }

        //Insert more than one model into model table
        public static List<Container> InsertContainer(List<Container> e)
        {
            db.Containers.AddRange(e);
            db.SaveChanges();
            return GetAllContainers();
        }

        //update one container into Container table
        public static List<Container> UpdateContainer(Container e)
        {
            var model1 = (from container in db.Containers
                          where container.ContainerNo.Contains(container.ContainerNo)
                          select container).SingleOrDefault();
            model1.Date = e.Date;
            model1.ContainerNo = e.ContainerNo;
            model1.SNBegin = e.SNBegin;
            model1.SNEnd = e.SNEnd;
            model1.Close = e.Close;

            db.SaveChanges();
            return GetAllContainers();
        }


        //update more than one item into Inventory table
        public static List<Container> UpdateContainer(List<Container> e)
        {
            foreach (Container i in e)
            {
                var containerItem = (from container in db.Containers
                                     where container.Seq.Equals(i.Seq)
                                     where container.ContainerNo.Equals(i.ContainerNo)
                                     select container).SingleOrDefault();
                containerItem.Date = i.Date;
                containerItem.ContainerNo = i.ContainerNo;
                containerItem.SNBegin = i.SNBegin;
                containerItem.SNEnd = i.SNEnd;
                containerItem.Close = i.Close;

            }

            db.SaveChanges();
            return GetAllContainers();
        }

        //delete one item from Inventory table
        public static List<Container> DeleteContainer(int seq)
        {
            var containerItem = (from container in db.Containers
                                 where container.Seq == seq
                                 select container).SingleOrDefault();
            db.Containers.Remove(containerItem);
            db.SaveChanges();
            return GetAllContainers();
        }

        //delete more than one item from Inventory table
        public static List<Container> DeleteContainer(List<Container> e)
        {

            db.Containers.RemoveRange(e);
            db.SaveChanges();
            return GetAllContainers();
        }
    }


}
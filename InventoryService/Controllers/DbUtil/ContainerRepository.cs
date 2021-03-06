﻿using InventoryService.DTOs;
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
        public static List<Container> GetContainer(String containerNo)
        {
            var query = from container in db.Containers
                        where container.ContainerNo.Equals(containerNo)

                        select container;
            return query.ToList();
        }


        //Query Container Items By Date
        public static List<Container> GetContainerByDate(DateTime datetime)
        {
            var query = from container in db.Containers
                        where container.Date == datetime
                        select container;
            return query.ToList();
        }

        public static Boolean isSNexsit(List<Container> e)
        {
            var containerItem = (from container in db.Containers
                                 select container);
            foreach (Container i in e)
            {
                foreach (Container s in containerItem)
                {
                    if (s.SNBegin.Equals(i.SNBegin) || s.SNEnd.Equals(i.SNEnd))
                        return true;
                    else if (s.SNBegin.Equals(i.SNEnd) || s.SNEnd.Equals(i.SNBegin))
                        return true;
                }
            }
            return false;
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
                          where container.ContainerNo.Equals(container.ContainerNo) || container.SNBegin.Equals(container.SNBegin)
                          select container).ToList();

            if (model1.Count() == 1)
            {
                foreach (Container i in model1)
                {
                    i.Date = e.Date;
                    i.ContainerNo = e.ContainerNo;
                    i.SNBegin = e.SNBegin;
                    i.SNEnd = e.SNEnd;
                    i.Close = e.Close;

                }
            }
            else
            {

                foreach (Container i in model1)
                {
                    i.Date = e.Date;

                }
            }

            db.SaveChanges();
            return GetAllContainers();
        }


        //update more than one item into Inventory table
        public static List<Container> UpdateContainer(List<Container> e)
        {
            foreach (Container i in e)
            {
                Container containerItem;
                if (i.Seq != 0)
                {
                    var containers = (from container in db.Containers
                                      where container.ContainerNo.Equals(i.ContainerNo) || container.SNBegin.Equals(i.SNBegin)
                                      select container).ToList();
                    if (containers.Count() == 1)
                    {
                        containerItem = containers.SingleOrDefault();
                        containerItem.Date = i.Date;
                        containerItem.ContainerNo = i.ContainerNo;
                        containerItem.ModelNo = i.ModelNo;
                        containerItem.SNBegin = i.SNBegin;
                        containerItem.SNEnd = i.SNEnd;
                        containerItem.Close = i.Close;

                    }
                    else
                    {
                        foreach (Container item in containers)
                        {
                            item.Date = i.Date;
                            item.Close = i.Close;
                        }
                    }
                }
                else
                {
                    containerItem = (from container in db.Containers
                                     where container.ContainerNo.Equals(i.ContainerNo) && container.SNBegin.Equals(i.SNBegin) && container.SNEnd.Equals(i.SNEnd)
                                     select container).SingleOrDefault();
                    containerItem.Date = i.Date;
                    containerItem.ContainerNo = i.ContainerNo;
                    containerItem.ModelNo = i.ModelNo;
                    containerItem.SNBegin = i.SNBegin;
                    containerItem.SNEnd = i.SNEnd;
                    containerItem.Close = i.Close;

                }




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
/*
 * Name: Maad Abduljaleel
 * Date: 17-06-2020
 * Class name: OrderDAO.cs
 * Purpose: A class that controls saving an order to the database and sending all the info to client side.
 */

using CaseStudy.APIHelpers;
using CaseStudy.DAL.DomainClasses;
using CaseStudy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseStudy.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;

        //a bool to determine weather we need backOrders or not.
        public bool backOrders { get; set; }

        //Constructor that takes an instance of the database (AppDbContext)
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }


        /* 
         * Method Name: AddOrder
         * Accepts: int (userid), Array(OrderSelectionHelper)
         * Returns: int (OrderID)
         * Purpose: saving an order to the database and returning the OrderID.
         */
        public int AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int OrderId = -1;
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order Order = new Order();
                        Order.UserId = userid;
                        Order.OrderDate = DateTime.Now;
                        Order.OrderAmount = 0.0M;
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            Order.OrderAmount += selection.item.MSRP * selection.Qty;

                        }
                        _db.Orders.Add(Order);
                        _db.SaveChanges();

                        // then add each item to the Orderitems table
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            OrderLineItem olItem = new OrderLineItem();
                            olItem.QtyOrdered = selection.Qty;
                            olItem.ProductId = selection.item.Id;
                            olItem.OrderId = Order.Id;
                            //Check if the QtyOnHand is greater than the selected Qty
                            //If it is then we're not adding anything to the QtyBackOrdered, however the QtyOnHand is updated and saved.
                            if (selection.item.QtyOnHand > selection.Qty)
                            {
                                selection.item.QtyOnHand -= selection.Qty;
                                olItem.QtySold = selection.Qty;
                                olItem.QtyBackOrdered = 0;
                                _db.Products.Update(selection.item);
                                _db.SaveChanges();
                            }
                            //Else it will add the amount that needs to be backorderd to the QtyBackOrdered and saves it.
                            else
                            {
                                selection.item.QtyOnBackOrder += (selection.Qty - selection.item.QtyOnHand);
                                olItem.QtyBackOrdered = (selection.Qty - selection.item.QtyOnHand);
                                olItem.QtySold = selection.item.QtyOnHand;
                                backOrders = true;
                                _db.Products.Update(selection.item);
                                _db.SaveChanges();
                                selection.item.QtyOnHand = 0;
                            }
                            olItem.SellingPrice = Convert.ToDecimal(selection.item.MSRP);
                            _db.OrderLineItems.Add(olItem);
                            _db.SaveChanges();
                        }
                       
                        _trans.Commit();
                        OrderId = Order.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return OrderId;
        }

        /* 
        * Method Name: GetAll
        * Accepts: int id
        * Returns: List<Order>
        * Purpose: returning all the orders that are ordered by a certain user
        */
        public List<Order> GetAll(int id)
        {
            return _db.Orders.Where(tray => tray.UserId == id).ToList();
        }


        /* 
        * Method Name: GetOrderDetails
        * Accepts: int oid, string email
        * Returns: List<OrderDetailsHelper>
        * Purpose: returning all the detail for an order for a specific user
        */
        public List<OrderDetailsHelper> GetOrderDetails(int oid, string email)
        {
            Customer cus = _db.Customers.FirstOrDefault(cus => cus.Email == email);
            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();
            // LINQ way of doing INNER JOINS
            var results = from o in _db.Orders
                          join oli in _db.OrderLineItems on o.Id equals oli.OrderId
                          join pi in _db.Products on oli.ProductId equals pi.Id
                          where (o.UserId == cus.Id && o.Id == oid)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              UserId = cus.Id,
                              ProductId = oli.ProductId,
                              ProductName = pi.ProductName,
                              Description = pi.Description,
                              QtySold = oli.QtySold,
                              QtyBackOrdered = oli.QtyBackOrdered,
                              QtyOrdered = oli.QtyOrdered,
                              OrderAmount = o.OrderAmount,
                              productPrice = pi.MSRP,
                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = results.ToList();
            return allDetails;
        }
    }
}

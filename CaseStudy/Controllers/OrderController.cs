/*
 * Name: Maad Abduljaleel
 * Date: 17-06-2020
 * Class name: OrderController.cs
 * Purpose: controller that is responsible for saving the orders.
 */
using System;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using CaseStudy.APIHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CaseStudy.Helpers;

namespace ExercisesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;

        //Constructor
        public OrderController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]

        /* 
         * Method Name: Index
         * Accepts: OrderHelper Instance
         * Returns: ActionResult<string>
         * Purpose: saving an order and shows if the order needs to be back ordered.
         */
        public ActionResult<string> Index(OrderHelper helper)
        {
            string retVal = "";
            try
            {
                CustomerDAO cDao = new CustomerDAO(_ctx);
                Customer orderOwner = cDao.GetByEmail(helper.email);
                OrderDAO oDao = new OrderDAO(_ctx);
                int orderId = oDao.AddOrder(orderOwner.Id, helper.selections);
                if (orderId > 0)
               
                {
                    retVal = "Order " + orderId + " saved!";
                    if(oDao.backOrders)
                    {
                        retVal += "Goods Backordered!";
                    }
                }
                else
                {
                    retVal = "Order not saved";
                }
            }
            catch (Exception ex)
            {
                retVal = "Order not saved " + ex.Message;
            }
            return retVal;
        }



        /* 
         * Method Name: List
         * Accepts: string email
         * Returns: ActionResult<List<Order>>
         * Purpose: returning a list of orders for the specified email
         */
        [HttpGet("{email}")]
        public ActionResult<List<Order>> List(string email)
        {
            List<Order> orders = new List<Order>();
            CustomerDAO cDao = new CustomerDAO(_ctx);
            Customer orderOwner = cDao.GetByEmail(email);
            OrderDAO oDao = new OrderDAO(_ctx);
            orders = oDao.GetAll(orderOwner.Id);
            return orders;
        }

        /* 
         * Method Name: GetOrderDetails
         * Accepts: int orderid, string email
         * Returns: ActionResult<List<OrderDetailsHelper>>
         * Purpose: returning all the details for a specified order
         */
        [HttpGet("{orderid}/{email}")]
        public ActionResult<List<OrderDetailsHelper>> GetOrderDetails(int orderid, string email)
        {
            OrderDAO dao = new OrderDAO(_ctx);
            return dao.GetOrderDetails(orderid, email);
        }

    }
}

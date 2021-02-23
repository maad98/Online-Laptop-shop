/*
 * Name: Maad Abduljaleel
 * Date: 17-06-2020
 * Class name: BranchController.cs
 * Purpose: controller that is responsible for showing the 3 closes stores on client side
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        AppDbContext _db;

        //Constructor
        public BranchController(AppDbContext context)
        {
            _db = context;
        }


        /* 
       * Method Name: Index
       * Accepts: float (lat), float (long)
       * Returns: ActionResult<List<Branch>>
       * Purpose: showing the 3 closest branches to client side.
       */
        [HttpGet("{lat}/{lng}")]
        public ActionResult<List<Branch>> Index(float lat, float lng)
        {
            BranchDAO dao = new BranchDAO(_db);
            return dao.GetThreeClosestBranches(lat, lng);
        }

    }
}
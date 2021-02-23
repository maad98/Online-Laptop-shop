/*
 * Name: Maad Abduljaleel
 * Date: 01-06-2020
 * Class name: BrandController.cs
 * Purpose: controls displaying the brands.
 */
using System.Collections.Generic;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        AppDbContext _db;

        //Constructor

        public BrandController(AppDbContext context)
        {
            _db = context;
        }

        /* 
         * Method Name: Index
         * Accepts: -
         * Returns: ActionResult<List<Brand>>
         * Purpose: list all the brands in the database
         */
        public ActionResult<List<Brand>> Index()
        {
            brandDAO dao = new brandDAO(_db);
            List<Brand> allBrands = dao.GetAll();
            return allBrands;
        }
    }
}
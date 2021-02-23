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
    public class BrandItemController : ControllerBase
    {
        AppDbContext _db;
        public BrandItemController(AppDbContext context)
        {
            _db = context;
        }
        [Route("{brandid}")]
        public ActionResult<List<Product>> Index(int brandid)
        {
            BrandItemDAO dao = new BrandItemDAO(_db);
            List<Product> itemsForBrand = dao.GetAllByBrand(brandid);
            return itemsForBrand;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using CaseStudy.DAL.DomainClasses;

namespace CaseStudy.DAL.DAO
{
    public class BrandItemDAO
    {
        private AppDbContext _db;
        public BrandItemDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Product> GetAllByBrand(int id)
        {
            return _db.Products.Where(item => item.Brand.Id == id).ToList();
        }
    }
}

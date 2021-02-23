using CaseStudy.DAL.DomainClasses;
using System.Collections.Generic;
using System.Linq;

namespace CaseStudy.DAL.DAO
{
    public class brandDAO
    {
        private AppDbContext _db;
        public brandDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Brand> GetAll()
        {
            return _db.Brands.ToList<Brand>();
        }
    }
}

/*
 * Name: Maad Abduljaleel
 * Date: 17-06-2020
 * Class name: BranchDAO.cs
 * Purpose: a class responsible for returning the 3 closest branches to client side.
 */
using CaseStudy.DAL.DomainClasses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.DAL.DAO
{
    public class BranchDAO
    {
        private AppDbContext _db;

        //Constructor
        public BranchDAO(AppDbContext context)
        {
            _db = context;
        }



        /* 
         * Method Name: GetThreeClosestBranches
         * Accepts: float (lat), float (long)
         * Returns: List<Branch>
         * Purpose: returning the 3 closest branches to client side through an SQL stored procedure
         */
        public List<Branch> GetThreeClosestBranches(float? lat, float? lng)
        {
            List<Branch> BranchDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lngParam = new SqlParameter("@lng", lng);
                var query = _db.Branches.FromSqlRaw("dbo.pGetThreeClosestBranches @lat, @lng", latParam,
                lngParam);
                BranchDetails = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BranchDetails;
        }


    }
}

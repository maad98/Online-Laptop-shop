/*
 * Name: Maad Abduljaleel
 * Date: 01-06-2020
 * Class name: RegisterController.cs
 * Purpose: controls the customer registration using e-mail.
 */

using System;
using System.Security.Cryptography;
using CaseStudy.DAL;
using CaseStudy.DAL.DAO;
using CaseStudy.DAL.DomainClasses;
using CaseStudy.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CaseStudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        AppDbContext _db;
        public RegisterController(AppDbContext context)
        {
            _db = context;
        }
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        
        /* 
         * Method Name: Index
         * Accepts: CustomerHelper Instance
         * Returns: ActionResult<CustomerHelper>
         * Purpose: Registering a customer in the database
         */
        public ActionResult<CustomerHelper> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer already = dao.GetByEmail(helper.email);
            if (already == null)
            {
                HashSalt hashSalt = GenerateSaltedHash(64, helper.password);
                helper.password = ""; // don’t need the string anymore
                Customer dbCustomer = new Customer();
                dbCustomer.Firstname = helper.firstname;
                dbCustomer.Lastname = helper.lastname;
                dbCustomer.Email = helper.email;
                dbCustomer.Hash = hashSalt.Hash;
                dbCustomer.Salt = hashSalt.Salt;
                dbCustomer = dao.Register(dbCustomer);
                if (dbCustomer.Id > 0)
                    helper.token = "Customer registered";
                else
                    helper.token = "Customer registration failed";
            }
            else
            {
                helper.token = "Customer registration failed - email already in use";
            }
            return helper;
        }

        /* 
        * Method Name: GenerateSaltedHash
        * Accepts: an integer of size, a string of password
        * Returns: instance HashSalt 
        * Purpose: used to authenticate passwords and mapping it to a fixed length
        */
        private static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            // Fills an array of bytes with a cryptographically strong sequence of random nonzero values.
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            // a password, salt, and iteration count, then generates a binary key
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }
    }
    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}

/*
 * Name: Maad Abduljaleel
 * Date: 01-06-2020
 * Class name: OrderSelectionHelper.cs
 * Purpose: A helper class for the OrderDAO that saves orderQty, and an instance of the Product class.
 */
using CaseStudy.DAL.DomainClasses;
namespace CaseStudy.APIHelpers
{
    public class OrderSelectionHelper
    {
        public int Qty { get; set; }
        public Product item { get; set; }
    }
}

/*
 * Name: Maad Abduljaleel
 * Date: 01-06-2020
 * Class name: OrderHelper.cs
 * Purpose: A helper class for the OrderDAO that saves a string (email), and an array of OrderSelectionHelper class.
 */
namespace CaseStudy.APIHelpers
{
    public class OrderHelper
    {
        public string email { get; set; }
        public OrderSelectionHelper[] selections { get; set; }
    }
}

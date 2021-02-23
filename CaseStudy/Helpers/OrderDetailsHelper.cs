using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        [StringLength(15)]

        public string ProductId { get; set; }

        public int UserId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string DateCreated { get; set; }

        [Column(TypeName = "money")]
        public decimal OrderAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal productPrice { get; set; }
        public int QtySold { get; set; }
        public int QtyBackOrdered { get; set; }

        public int QtyOrdered { get; set; }
    }
}

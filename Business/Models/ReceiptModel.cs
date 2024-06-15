using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ReceiptModel : BaseModel
    {
        public int CustomerId { get; set; }
        public DateTime OperationDate { get; set; }
        public bool IsCheckedOut { get; set; }
        public ICollection<int> ReceiptDetailsIds { get; set; }
    }
}

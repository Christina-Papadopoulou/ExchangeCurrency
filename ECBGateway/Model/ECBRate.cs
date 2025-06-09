using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECBGateway.Model
{
    public class ECBRate
    {
        public long Id { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}

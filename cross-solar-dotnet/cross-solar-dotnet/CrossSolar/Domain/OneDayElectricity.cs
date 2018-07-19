using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossSolar.Domain
{
    public class OneDayElectricity

    {
        public int Id { get; set; }

        public string PanelId { get; set; }

        public decimal Sum { get; set; }

        public decimal Average { get; set; }

        public decimal Maximum { get; set; }

        public decimal Minimum { get; set; }

        public DateTime DateTime { get; set; }

    }
}

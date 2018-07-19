using System;

namespace CrossSolar.Models
{
    public class OneDayElectricityModel
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
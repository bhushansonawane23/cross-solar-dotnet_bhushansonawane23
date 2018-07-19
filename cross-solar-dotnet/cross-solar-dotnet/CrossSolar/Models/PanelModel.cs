using System.ComponentModel.DataAnnotations;

namespace CrossSolar.Models
{
    public class PanelModel
    {
        public int Id { get; set; }

        [Required]
        [Range(-90, 90)]
        [RegularExpression(@"^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{6}")]
        public double Latitude { get; set; }

        [Required]
        [RegularExpression(@"^-?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{6}")]
        [Range(-180, 180)] public double Longitude { get; set; }

        [Required]
        [RegularExpression(@"^.{16}$")]
        public string Serial { get; set; }

        public string Brand { get; set; }
    }
}
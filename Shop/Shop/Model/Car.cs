using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Model
{
    public class Car
    {
        public string VinNumber { get; set; }
        public string StsNumber { get; set; }
        public string LicensePlate { get; set; }
        public string BrandModel { get; set; }
        public string SteeringPosition { get; set; }
        public int Year { get; set; }
        public string BodyType { get; set; }
        public int Mileage { get; set; }
        public string ExteriorColor { get; set; }
        public bool IsGboInstalled { get; set; }
        public bool IsRepairNeeded { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsTradePossible { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public List<string> PhotosUrl = new List<string>();
    }
}

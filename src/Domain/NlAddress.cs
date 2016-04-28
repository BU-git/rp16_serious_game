using System.Collections.Generic;

namespace Domain
{
    public class NlAddress
    {
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string HouseNumberAddition { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; }
        public string Province { get; set; }
        public int RdX { get; set; }
        public int RdY { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string BagNumberDesignationId { get; set; }
        public string BagAddressableObjectId { get; set; }
        public string AddressType { get; set; }
        public List<string> Purposes { get; set; }
        public int SurfaceArea { get; set; }
        public List<string> HouseNumberAdditions { get; set; }
    }
}

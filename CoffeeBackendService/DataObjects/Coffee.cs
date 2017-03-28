using Microsoft.Azure.Mobile.Server;

namespace CoffeeBackendService.DataObjects
{
    public class Coffee : EntityData
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Stars { get; set; }
    }
}
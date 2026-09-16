using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace FloodPrediction
{
    [DataContract]
    public class WeatherRoot
    {
        [DataMember(Name = "main")]
        public MainData Main { get; set; }

        [DataMember(Name = "rain")]
        public RainData Rain { get; set; }
    }

    [DataContract]
    public class MainData
    {
        [DataMember(Name = "temp")]
        public double Temp { get; set; }

        [DataMember(Name = "humidity")]
        public double Humidity { get; set; }

        [DataMember(Name = "pressure")]
        public double Pressure { get; set; }
    }

    [DataContract]
    public class RainData
    {
        [DataMember(Name = "1h")]
        public double OneHour { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace UTeM_EComplaint.Tools
{
    internal class MapHandler
    {
        public static MapSpan moveToLocation(double latitude, double longitude)
        {
            Position position = new Position(latitude, longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            return mapSpan;
        }

        public static Xamarin.Forms.GoogleMaps.MapSpan moveToLocationGoogle(double latitude, double longitude)
        {
            Xamarin.Forms.GoogleMaps.Position position = new Xamarin.Forms.GoogleMaps.Position(latitude, longitude);
            Xamarin.Forms.GoogleMaps.MapSpan mapSpan = new Xamarin.Forms.GoogleMaps.MapSpan(position, 0.01, 0.01);
            return mapSpan;
        }
    }
}

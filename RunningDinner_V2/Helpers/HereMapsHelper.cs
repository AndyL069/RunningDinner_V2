using GeoCoordinatePortable;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RunningDinner.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace RunningDinner.Helpers
{
    public sealed class HereMapsHelper : IDisposable
    {
        private static HttpClient client;
        private static IConfiguration Configuration;
        private static string Url;
        public HereMapsHelper(IConfiguration configuration)
        {
            client = new HttpClient();
            Configuration = configuration;
            string apiKey = Configuration?.GetMapsSettings("apiKey");
            Url = "https://geocoder.ls.hereapi.com/6.2/geocode.json?apiKey=" + apiKey;
        }

        /// <summary>
        /// Gets the coordinates for a street address (HERE Maps Routing API)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static async Task<GeoCoordinate> GetCoordinates(string address)
        {
            string url = Url + "&searchtext=" + address;
            HttpResponseMessage response = await client.GetAsync(new Uri(url));
            string content = response.Content.ReadAsStringAsync().Result;
            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(content);
            double latitude = rootObject.Response.View[0].Result[0].Location.DisplayPosition.Latitude;
            double longitude = rootObject.Response.View[0].Result[0].Location.DisplayPosition.Longitude;
            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Gets the coordinates for a street address (HERE Maps Routing API)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string GetCoordinatesWithAddressString(string address)
        {
            string url = Url + "&searchtext=" + address;
            HttpResponseMessage response = client.GetAsync(new Uri(url)).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(content);
            double latitude = rootObject.Response.View[0].Result[0].Location.DisplayPosition.Latitude;
            double longitude = rootObject.Response.View[0].Result[0].Location.DisplayPosition.Longitude;
            string district = rootObject.Response.View[0].Result[0].Location.Address.District;
            string street = rootObject.Response.View[0].Result[0].Location.Address.Street;
            string number = rootObject.Response.View[0].Result[0].Location.Address.HouseNumber;
            return latitude.ToString(CultureInfo.InvariantCulture) + ";" + longitude.ToString(CultureInfo.InvariantCulture) + ";" + street + " " + number + ", " + district;
        }

        /// <summary>
        /// Gets the coordinates for a street address (HERE Maps Routing API)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string GetCoordinatesString(string address)
        {
            string url = Url + "&searchtext=" + address;
            HttpResponseMessage response = client.GetAsync(new Uri(url)).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(content);
            double latitude = rootObject.Response.View[0].Result[0].Location.DisplayPosition.Latitude;
            double longitude = rootObject.Response.View[0].Result[0].Location.DisplayPosition.Longitude;
            return latitude.ToString(CultureInfo.InvariantCulture) + ";" + longitude.ToString(CultureInfo.InvariantCulture);
        }

        private class NavigationPosition
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private class Location
        {
            public Address Address { get; set; }
            public NavigationPosition DisplayPosition { get; set; }
        }

        private class Address
        {
            public string District { get; set; }
            public string Street { get; set; }
            public string HouseNumber { get; set; }
        }

        private class Result
        {
            public Location Location { get; set; }
        }

        private class View
        {
            public List<Result> Result { get; set; }
        }

        private class Response
        {
            public List<View> View { get; set; }
        }

        private class RootObject
        {
            public Response Response { get; set; }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}

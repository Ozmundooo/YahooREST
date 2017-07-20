using Android.App;
using Android.Widget;
using Android.OS;
using System.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System;

namespace YahooREST
{
    [Activity(Label = "YahooREST", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button getButton;
        private EditText getEditText;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            getButton = FindViewById<Button>(Resource.Id.getButton);
            getEditText = FindViewById<EditText>(Resource.Id.cityET);

            getButton.Click += async (sender, e) =>
            {

                // Get the latitude and longitude entered by the user and create a query.
                string url = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D"
                + getEditText +
                "%2C%20"
                + ")&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

                // Fetch the weather information asynchronously, 
                // parse the results, then update the screen:
                JsonValue json = await FetchWeatherAsync(url);
                //ParseAndDisplay(json);
            };

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
         }
        private void ParseAndDisplay(JsonValue json)
        {
            TextView temperature = FindViewById<TextView>(Resource.Id.tempTextView);
            TextView cond = FindViewById<TextView>(Resource.Id.condTextView);
            JsonValue weatherResults = json["weatherObservation"];

            // The temperature is expressed in Celsius:
            double temp = weatherResults["temperature"];
        }
    }
}


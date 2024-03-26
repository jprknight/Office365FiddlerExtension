using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Office365FiddlerExtension.Services
{
    public class JsonValidatorService
    {
        internal Session session { get; set; }

        private static JsonValidatorService _instance;
        public static JsonValidatorService Instance => _instance ?? (_instance = new JsonValidatorService());

        /// <summary>
        /// Function to determine if the Json in a session response is valid Json. 
        /// If it is, we can mark a HTTP 200 response session as actually ok with more confidence.
        /// </summary>
        /// <param name="Session"></param>
        /// <returns>bool</returns>
        public bool IsValidJson(Session session)
        {
            this.session = session;

            string strInput = this.session.GetResponseBodyAsString();

            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} TRUE: {strInput}");
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} EXCEPTION PARSING JSON: {jex.Message}");
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} JSON EXCEPTION: {ex.ToString()}");
                    return false;
                }
            }
            else
            {
                //FiddlerApplication.Log.LogString($"{Assembly.GetExecutingAssembly().GetName().Name} ({this.GetType().Name}): {this.session.id} FALSE: {strInput}");
                return false;
            }
        }
    }
}

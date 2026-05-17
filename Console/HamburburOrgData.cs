using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Deez.TooMuchInfo.Console;

public static class HamburburOrgData
{
    private static JObject DataBackingField;

    public static JObject Data
    {
        get
        {
            if (DataBackingField != null)
                return DataBackingField;

            using HttpClient    httpClient   = new();
            HttpResponseMessage dataResponse = httpClient.GetAsync("https://raw.githubusercontent.com/DeezVrOfficial/Deez-s-Serverdata/refs/heads/main/data").Result;
            using Stream        dataStream   = dataResponse.Content.ReadAsStreamAsync().Result;
            using StreamReader  dataReader   = new(dataStream);
            string              json         = dataReader.ReadToEnd().Trim();
            DataBackingField = JObject.Parse(json);

            return DataBackingField;
        }
    }

    public static void ResetDataBackingField() => DataBackingField = null;
}
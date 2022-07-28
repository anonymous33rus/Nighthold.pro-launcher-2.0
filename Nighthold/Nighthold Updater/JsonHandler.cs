using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nighthold_Updater
{
    public partial class RemoteFiles
    {
        [JsonProperty("remote_url")]
        public string RemoteUrl { get; set; }

        [JsonProperty("cdn_url")]
        public string CdnUrl { get; set; }

        [JsonProperty("local_path")]
        public string LocalPath { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }
    }

    public partial class RemoteFiles
    {
        public static List<RemoteFiles> FromJson(string json) => JsonConvert.DeserializeObject<List<RemoteFiles>>(json, Converter.Settings);
    }

    public class Updater
    {
        public static async Task<string> GetRemoteFilesListJson()
        {
            return await WebTool.GetStringFromPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "nighthold_updater_list" }
            });
        }
    }

    public class DiscordClass
    {
        public static async Task SendNewIssueReport(string reportBy, string launcherVersion, string issueAt, string issueMessage)
        {
            await WebTool.SendJsonPOST(Config.AppUrl, new Dictionary<string, string>
            {
                { "type", "discord_issue_report" },
                { "by", reportBy },
                { "version", launcherVersion },
                { "at", issueAt },
                { "issue", issueMessage }
            });
        }
    }

    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    public class WebTool
    {
        public static async Task<string> GetStringFromPOST(string URL, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task SendJsonPOST(string URL, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                }
            }
            catch
            {

            }
        }
    }
}

using System;
using System.IO;
using Newtonsoft.Json;

namespace BridgeBack
{
    public class Vart
    {
        private static string json = File.ReadAllText("config.json");
        private static dynamic jsonObj = JsonConvert.DeserializeObject(json);
        public static readonly ulong DiscChannelId = jsonObj["Settings"]["Discord"]["ChannelId"];
        public static readonly string TeleChatId = jsonObj["Settings"]["Telegram"]["ChatId"];
        public static readonly string DiscToken = jsonObj["Settings"]["Discord"]["Token"];
        public static readonly string TeleToken = jsonObj["Settings"]["Telegram"]["Token"];
        public static readonly string DiscBridgeChannel = jsonObj["Settings"]["Discord"]["ChannelBridgeName"];
    }
}
/*
Should telegram bot be its own new telegram bot?
Should the telegram chat be its own chat?
*/

using System;
using Discord;
using Telegram;
using System.IO;
using System.Threading.Tasks;
using Discord.WebSocket;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BridgeBack
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();
        
        private DiscordSocketClient _client;
        private ITelegramBotClient botClient;
        async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.Log += Log;
            string DiscordToken = "Nzk0NzkyNTU4MDcyODIzODI4.X-_-QA.NyOtg7qs138274WagyVKspSLy38";
            string TelegramToken = "1540037178:AAFpOZap7GNOByt2RSk69YSG9gDRGRA6CSA";
            botClient = new TelegramBotClient(TelegramToken);
            var me = botClient.GetMeAsync().Result;
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            await _client.LoginAsync(TokenType.Bot, DiscordToken);
            await _client.StartAsync();
            // Block task until program closed
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private Task CommandHandler(SocketMessage message)
        {
            if (message.Author.IsBot)
                return Task.CompletedTask;
            if (message.Channel.ToString().Equals("bridge-to-telegram") == false)
                return Task.CompletedTask;
            string mesg = message.Content.ToString();
            botClient.SendTextMessageAsync(chatId: "-1001349057811", text: $@"{message.Author.ToString()} says: {mesg}", disableNotification: true);
            return Task.CompletedTask;
        }
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                if (e.Message.From.IsBot == false)
                {
                    try
                    {
                        string refinedMessage = e.Message.Text.Replace("#bridge", "");
                        SocketChannel channel = _client.GetChannel(794799398734266388);
                        await (channel as IMessageChannel).SendMessageAsync($@"{e.Message.From.FirstName} says: {refinedMessage}");
                    } catch (Exception easports)
                    {
                        Console.WriteLine(easports);
                    }
                }
            }
            /*if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document) // figure this out
            {
                if (e.Message.Caption != null && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    if (e.Message.Caption.Contains("#bridge") && e.Message.From.IsBot == false)
                    {
                        MemoryStream ms = new MemoryStream();
                        SocketChannel channel = _client.GetChannel(794799398734266388);
                        var iSend = channel as IMessageChannel;

                        var file = await botClient.GetFileAsync(e.Message.Document.FileId);
                        await botClient.DownloadFileAsync(file.FilePath, destination: ms);
                        await iSend.SendFileAsync(stream: ms, filename: e.Message.Document.FileName);
                    
                    }
                }
            }*/
        }
    }
}

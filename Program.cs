/*
Should telegram bot be its own new telegram bot?
Should the telegram chat be its own chat?
*/

using System;
using Discord;
using Telegram;
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
        SocketMessage broh;
        async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.Log += Log;
            string DiscordToken = "Nzk0NzkyNTU4MDcyODIzODI4.X-_-QA.NyOtg7qs138274WagyVKspSLy38";
            string TelegramToken = "1255929985:AAFZVSUX-nxjTXGb15RFA-mXt0GiPam7sWs";
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
            
            //Console.WriteLine(message.Channel.ToString());
            //Console.WriteLine(message.Content.ToString());
            string mesg = message.Content.ToString();
            botClient.SendTextMessageAsync(chatId: "-1001345308114", text: $@"{message.Author.ToString()} says: {mesg}", disableNotification: true);
            
            broh = message;
            return Task.CompletedTask;
        }
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text.Contains("#bridge") && e.Message.From.IsBot == false)
            {
                try
                {
                    string refinedMessage = e.Message.Text.Replace("#bridge", "");
                    await broh.Channel.SendMessageAsync($@"{e.Message.From.FirstName} says: {refinedMessage}");
                } catch (Exception easports)
                {
                    Console.WriteLine(easports);
                }
            }
        }
    }
}

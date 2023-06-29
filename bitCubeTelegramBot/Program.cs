using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

//link do bot t.me/bitcubetest_bot ou accesse o telegram e prorcure por BotFather a digite /newbot para criar um novo bot e pegar o token
TelegramBotClient botClient = new TelegramBotClient("6133393410:AAENbg6YWSYobDgOjqJQMXErpaqQN5He4Z4");

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
botClient.StartReceiving(
    updateHandler: UpdateHandlerAsync,
    pollingErrorHandler: PollingErrorHandlerAsync,
    receiverOptions: new ReceiverOptions()
    {
        AllowedUpdates = Array.Empty<UpdateType>()
    },
    cancellationToken: cancellationTokenSource.Token
);
async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not {} message) 
        return;
    if (message.Text is not {} messageText)
        return;
    
    long chatId = message.Chat.Id;
    string? firstName = message.From?.FirstName;
    string? lastName = message.From?.LastName;

    Console.WriteLine($"Message from {firstName} with text: {messageText}");

    if (messageText == "/start")
    {
        Message sendMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"Olá {firstName} {lastName}, tudo bem?" +
                  $"\nEu sou o bot do bitCube, um grupo de desenvolvedores de software" +
                  $"\nSe quiser saber mais sobre nós, acesse: [bitCube](https://bitcube.pt)" +
                  "\n" +
                  $"\nClique em uma das opções abaixo para que eu possa te ajudar:" +
                  "\n/bitCube   1 \\- O que é a bitCube?" +
                  "\n/rightDeal 2 \\- O que é a RightDeal?" +
                  "\n/contact   3 \\- Como posso entrar em contato com a BitCube?" +
                  "\n/contact2  3 \\- Como posso entrar em contato com a RigthDeal?" +
                  "\n/location  4 \\- Onde vocês estão localizados?",
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken
        ); 
    }
    
    switch (messageText.ToLower())
    {
        case "/bitcube":
        case "o que é a bitcube?" :
        {
            Message reply = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Somos uma empresa de software em Portugal",
                cancellationToken: cancellationToken
            ); 
            return;
        }
        case "/rightdeal":
        case "o que é a rightdeal?" :
        {
            Message reply = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Somos um site de intermediação imobiliária em Portugal",
                cancellationToken: cancellationToken
            ); 
            return;
        }
        case "/contact":
        {
            Message reply = await botClient.SendContactAsync(
                chatId: chatId,
                phoneNumber: "+351 910 000 000",
                firstName: "bitCube",
                vCard: "BEGIN:VCARD\n" +
                       "VERSION:3.0\n" +
                       "N:bitCube\n" +
                       "FN:bitCube\n" +
                       "ORG:bitCube Solutions\n" +
                       "TEL;TYPE=voice,work,pref:+351910000000\n" +
                       "EMAIL:contact@bitcube.pt\n" +
                       "END:VCARD",
                cancellationToken: cancellationToken);
            return;
        }
        case "/contact2":
        {
            Message reply = await botClient.SendContactAsync(
                chatId: chatId,
                phoneNumber: "+1234567890",
                firstName: "RightDeal",
                vCard: "BEGIN:VCARD\n" +
                       "VERSION:3.0\n" +
                       "N:Gama;Paulo\n" +
                       "ORG:Scruffy-looking nerf herder\n" +
                       "TEL;TYPE=voice,work,pref:+1234567890\n" +
                       "EMAIL:paulogama@rightdeal.pt\n" +
                       "END:VCARD",
                cancellationToken: cancellationToken);
            return;
        }
        case "/location":
        {
            Message reply = await botClient.SendVenueAsync(
                chatId: chatId,
                latitude: 41.14961f,
                longitude: -8.61099f,
                title: "Aduela",
                address: "Rua da Aduela, 4465-261 Leça do Balio",
                cancellationToken: cancellationToken
            ); 
            return;
        }
        default:
        {
            Message reply = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Sorry, I didnt understand that",
                cancellationToken: cancellationToken
            ); 
            return;
        }
    }
}

Task PollingErrorHandlerAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine($"Erro no polling: {exception.Message}");
    return Task.CompletedTask;
}


User me = await botClient.GetMeAsync(cancellationToken: cancellationTokenSource.Token);
Console.WriteLine($"Escutando as mensagens do bot {me.Username}");
Console.ReadLine();

cancellationTokenSource.Cancel();
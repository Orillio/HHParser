using HeadHunterParser.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using HeadHunterParser.StaticClasses;
using Telegram.Bot.Types.ReplyMarkups;

namespace HeadHunterParser.Telegram
{
    class TelegramClient
    {
        TelegramBotClient bot;
        public List<TelegramUser> Users;

        public TelegramClient(string token)
        {
            bot = new TelegramBotClient(token);
            Users = new List<TelegramUser>();
            bot.OnMessage += MessageListener;
            bot.OnCallbackQuery += OnButtonPress;
            bot.StartReceiving();
        }

        private async void OnButtonPress(object sender, CallbackQueryEventArgs e)
        {
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("<"),
                InlineKeyboardButton.WithCallbackData(">")
            });

            var user = Users.FindUser(e.CallbackQuery.Message.Chat.Id);
            switch (e.CallbackQuery.Data)
            {
                case ">":
                    user.MessageTuple.Page++;
                    if (user.MessageTuple.Page == user.ReturnedVacancies.Count - 1)
                        return;

                    var nextVacancy = await user.Parser.GetVacancyAsync(user.ReturnedVacancies[user.MessageTuple.Page]);
                    await bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, user.MessageTuple.MessageId, nextVacancy.ToString(), replyMarkup: markup);
                    break;
                case "<":
                    if (user.MessageTuple.Page == 0) return;
                    user.MessageTuple.Page--;

                    var prevVacancy = await user.Parser.GetVacancyAsync(user.ReturnedVacancies[user.MessageTuple.Page]);
                    await bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, user.MessageTuple.MessageId, prevVacancy.ToString(), replyMarkup: markup);
                    break;
                default:
                    break;
            }
        }

        private async void MessageListener(object sender, MessageEventArgs e)
        {
            var help = @"
Приветствую! Здесь вы можете искать вакансии по своему запросу.
Полезные команды:
/help - тут понятно)
/find <ваш_запрос> - поиск вакансий по запросу
/changetown <новый_город> - сменить поиск с текущего города на новый (город по умолчанию - Москва)
";
            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("<"),
                InlineKeyboardButton.WithCallbackData(">")
            });
            if (!Users.UserExists(e.Message.Chat.Id))
            {
                Users.Add(new TelegramUser(new HHParser(), e.Message.From.FirstName, e.Message.Chat.Id));
                await bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                var mess = await bot.SendTextMessageAsync(e.Message.Chat.Id, help);
                return;
            }
            var user = Users.FindUser(e.Message.Chat.Id);
            var newtext = e.Message.Text.Split(' ').ToList();

            switch (newtext[0])
            {
                case "/help":

                    await bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, help);
                    break;

                case "/find":

                    await bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                    newtext.RemoveAt(0);
                    var request = newtext.Count != 0
                        ? newtext.Aggregate((item1, item2) => item1 + " " + item2)
                        : null;
                    if(request == null)
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "/find <ваш_запрос>");
                        return;
                    }
                    var search = await bot.SendTextMessageAsync(e.Message.Chat.Id,"Идет поиск...");
                    var temp = user.Parser.GetJobLinksAsync(request).Result;
                    if(temp.Count() == 0)
                    {
                        await bot.EditMessageTextAsync(e.Message.Chat.Id, search.MessageId, "Вакансий не найдено((");
                        return;
                    }
                    user.ReturnedVacancies = temp.ToList();
                    var message = await bot.EditMessageTextAsync(e.Message.Chat.Id, search.MessageId,
                        user.Parser.GetVacancyAsync(user.ReturnedVacancies[0]).Result.ToString(), replyMarkup: markup);

                    if (user.MessageTuple.MessageId == default)
                    {
                        user.MessageTuple.MessageId = message.MessageId;
                        user.MessageTuple.Page = 0;
                        break;
                    }
                    else
                    {
                        await bot.DeleteMessageAsync(e.Message.Chat.Id, user.MessageTuple.MessageId);
                        user.MessageTuple.MessageId = message.MessageId;
                        user.MessageTuple.Page = 0;
                        break;
                    }

                case "/changetown":
                    newtext.RemoveAt(0);
                    var newTown = newtext.Count != 0
                        ? newtext.Aggregate((item1, item2) => item1 + " " + item2)
                        : null;
                    if (newTown == null)
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "/changetown <новый_город>");
                        return;
                    }
                    try
                    {
                        user.Parser.ChangeParseTown(newTown);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, $"Вы сменили город на {user.Parser.CuurentTownName}");
                    }
                    catch (Exception ex)
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, ex.Message);
                        return;
                    }
                    break;

                default:
                    await bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                    var mess = await bot.SendTextMessageAsync(e.Message.Chat.Id, "Введите команду. Для помощи используйте /help");
                    user.Messages.Add(mess);
                    break;
            }
        }
    }
}

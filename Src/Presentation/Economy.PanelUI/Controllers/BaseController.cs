using Azure;
using Economy.Core.Enums;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Economy.Panel.UI.Controllers
{
    public class BaseController : Controller
    {
        protected void AddMessage<T>(ResponseModel<T> response)
        {
            if (response?.Message == null || !response.Message.Messages.Any())
            {
                return; // Eğer mesajlar null veya boşsa, fonksiyon sonlanır
            }

            var messageBuilder = new StringBuilder();

            // Mesajları birleştir
            foreach (var item in response.Message.Messages)
            {
                messageBuilder.AppendLine(item);
            }

            // TempData'yı ayarla
            TempData["MessageNotification"] = messageBuilder.ToString();
            TempData["TypeNotification"] = response.Notification;

            // Mesaj başlığını notificationType'a göre ayarla
            TempData["TitleNotification"] = response.Notification switch
            {
                NotificationType.Success => "Başarılı",
                NotificationType.Information => "Bilgi",
                NotificationType.Warning => "Uyarı",
                NotificationType.Danger => "Hata",
                _ => "Bildirim"
            };
        }

        protected void AddMessage<T>(ServiceResult<T> response)
        {
            if (response == null)
                return;

            var messages = new List<string>();

            if (!string.IsNullOrWhiteSpace(response.Message))
                messages.Add(response.Message);

            if (response.Errors?.Any() == true)
                messages.AddRange(response.Errors);

            if (!messages.Any())
                return;

            var messageBuilder = new StringBuilder();
            foreach (var msg in messages)
                messageBuilder.AppendLine(msg);

            TempData["MessageNotification"] = messageBuilder.ToString().TrimEnd();
            TempData["TypeNotification"] = response.StatusCode switch
            {
                >= 200 and < 300 => response.HasData ? NotificationType.Success : NotificationType.Warning,
                >= 400 and < 500 => NotificationType.Warning,
                >= 500 => NotificationType.Danger,
                _ => NotificationType.Information
            };

            TempData["TitleNotification"] = response.StatusCode switch
            {
                >= 200 and < 300 => response.HasData ? "Başarılı" : "Uyarı",
                >= 400 and < 500 => "Uyarı",
                >= 500 => "Hata",
                _ => "Bildirim"
            };
        }

        protected void AddValidationErrorsToModelState(IReadOnlyDictionary<string, string[]>? validationErrors)
        {
            if (validationErrors == null) return;

            foreach (var (key, messages) in validationErrors)
            {
                foreach (var message in messages)
                {
                    ModelState.AddModelError(key, message);
                }
            }
        }
    }
}


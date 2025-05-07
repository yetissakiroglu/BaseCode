using Azure;
using Economy.Core.Enums;
using Economy.Core.Tools;
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
            TempData["NotificationType"] = response.Notification;

            // Mesaj başlığını notificationType'a göre ayarla
            TempData["MessageTitle"] = response.Notification switch
            {
                NotificationType.Success => "Başarılı",
                NotificationType.Information => "Bilgi",
                NotificationType.Warning => "Uyarı",
                NotificationType.Danger => "Hata",
                _ => "Bildirim"
            };
        }
    }
}


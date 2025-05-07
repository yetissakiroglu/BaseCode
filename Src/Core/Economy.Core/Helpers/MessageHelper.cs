using Economy.Core.Enums;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.RegularExpressions;

namespace Economy.Core.Helpers
{
    public static class MessageHelper
    {
        public static HtmlString Notification(this IHtmlHelper htmlHelper, NotificationType notificationType, string messageTitle, string message)
        {
            string cleanedMessage = Regex.Replace(message, "<.*?>", string.Empty) // HTML etiketlerini sil
                                 .Replace("\r", "") // \r karakterini sil
                                 .Replace("\n", "") // \n karakterini sil
                                 .Trim();
            StringBuilder sb = new StringBuilder();

            if (notificationType == NotificationType.Success)
            {
                sb.AppendFormat("showSuccessNotification('{0}','{1}');", cleanedMessage, messageTitle);
            }
            else if (notificationType == NotificationType.Warning)
            {
                sb.AppendFormat("showWarningNotification('{0}','{1}');", cleanedMessage, messageTitle);
            }
            else if (notificationType == NotificationType.Danger)
            {
                sb.AppendFormat("showErrorNotification('{0}','{1}');", cleanedMessage, messageTitle);
            }
            else if (notificationType == NotificationType.Information)
            {
                sb.AppendFormat("showInfoNotification('{0}','{1}');", cleanedMessage, messageTitle);
            }
            return new HtmlString(sb.ToString());
        }
    }

}

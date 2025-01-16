using Economy.Core.Extensions;
using Economy.Domain.Enums;
using Microsoft.AspNetCore.Html;

namespace Economy.Application.Extensions
{
    public static class htmlExtensions
    {
        public static IHtmlContent ToBadgeHtml(this ApprovalStatus status)
        {
            string badgeClass;
            string iconClass;

            switch (status)
            {
                case ApprovalStatus.Pending:
                    badgeClass = "badge-subtle-warning";
                    iconClass = "fas fa-pencil-alt";
                    break;
                case ApprovalStatus.Approved:
                    badgeClass = "badge-subtle-success";
                    iconClass = "fas fa-check";
                    break;
                case ApprovalStatus.Rejected:
                    badgeClass = "badge-subtle-danger";
                    iconClass = "fas fa-times";
                    break;
                default:
                    badgeClass = "badge-subtle-secondary";
                    iconClass = string.Empty;
                    break;
            }

            var description = status.GetDescription();

            return new HtmlString($@"
            <span class='badge rounded-pill {badgeClass}'>
                {description}
                <span class='ms-1 {iconClass}' data-fa-transform='shrink-2'></span>
            </span>");
        }

    }
}

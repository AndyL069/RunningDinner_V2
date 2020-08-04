using Microsoft.AspNetCore.Mvc;

namespace RunningDinner.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Creates link for partner confirmation after sending invitation.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="invitationId"></param>
        /// <param name="email"></param>
        /// <param name="returnUrl"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static string PartnerConfirmationLinkCallback(this IUrlHelper urlHelper, int invitationId, string email, string returnUrl, string scheme)
        {
            return urlHelper.PageLink(
                pageName: "AcceptInvitation",
                values: new { invitationId, email, returnUrl },
                protocol: scheme);
        }

        /// <summary>
        /// Creates link for partner confirmation after sending invitation.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="invitationId"></param>
        /// <param name="email"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public static string PartnerConfirmationLink(this IUrlHelper urlHelper, int invitationId, string email)
        {
            return urlHelper.Page(
                pageName: "AcceptInvitation",
                pageHandler: null,
                values: new { invitationId, email });
        }
    }
}

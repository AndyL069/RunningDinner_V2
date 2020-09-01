using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunningDinner.Extensions
{
    public static class EmailSenderExtensions
    {
        /// <summary>
        /// Removes a contact from any contact list. 
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public static async Task RemoveListRecipientAsync(this IEmailSender emailSender, string apiKey, string apiSecret, long contactId)
        {
            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Listrecipient.Resource,
                ResourceId = ResourceId.Numeric(contactId)
            };

            await client.DeleteAsync(request);
        }
        /// <summary>
        /// Adds an existing mailjet contact to a contact list
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="contactId"></param>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static async Task<long> AddContactToContactListAsync(this IEmailSender emailSender, string apiKey, string apiSecret, string contactId, string listId)
        {
            long listRecipientId = 0;
            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Listrecipient.Resource,
            }
                .Property("ContactID", contactId)
                .Property("ListID", listId);
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.GetData();
                List<ResponseClass> responseDataList = JsonConvert.DeserializeObject<List<ResponseClass>>(responseData.ToString());
                listRecipientId = responseDataList.FirstOrDefault().ID;
            }

            return listRecipientId;
        }

        /// <summary>
        /// Createse a new Mailjet contact
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="contactName"></param>
        /// <param name="contactEmail"></param>
        /// <returns></returns>
        public static async Task<int> CreateContactAsync(this IEmailSender emailSender, string apiKey, string apiSecret, string contactName, string contactEmail)
        {
            int contactId = 0;
            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Contact.Resource,
            }
                .Property(Contact.IsExcludedFromCampaigns, "true")
                .Property(Contact.Name, contactName)
                .Property(Contact.Email, contactEmail);
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.GetData();
                List<ResponseClass> responseDataList = JsonConvert.DeserializeObject<List<ResponseClass>>(responseData.ToString());
                contactId = responseDataList.FirstOrDefault().ID;
            }

            return contactId;
        }

        /// <summary>
        /// Creates a new contact list for an event.
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static async Task<int> CreateContactListAsync(this IEmailSender emailSender, string apiKey, string apiSecret, string listName)
        {
            int contactListId = 0;
            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Contactslist.Resource,
            }
            .Property(Contactslist.Name, listName);
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.GetData();
                List<ResponseClass> responseDataList = JsonConvert.DeserializeObject<List<ResponseClass>>(responseData.ToString());
                contactListId = responseDataList.FirstOrDefault().ID;
            }

            return contactListId;
        }

        /// <summary>
        /// Transactional mails from Großstadt Dinner.
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="subject"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailFromName"></param>
        /// <param name="username"></param>
        /// <param name="mailTo"></param>
        /// <param name="mailToName"></param>
        /// <param name="link"></param>
        /// <param name="username2"></param>
        /// <param name="eventName"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        public static async Task SendMailjetAsync(this IEmailSender emailSender, string apiKey, string apiSecret, int templateId, string subject, string mailFrom, string mailFromName, string username, string mailTo, string mailToName, string link, string username2 = "", string eventName = "", string eventDate = "")
        {
            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
                new JObject {
                {"From", new JObject
                    {
                        {"Email", mailFrom},
                        {"Name", mailFromName}
                    }
                },
                {"To", new JArray
                    {
                        new JObject {
                            {"Email", mailTo},
                            {"Name", mailToName}
                        }
                    }
                },
                {"TemplateID", templateId},
                {"TemplateLanguage", true},
                {"Subject", subject},
                {"Variables", new JObject
                    {
                        {"username", username},
                        {"username2", username2},
                        {"confirmation_link", link},
                        {"eventName", eventName},
                        {"eventDate", eventDate}
                    }
                },
                {"TemplateErrorReporting", new JObject
                    {
                        { "Email", "andreas.lichtsinn@gmx.de" },
                        { "Name", "Andreas Lichtsinn" }
                    }
                }}
            });

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                //logger.LogError(string.Format(CultureInfo.CurrentCulture, "Total: {0}, Count: {1}", response.GetTotal(), response.GetCount()));
                //logger.LogError(response.GetData().ToString());
                //DbManager.AddMailLog(new MailLog
                //{
                //    Date = DateTime.Today,
                //    MailTo = mailTo?.TrimEnd(','),
                //    Subject = subject
                //});
            }

            else
            {
                //logger.LogError(string.Format(CultureInfo.CurrentCulture, "StatusCode: {0}", response.StatusCode));
                //logger.LogError(string.Format(CultureInfo.CurrentCulture, "ErrorInfo: {0}", response.GetErrorInfo()));
                //logger.LogError(response.GetData().ToString());
                //logger.LogError(string.Format(CultureInfo.CurrentCulture, "ErrorMessage: {0}", response.GetErrorMessage()));
            }
        }

        /// <summary>
        /// Email via ContactForm
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="logger"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="templateId"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<bool> SendMailjetAsync(this IEmailSender emailSender, string apiKey, string apiSecret, int templateId, string name, string email, string message)
        {
            MailjetClient client = new MailjetClient(apiKey, apiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
                new JObject {
                {"From", new JObject
                    {
                        {"Email", email},
                        {"Name", name}
                    }
                },
                {"To", new JArray
                    {
                        new JObject {
                            {"Email", "andreas.lichtsinn@gmx.de"},
                            {"Name", "Andreas Lichtsinn"}
                        }
                    }
                },
                {"TemplateID", templateId},
                {"TemplateLanguage", true},
                {"Subject", "Nachricht über Kontaktformular"},
                {"Variables", new JObject
                    {
                        {"name", name},
                        {"email", email },
                        {"message", message}
                    }
                },
                {"TemplateErrorReporting", new JObject
                    {
                        { "Email", "andreas.lichtsinn@gmx.de" },
                        { "Name", "Andreas Lichtsinn" }
                    }
                }}
            });

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }

    public class ResponseClass
    {
        public int ID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using RunningDinner.Data;
using RunningDinner.Extensions;
using RunningDinner.Helpers;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Identity.Pages.Account.Manage
{
    public partial class CreateEventViewModel : PageModel
    {
        private readonly IDinnerEventsRepository _dinnerEventsRepository;
        private readonly IEmailSender _emailSender;
        private IConfiguration _configuration { get; set; }

        public CreateEventViewModel(IDinnerEventsRepository dinnerEventsRepository, IConfiguration configuration, IEmailSender emailSender)
        {
            _dinnerEventsRepository = dinnerEventsRepository;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public List<SelectListItem> Images { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Name muss ausgefüllt sein")]
            public string EventName { get; set; }
            [Required(ErrorMessage = "Datum muss ausgefüllt sein")]
            public DateTime EventDate { get; set; }
            [Required(ErrorMessage = "Anmeldeschluss muss ausgefüllt sein")]
            public DateTime LastRegisterDate { get; set; }
            [Required(ErrorMessage = "Location Adresse muss ausgefüllt sein")]
            public string PartyLocation { get; set; }
            [Required(ErrorMessage = "Location Name muss ausgefüllt sein")]
            public string PartyLocationName { get; set; }
            [Required(ErrorMessage = "Stadt muss ausgefüllt sein")]
            public string City { get; set; }
            public string EventPictureLink { get; set; }
        }

        /// <summary>
        /// Returns all event picture links from azure blob storage
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetEventPictures()
        {          
            return await StorageHelper.GetEventPictures(_configuration).ConfigureAwait(false);
        }

        public async Task<IActionResult> OnGet()
        {
            var images = await GetEventPictures().ConfigureAwait(false);
            var baseUrl = _configuration?.GetAzureStorageSettings("BaseUrl");
            var imageContainer = _configuration?.GetAzureStorageSettings("ImageContainer");
            Images = new List<SelectListItem>();
            foreach (var image in images)
            {
                Images.Add(new SelectListItem { Text = image, Value = baseUrl + imageContainer +  "/" + image });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dinnerEvent = new DinnerEvent
            {
                EventName = Input.EventName,
                EventDate = Input.EventDate,
                LastRegisterDate = Input.LastRegisterDate,
                PartyLocation = Input.PartyLocation,
                PartyLocationName = Input.PartyLocationName,
                City = Input.City,
                EventPictureLink = Input.EventPictureLink,
                RoutesOpen = false
            };

            _dinnerEventsRepository.Insert(dinnerEvent);
            _dinnerEventsRepository.SaveChanges();
            var apiKey = _configuration.GetEmailSettings("apiKey");
            var apiSecret = _configuration.GetEmailSettings("apiSecret");
            int id = await _emailSender.CreateContactListAsync(apiKey, apiSecret, Input.EventName).ConfigureAwait(false);
            if (id != 0)
            {
                dinnerEvent.ContactList = id;
                _dinnerEventsRepository.Update(dinnerEvent);
            }

            _dinnerEventsRepository.SaveChanges();
            StatusMessage = "Dein Event wurde erstellt.";
            return RedirectToPage();
        }        
    }
}



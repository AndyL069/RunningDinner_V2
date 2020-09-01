using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using RunningDinner.Data;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    [Authorize(Roles = "Administrator")]
    public class EventSettings : PageModel
    {
        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private IConfiguration Configuration { get; set; }

        public EventSettings(IDinnerEventsRepository dinnerEventsRepository,
            IConfiguration configuration)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            Configuration = configuration;
        }

        public List<SelectListItem> Images { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Event freischalten")]
            public bool EventEnabled { get; set; }

            [Display(Name = "Routen freischalten")]
            public bool RoutesOpen { get; set; }

            [Required(ErrorMessage = "Name muss ausgefüllt sein")]
            [Display(Name = "Event Name")]
            public string EventName { get; set; }

            [Required(ErrorMessage = "Location Adresse muss ausgefüllt sein")]
            [Display(Name = "Adresse der After Party Location")]
            public string PartyLocation { get; set; }

            [Required(ErrorMessage = "Location Name muss ausgefüllt sein")]
            [Display(Name = "Name der After Party Location")]
            public string PartyLocationName { get; set; }

            [Required(ErrorMessage = "Stadt muss ausgefüllt sein")]
            [Display(Name = "Event Stadt")]
            public string EventCity { get; set; }

            [Required(ErrorMessage = "Datum muss ausgefüllt sein")]
            [Display(Name = "Event Datum")]
            public DateTime EventDate { get; set; }

            [Required(ErrorMessage = "Anmeldeschluss muss ausgefüllt sein")]
            [Display(Name = "Anmeldeschluss")]
            public DateTime LastRegisterDate { get; set; }

            [Display(Name = "Event Bild")]
            public string EventPictureLink { get; set; }
        }

        /// <summary>
        /// Settings Get Method.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            var eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            var images = await GetEventPictures();
            Images = new List<SelectListItem>();
            foreach (var image in images)
            {
                Images.Add(new SelectListItem { Text = image, Value = "https://runningdinner.blob.core.windows.net/$web/" + image });
            }

            if (dinnerEvent != null)
            {
                Input = new InputModel();
                // Values are passed-through to layout page
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
                Input.RoutesOpen = dinnerEvent.RoutesOpen;
                Input.EventDate = dinnerEvent.EventDate;
                Input.EventEnabled = dinnerEvent.EventEnabled;
                Input.LastRegisterDate = dinnerEvent.LastRegisterDate;
                Input.PartyLocation = dinnerEvent.PartyLocation;
                Input.PartyLocationName = dinnerEvent.PartyLocationName;
                Input.EventPictureLink = dinnerEvent.EventPictureLink;
                Input.EventCity = dinnerEvent.City;
                Input.EventName = dinnerEvent.EventName;
            }

            return Page();
        }

        /// <summary>
        /// Settings Post Method.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            var eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent != null)
            {
                // Values are passed-through to layout page
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (dinnerEvent == null)
            {
                return Page();
            }

            dinnerEvent.EventEnabled = Input.EventEnabled;
            dinnerEvent.EventDate = Input.EventDate;
            dinnerEvent.LastRegisterDate = Input.LastRegisterDate;
            dinnerEvent.RoutesOpen = Input.RoutesOpen;
            dinnerEvent.City = Input.EventCity;
            dinnerEvent.PartyLocation = Input.PartyLocation;
            dinnerEvent.PartyLocationName = Input.PartyLocationName;
            dinnerEvent.EventPictureLink = Input.EventPictureLink;
            dinnerEvent.EventName = Input.EventName;
            DinnerEventsRepository.Update(dinnerEvent);
            DinnerEventsRepository.SaveChanges();
            TempData["StatusMessage"] = "Die Eventeinstellungen wurden gespeichert.";
            return RedirectToPage();
        }

        /// <summary>
        /// Returns all event picture links from azure blob storage
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetEventPictures()
        {
            List<string> eventPictures = new List<string>();
            // Create a BlobServiceClient object which will be used to create a container client
            var connectionString = Configuration?.GetAzureStorageSettings("connectionString");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("$web");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                if (blobItem.Name.Contains("events", StringComparison.OrdinalIgnoreCase))
                {
                    eventPictures.Add(blobItem.Name);
                }
            }

            return eventPictures;
        }
    }
}

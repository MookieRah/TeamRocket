using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseObjects;
using Webbsida.Controllers;

namespace Webbsida.ViewModels
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }

        //public virtual List<EventUser> EventUsers { get; set; }

        [StringLength(25, ErrorMessage = "Eventnamnet måste vara mellan 2 och 25 tecken"), MinLength(2, ErrorMessage = "Eventnamnet måste vara mellan 2 och 25 tecken")]
        [Required(ErrorMessage = "Eventnamn krävs")]
        [Display(Name = "Eventnamn")]
        public string Name { get; set; }

        [StringLength(600, ErrorMessage = "Beskrivningen måste vara mellan 25 och 600 tecken"), MinLength(25, ErrorMessage = "Beskrivningen måste vara mellan 25 och 600 tecken")]
        [Required(ErrorMessage = "Beskrivning krävs")]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Du måste sätta ett startdatum")]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Du måste sätta ett slutdatum")]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }
        
        [Required(ErrorMessage = "Vänligen välj vart eventet ska hållas.")]
        public float Latitude { get; set; }
        [Required(ErrorMessage = " ")]
        public float Longitude { get; set; }

        //[Range(1, int.MaxValue)]
        [Display(Name = "Max antal deltagare")]
        public int? MaxSignups { get; set; }
        //[Range(0, int.MaxValue)]
        [Display(Name = "Minsta antal deltagare")]
        public int? MinSignups { get; set; }

        [Display(Name = "Eventuell kostnad")]
        //[Range(0d, decimal.MaxValue)]
        public decimal? Price { get; set; }

        [Display(Name = "Taggar")]
        public string Tags { get; set; }

        [Required(ErrorMessage = "Vänligen välj en bild för eventet")]
        [DataType(DataType.Upload)]
        [Display(Name = "Bild")]
        public HttpPostedFileBase Image { get; set; }

        public void ValidateInput(EventController eventController)
        {
            // Kolla filändelsen
            var fileExtension = Path.GetExtension(Image.FileName).ToLower();
            if (!(fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".gif" || fileExtension == ".jpeg" || fileExtension == ".jpe" || fileExtension == ".jfif"))
                eventController.ModelState.AddModelError("Image", "Bilden måste vara någon av följande typer; .png, .jpg, .gif, .jpeg, .jpe, .jfif");
            // Max image-size = 3mb, should maybe accept null with default Image!
            if (Image.ContentLength > 3000000)
                eventController.ModelState.AddModelError("Image", "Max 3 mb!");
            // Man kan sätta negativa MaxSignups Och MinSignups
            if (MaxSignups < 1)
                eventController.ModelState.AddModelError("MaxSignups", "Max deltagare måste lämnas tom eller vara minst 1.");
            if (MinSignups < 1)
                eventController.ModelState.AddModelError("MinSignups", "Min deltagare måste lämnas tom eller vara minst 1.");
            // Man kan sätta fler MinSignups Än MaxSignups
            if (MaxSignups < MinSignups)
                eventController.ModelState.AddModelError("MaxSignups", "Max deltagare måste vara större än minsta antalet deltagare.");
            // Priset måste ha ett max-value (så det inte kraschar)
            if (Price > 1000000000)
            {
                eventController.ModelState.AddModelError("Price", "Priset är för högt!");
                Price = null;
            }
            // TODO: Disabled to make it faster to create an event
            //if (evm.StartDate > evm.EndDate)
            //{ 
            //eventController.ModelState.AddModelError("StartDate", "StartDatum måste vara tidigare än SlutDatum.");
            //}
        }

        public List<Tag> GenerateEventTags
        {
            get
            {
                if (Tags == null)
                    return new List<Tag>();


                var eventTags = Tags.Split(',');
                for (int i = 0; i < eventTags.Length; i++)
                {
                    eventTags[i] = eventTags[i].Trim().ToLower();
                }

                var result = new List<Tag>();
                foreach (var eventTag in eventTags.Distinct())
                {
                    result.Add(new Tag()
                    {
                        Name = eventTag.ToLower()
                    });
                }

                return result;
            }
        }
    }
}

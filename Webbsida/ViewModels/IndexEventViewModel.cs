using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DatabaseObjects;
using System.ComponentModel.DataAnnotations;

namespace Webbsida.ViewModels
{
    public class IndexEventViewModel
    {
        public int Id { get; set; }

        public List<EventUser> EventUsers { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Display(Name = "Max antal deltagare")]
        public int? MaxSignups { get; set; }

        [Display(Name = "Minst antal deltagare")]
        public int? MinSignups { get; set; }

        [Display(Name = "Pris")]
        public decimal? Price { get; set; }

        public int SpotsRemaining { get; set; }

        public double Distance { get; set; }

        public Profile GetOwner()
        {
            return EventUsers.Where(x => x.IsOwner == true).Select(x => x.Profile).FirstOrDefault();
        }

        public double GetOrder => SpotsRemaining + Distance + (StartDate - DateTime.Now).Days;

        public string ImagePath { get; set; }

        /// <summary>
        /// This method returns a partial description based on the description of the event.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetPartialDescription(int num)
        {
            if (Description == null)
                return "";

            int wordCount = 10;
            var partialDescription = "";

            do
            {
                partialDescription = "";

                var rawMatches = Regex.Matches(Description, GetRegExPattern(wordCount));

                if (rawMatches.Count > 0)
                {
                    partialDescription =
                        rawMatches.Cast<object>()
                            .Where(rawMatch => !string.IsNullOrWhiteSpace(rawMatch.ToString()))
                            .Aggregate(partialDescription, (current, rawMatch) => current + rawMatch.ToString() + " ");
                }
                else
                {
                    partialDescription = "Ingen beskrivning möjlig.";
                }

                wordCount--;

                var test = partialDescription.Length;
            } while (partialDescription.Length > num || wordCount < 1);


            //If there are any regex matches, they are added to the partial description.

            //Since there is a whitespace after every regex match, I remove the last white space and then add the elipses.
            return (partialDescription.Remove(partialDescription.Length - 2)) + "...";
        }

        public string GetRegExPattern(int num) => @"^(\w+\s+){1," + num + "}";
    }
}
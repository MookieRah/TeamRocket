using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DatabaseObjects;

namespace Webbsida.ViewModels
{
    public class IndexEventViewModel
    {
        public IndexEventViewModel(Event eventDbo)
        {
            this.Id = eventDbo.Id;
            this.EventUsers = eventDbo.EventUsers;
            this.Name = eventDbo.Name;
            this.Description = eventDbo.Description;
            this.StartDate = eventDbo.StartDate;
            this.EndDate = eventDbo.EndDate;
            this.Latitude = eventDbo.Latitude;
            this.Longitude = eventDbo.Longitude;
            this.MaxSignups = eventDbo.MaxSignups;
            this.MaxSignups = eventDbo.MinSignups;
            this.Price = eventDbo.Price;
            this.ImagePath = eventDbo.ImagePath;
        }

        public int Id { get; set; }

        public List<EventUser> EventUsers { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? MaxSignups { get; set; }
        public int? MinSignups { get; set; }

        public decimal? Price { get; set; }

        public Profile GetOwner()
        {
            return EventUsers.Where(x => x.IsOwner == true).Select(x => x.Profile).FirstOrDefault();
        }

        public string ImagePath { get; set; }

        /// <summary>
        /// This method returns a partial description based on the description of the event.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetPartialDescription(int num)
        {
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
                    partialDescription = "No description available.";
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
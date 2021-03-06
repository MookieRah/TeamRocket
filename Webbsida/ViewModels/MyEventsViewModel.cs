﻿using System.Collections.Generic;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.ViewModels
{
    public class MyEventsViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<IndexEventViewModel> EventsOwned { get; set; }
        public List<IndexEventViewModel> EventsBooked { get; set; }
    }
}
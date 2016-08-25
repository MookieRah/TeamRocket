using System;
using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Webbsida.Models;

namespace Tests
{
    [TestFixture]
    public class EventTests
    {
        private readonly ApplicationDbContext db;

        public EventTests()
        {
            db = new ApplicationDbContext();
        }

        [Test]
        public void GetEventByEventName()
        {
            // ARRANGE
            var expectedResult = "Name1";

            // ACT
            var result = db.Events.SingleOrDefault(n => n.Name == "Name1");

            // ASSERT
            Assert.AreEqual(expectedResult, result.Name);
        }

        [Test]
        public void AssertThatAllEventsHasAnOwner()
        {
            // ARRANGE

            // ACT
            var numberOfOwnedEvents = db.EventUsers.Count(n => n.IsOwner);
            var numberOfEventsWithOwner = db.EventUsers.Where(x => x.IsOwner).Select(x => x.Profile).Count();

            // ASSERT
            Assert.AreEqual(numberOfOwnedEvents, numberOfEventsWithOwner);
        }
    }
}

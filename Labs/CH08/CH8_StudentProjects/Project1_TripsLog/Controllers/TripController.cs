using Microsoft.AspNetCore.Mvc;
using Project1_TripsLog.Data;
using Project1_TripsLog.Models;
using Project1_TripsLog.ViewModels;

namespace Project1_TripsLog.Controllers
{
    public class TripController : Controller
    {
        private readonly TripsContext _context;

        public TripController(TripsContext context)
        {
            _context = context;
        }

        // ===================== PAGE 1: DESTINATION =====================
        [HttpGet]
        public IActionResult AddDestination()
        {
            ViewBag.SubHeader = "Enter trip destination details";
            return View();
        }

        [HttpPost]
        public IActionResult AddDestination(TripDestination model)
        {
            if (ModelState.IsValid)
            {
                TempData["Destination"] = model.Destination;
                TempData["StartDate"] = model.StartDate;
                TempData["EndDate"] = model.EndDate;
                TempData["AccommodationName"] = model.AccommodationName;

                return RedirectToAction("AddAccommodations");
            }

            ViewBag.SubHeader = "Enter trip destination details";
            return View(model);
        }

        // ===================== PAGE 2: ACCOMMODATIONS =====================
        [HttpGet]
        public IActionResult AddAccommodations()
        {
            if (TempData["Destination"] == null)
                return RedirectToAction("AddDestination");

            TempData.Keep();
            ViewBag.SubHeader = TempData.Peek("AccommodationName")?.ToString();

            return View(new TripAccommodations());
        }

        [HttpPost]
        public IActionResult AddAccommodations(TripAccommodations model)
        {
            if (ModelState.IsValid)
            {
                TempData.Keep();
                TempData["HotelPhone"] = model.AccommodationPhone;
                TempData["HotelEmail"] = model.AccommodationEmail;

                return RedirectToAction("AddActivities");
            }

            ViewBag.SubHeader = TempData.Peek("AccommodationName")?.ToString();
            return View(model);
        }

        // ===================== PAGE 3: ACTIVITIES =====================
        [HttpGet]
        public IActionResult AddActivities()
        {
            if (TempData["Destination"] == null)
                return RedirectToAction("AddDestination");

            TempData.Keep();
            ViewBag.SubHeader = TempData.Peek("Destination")?.ToString();

            return View(new TripActivities());
        }

        [HttpPost]
        public IActionResult AddActivities(TripActivities model)
        {
            if (ModelState.IsValid)
            {
                // Combine optional activities
                var activities = new List<string?> { model.Activity1, model.Activity2, model.Activity3 }
                                    .Where(a => !string.IsNullOrWhiteSpace(a))
                                    .ToList();

                var trip = new Trip
                {
                    Destination = TempData["Destination"]?.ToString() ?? "",
                    StartDate = (DateTime)(TempData["StartDate"] ?? DateTime.MinValue),
                    EndDate = (DateTime)(TempData["EndDate"] ?? DateTime.MinValue),
                    AccommodationName = TempData["AccommodationName"]?.ToString() ?? "",
                    AccommodationPhone = TempData["HotelPhone"]?.ToString(),
                    AccommodationEmail = TempData["HotelEmail"]?.ToString(),
                    Activities = string.Join("\n", activities) // store as newline-separated
                };

                _context.Trips.Add(trip);
                _context.SaveChanges();

                TempData.Clear();
                TempData["Message"] = $"Trip to {trip.Destination} added!";

                return RedirectToAction("Index", "Home");
            }

            ViewBag.SubHeader = TempData.Peek("Destination")?.ToString();
            return View(model);
        }

        // ===================== CANCEL =====================
        public IActionResult Cancel()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Redox_Code_Test
{

    // Exercise 1: LINQ Query
    public class Exercise1
    {
        public void Run()
        {
            // 1. Create a list from 1 to 100.
            var numbers = Enumerable.Range(1, 100).ToList();

            // 2. Use LINQ to find all even numbers in the list and print them.
            Console.WriteLine("Even numbers:");
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            foreach (var n in evenNumbers)
            {
                Console.Write(n + " ");
            }
            Console.WriteLine();

            // 3. Use a loop to find numbers divisible by 3 or 5 but not both.
            var filteredNumbers = new List<int>();
            foreach (var n in numbers)
            {
                bool divBy3 = n % 3 == 0;
                bool divBy5 = n % 5 == 0;
                if ((divBy3 || divBy5) && !(divBy3 && divBy5))
                {
                    filteredNumbers.Add(n);
                }
            }
            Console.WriteLine("Numbers divisible by 3 or 5 but not both:");
            Console.WriteLine(string.Join(", ", filteredNumbers));
        }
    }

    // Exercise 2: Event Scheduler
    public class Exercise2
    {
        public void Run()
        {
            EventScheduler scheduler = new EventScheduler();

            // Example events
            Event e1 = new Event("Team Sync", "Meeting Room A", new DateTime(2025, 11, 2, 14, 30, 0));
            Event e2 = new Event("Project Demo", "Main Hall", new DateTime(2025, 11, 2, 16, 0, 0));
            Event e3 = new Event("Team Sync", "Meeting Room A", new DateTime(2025, 11, 2, 14, 30, 0)); // Duplicate time and location
            Event e4 = new Event("Client Call", "Meeting Room A", new DateTime(2023, 5, 1, 10, 0, 0)); // Duplicate Location, different time
            Event e5 = new Event("Project Dragon", "Meeting Room B", new DateTime(2023, 5, 1, 10, 0, 0)); // Different Location and name but same time
            Event e6 = new Event("Future", "Old Hall", new DateTime(2026, 1, 1, 10, 0, 0)); // future event

            Console.WriteLine("Scheduling Event 1: " + (scheduler.ScheduleEvent(e1) ? "Success" : "Failed (double-booking)"));
            Console.WriteLine("Scheduling Event 2: " + (scheduler.ScheduleEvent(e2) ? "Success" : "Failed (double-booking)"));
            Console.WriteLine("Scheduling Event 3 (duplicate): " + (scheduler.ScheduleEvent(e3) ? "Success" : "Failed (double-booking)"));
            Console.WriteLine("Scheduling Event 4: " + (scheduler.ScheduleEvent(e4) ? "Success" : "Failed (double-booking)"));
            Console.WriteLine("Scheduling Event 5: " + (scheduler.ScheduleEvent(e5) ? "Success" : "Failed (double-booking)"));
            Console.WriteLine("Scheduling Event 6 (future): " + (scheduler.ScheduleEvent(e6) ? "Success" : "Failed (double-booking)"));

            Console.WriteLine("Canceling Event 2: " + (scheduler.CancelEvent(e2) ? "Canceled" : "Not found"));

            Console.WriteLine("\n All scheduled events (including past events):");
            scheduler.PrintEvents();

            Console.WriteLine("\nUpcoming Events:");
            foreach (var ev in scheduler.GetUpcomingEvents())
            {
                Console.WriteLine($"{ev.Name} at {ev.Location} on {ev.DateTime}");
            }
        }
    }

    // Event class
    public class Event
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public Event(string name, string location, DateTime dateTime)
        {
            Name = name;
            Location = location;
            DateTime = dateTime;
        }
    }

    // EventScheduler class
    public class EventScheduler
    {
        private List<Event> events = new List<Event>();
        private string storageFile = "events.json";

        public EventScheduler()
        {
            LoadEvents();
        }
        // Prevent double-booking at the same time and location
        public bool ScheduleEvent(Event e)
        {
            if (events.Any(ev => ev.DateTime == e.DateTime && ev.Location == e.Location))
            {
                return false;
            }
            events.Add(e);
            SaveEvents();
            return true;
        }
        public bool CancelEvent(Event e)
        {
            var eventToRemove = events.FirstOrDefault(ev =>
                ev.Name == e.Name && ev.Location == e.Location && ev.DateTime == e.DateTime);
            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);
                SaveEvents();
                return true;
            }
            return false;
        }
        public List<Event> GetUpcomingEvents()
        {
            var upcoming = events.Where(ev => ev.DateTime >= DateTime.Now).OrderBy(ev => ev.DateTime).ToList();
            Console.WriteLine($"{upcoming.Count} upcoming events found.");
            return upcoming;
        }
        public void PrintEvents()
        {
            foreach (var ev in events)
            {
                Console.WriteLine($"{ev.Name} at {ev.Location}, {ev.DateTime}");
            }
        }
        private void SaveEvents()
        {
            File.WriteAllText(storageFile, JsonSerializer.Serialize(events));
        }
        private void LoadEvents()
        {
            if (File.Exists(storageFile))
            {
                var json = File.ReadAllText(storageFile);
                events = JsonSerializer.Deserialize<List<Event>>(json) ?? new List<Event>();
            }
        }
    }

    // Main program runner
    class Program
    {
        static void Main(string[] args)
        {
            var exercise1 = new Exercise1();
            var exercise2 = new Exercise2();

            Console.WriteLine("\nExercise 1: LINQ Query\n");
            exercise1.Run();

            Console.WriteLine("\nExercise 2: Event Scheduler\n");
            exercise2.Run();
        }
    }
}

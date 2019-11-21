using System;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CampgroundCLI
{
    public class CampgroundCLI
    {
        //Member variables
        private string _parkName = "";
        private DateTime _arrivalDate;
        private DateTime _departureDate;
        private int _campgroundId;
        private CampgroundDAO _campgroundDAO;

        //User response possiblities
        private const string AcadiaInfo = "1";
        private const string ArchesInfo = "2";
        private const string CuyahogaValleyInfo = "3";
        private const string quitProgram = "Q";


        public CampgroundCLI(CampgroundDAO campgroundDAO)
        {
            _campgroundDAO = campgroundDAO;
        }

        /// <summary>
        ///Main display when user logs
        ///into the program
        /// </summary>
        public void RunCLI()
        {
            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                string userInput = Console.ReadLine().ToUpper();
                if (userInput == AcadiaInfo)
                {
                    Console.Clear();
                    _parkName = "Acadia";
                    DisplayParkScreen(_parkName);
                    CampgroundSubmenu();
                }
                else if (userInput == ArchesInfo)
                {
                    Console.Clear();
                    _parkName = "Arches";
                    DisplayParkScreen(_parkName);
                    CampgroundSubmenu();
                }
                else if (userInput == CuyahogaValleyInfo)
                {
                    Console.Clear();
                    _parkName = "Cuyahoga Valley";
                    DisplayParkScreen(_parkName);
                    CampgroundSubmenu();
                }
                else if (userInput == quitProgram)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid Selection, please try again...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Displays main menu
        /// </summary>
        private void PrintMenu()
        {
            Console.WriteLine("Welcome to the National Park Information Center!");
            Console.WriteLine();
            Console.WriteLine("Please select a park for further details...");
            Console.WriteLine();
            Console.WriteLine("1.Acadia");
            Console.WriteLine("2.Arches");
            Console.WriteLine("3.Cuyahoga National Valley Park");
            Console.WriteLine("Q.Quit");
        }

        /// <summary>
        /// Displays main park menu
        /// </summary>
        private void DisplayParkScreen(string parkName)
        {
            Console.WriteLine("Park Information Screen");
            Console.WriteLine();
            Console.WriteLine(ParkInfo());
        }

        /// <summary>
        /// Displays all the information of the  
        /// park chosen by the user.
        /// </summary>
        private string ParkInfo()
        {
            var parkChoice = _campgroundDAO.GetPark(_parkName);
            string parkInfo = ($"{parkChoice.Name}\n");
            parkInfo += string.Format("{0,-20}{1}\n", "Location: ", parkChoice.Location);
            parkInfo += string.Format("{0,-20}{1}\n", "Established: ", parkChoice.EstablishedDate.ToString("MM/dd/yyyy"));
            parkInfo += string.Format("{0,-20}{1}\n", "Area(KM): ", parkChoice.Area + " SQ KM");
            parkInfo += string.Format("{0,-20}{1}\n", "Annual Visitors: ", parkChoice.Visitors);
            parkInfo += string.Format("{0}{1, -40}\n", "Description: \n", DescriptionWrap(parkChoice.Description));
            return parkInfo;
        }

        /// <summary>
        /// Displays site information to reserve
        /// sites under the campground selection
        /// </summary>
        private void CampgroundSubmenu()
        {
            bool IsExit = false;
            while (!IsExit)
            {
                Console.Clear();
                DisplayParkScreen(_parkName);
                ViewCampground();
                Console.WriteLine("1. Search for available Reservation");
                Console.WriteLine("2. Return to previous screen");
                string userSelection = Console.ReadLine();
                if (userSelection == "1")
                {
                    CheckSite();
                }
                else if (userSelection == "2")
                {
                    IsExit = true;
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid Selection, please press any key to continue and try again");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
                
        /// <summary>
        /// Displays campgrounds in park
        /// </summary>
        private void ViewCampground()
        {
            Console.WriteLine("List of Campgrounds");
            Console.WriteLine();
            Console.WriteLine(CampgroundInfo());
        }

        /// <summary>
        /// Displays campgrounds of the park
        /// and displays their personal information
        /// </summary>
        public string CampgroundInfo()
        {
            string campInfo = string.Format("{0, 10}{1, 38}{2, 20}{3, 25}\n", "Name", "Open", "Close", "Daily Fee");
            var campChoice = _campgroundDAO.GetCampgrounds(_parkName);
            foreach (var campGround in campChoice)
            {
                campInfo += string.Format("{0, -5}", campGround.CampgroundID);
                campInfo += string.Format("{0,-37}", campGround.Name);
                campInfo += string.Format("{0,-19}", MonthName(campGround.OpenMonth));
                campInfo += string.Format("{0,-25}", MonthName(campGround.CloseMonth));
                campInfo += $"{Math.Round(campGround.DailyFee, 2)} \n";
            }
            return campInfo;
        }

        /// <summary>
        /// Pulls available campsites up that are not being reserved
        /// and verifies user input for site choice.
        /// </summary>
        /// <returns>Returns True if successful.</returns>
        private void CheckSite()
        {
            Console.WriteLine("Please select a campground, enter 0 to quit");           
            try
            {
                _campgroundId = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {

            }
            if (_campgroundId == 0)
            {
                Console.Clear();
            }
            else if (VerifyCampground(_campgroundId))
            {
                VerifyDates();
                var listOfSites = SiteList(_campgroundId);
                if (listOfSites.Count > 0)
                {
                    Console.Clear();
                    ReservationInfo reservationInfo = new ReservationInfo()
                    {
                        CampgroundId = _campgroundId,
                        ArrivalDate = _arrivalDate,
                        DepartureDate = _departureDate
                    };
                    MakeReservation(reservationInfo, listOfSites);
                }
                else
                {
                    Console.WriteLine("There is currently no availiable campsites for that time, please press any key to make another reservation");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Invalid campground, press any key to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }

        /// <summary>
        /// Verifies the number entered by the user
        /// is a valid campground number in the park
        /// </summary>
        /// <returns>Returns True if successful.</returns>
        public bool VerifyCampground(int userInput)
        {
            bool result = false;
            var campList = _campgroundDAO.GetCampgrounds(_parkName);
            foreach (var camp in campList)
            {
                if (userInput == camp.CampgroundID)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Verifies dates entered are in correct DateTime format.
        /// </summary>
        /// <returns>Returns True if successful.</returns>
        private void VerifyDates()
        {
            bool dateChecker = false;
            while (!dateChecker)
            {
                try
                {
                    Console.WriteLine("Please type your arrival date (mm/dd/yyyy): ");
                    _arrivalDate = Convert.ToDateTime(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid date format, please try again");
                    Console.ReadKey();
                }
                if (_arrivalDate < DateTime.UtcNow.Date)
                {
                    Console.WriteLine("Reservation cannot be booked before todays date. " +
                        "Press any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    dateChecker = true;
                }
            }
            dateChecker = false;
            while (!dateChecker)
            {
                try
                {
                    Console.WriteLine("Please type in your departure date (mm/dd/yyyy): ");
                    _departureDate = Convert.ToDateTime(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid date format, please try again");
                    Console.ReadKey();
                }
                if (_departureDate <= _arrivalDate)
                {
                    Console.WriteLine("Departure date is before arrival, please choose a date after. " +
                       "Press any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    dateChecker = true;
                }
            }
        }

        /// <summary>
        /// Creates a list of sites in the given campground
        /// </summary>
        /// <param name="campgroundId">Id of campground to pull sites from</param>
        /// <returns></returns>
        public List<Site> SiteList(int campgroundId)
        {            
            var siteList = _campgroundDAO.GetAvailableSites(campgroundId, _arrivalDate, _departureDate);           
            return siteList;
        }

        /// <summary>
        /// Allows user to input their info to make a reservation
        /// </summary>
        private void MakeReservation(ReservationInfo info, List<Site> sites)
        {
            bool makeReservation = false;
            while (!makeReservation)
            {
                Console.WriteLine(SiteInfo(sites));
                Console.WriteLine("Please enter the number of the site you wish to book, 0 to cancel");
                int siteNumber = -1;
                try
                {
                    siteNumber = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("");
                }
                if (VerifySite(siteNumber, sites))
                {

                    Console.WriteLine("Please enter your name for our reservation records...");
                    string nameEntered = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Does your reservation information appear correct?");
                    Console.WriteLine();
                    Console.WriteLine("Name: {0}| For Site# {1} | For {2} to {3}", nameEntered,
                    siteNumber, _arrivalDate.ToString("MM/dd/yyyy"), _departureDate.ToString("MM/dd/yyyy"));
                    Console.WriteLine();
                    Console.WriteLine("Enter Y to confirm, any other key to cancel");
                    string confirmReservation = Console.ReadLine().ToUpper();
                    if (confirmReservation == "Y")
                    {
                        var confirmationNum = _campgroundDAO.CreateReservation(siteNumber, nameEntered, info, sites);
                        Console.WriteLine($"Thank you! Your confirmation number is {confirmationNum}");
                        Console.ReadKey();
                        makeReservation = true;
                    }
                    else
                    {
                        Console.WriteLine("Please press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else if (siteNumber == 0)
                {
                    makeReservation = true;
                }
                else 
                {
                    Console.WriteLine("You have chosen an invalid campsite, press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                }               
            }
        }

        public string SiteInfo(List<Site> sites)
        {
            string siteInfo = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}\n", "Site ",
                "Max Occupancy ", "Accessible ", "MaxRVLength ", "Utilities ", "Cost");            
            foreach (var site in sites)
            {
                siteInfo += string.Format("{0,-20}", site.SiteNumber);
                siteInfo += string.Format("{0,-20}", site.MaxOccupancy);
                siteInfo += string.Format("{0,-20}", site.Accessible);
                siteInfo += string.Format("{0,-20}", site.MaxRVLength);
                siteInfo += string.Format("{0,-20}", site.Utilities);
                siteInfo += string.Format("{0,-20}", Math.Round(TotalCost(), 2));
            }
            return siteInfo;
        }

        /// <summary>
        /// Converts numerical value of month
        /// into name of month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private string MonthName(int month)
        {
            var monthList = new Dictionary<int, string>()
            {
                { 1, "January" },
                { 2, "February" },
                { 3, "March" },
                { 4, "April" },
                { 5, "May" },
                { 6, "June" },
                { 7, "July" },
                { 8, "August" },
                { 9, "September" },
                { 10, "October" },
                { 11, "November" },
                { 12, "December" }
            };
            return monthList[month];
        }

        /// <summary>
        /// Formats description of the park
        /// to fit the console screen
        /// </summary>
        private string DescriptionWrap(string parkDescription)
        {
            string description = "\n";
            string singleLine = "";
            int lineCounter = 0;
            int linelength = 800;
            string[] wordList = parkDescription.Split(" ");
            foreach (var word in wordList)
            {
                if (lineCounter < linelength)
                {
                    singleLine += word + " ";
                    lineCounter += singleLine.Length;
                }
                else
                {
                    description += singleLine + "\n";
                    singleLine = word + " ";
                    lineCounter = word.Length;
                }
            }
            return description;
        }

        private decimal TotalCost()
        {
            decimal dailyCost = 0;
            decimal totalDays = (decimal)(_departureDate - _arrivalDate).TotalDays;
            var listOfCampgrounds = _campgroundDAO.GetCampgrounds(_parkName);
            foreach (var campground in listOfCampgrounds)
            {
                if (campground.CampgroundID == _campgroundId)
                {
                    dailyCost = campground.DailyFee;
                }
            }
            decimal result = totalDays * dailyCost;
            return result;
        }

        public bool VerifySite(int userInput, List<Site> sites)
        {
            bool result = false;
            foreach (var site in sites)
            {
                if (userInput == site.SiteNumber)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}


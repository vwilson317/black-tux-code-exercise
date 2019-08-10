using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Inventory_Created_Correctly()
        {
            //arrange
            var inventory = new Inventory();
            var expectedInventory = 30;

            //act
            var actual = inventory.Tuxedos.Count;

            //assert
            Assert.AreEqual(expectedInventory, actual);
        }

        [Test]
        public void ParseInput_Double()
        {
            //arrange
            var inventory = new Inventory();

            //act
            var eventRequests = inventory.ParseInput(@"5,0,1,2
                                                5,0,1,2");

            //assert
            Assert.AreEqual(2, eventRequests.Count);
        }

        [Test]
        public void CheckInventory_Should_Return_True_When_Any_Count_Less_Than_10()
        {
            //arrange
            var inventory = new Inventory();
            var eventRequest = inventory.ParseInput(@"5,0,1,2
                                                5,0,1,2");
            //act
            var actual = inventory.CheckInventory(eventRequest);

            //assert
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void CheckInventory_Should_Return_False_When_Small_Count_More_Than_10()
        {
            //arrange
            var inventory = new Inventory();
            var eventRequest = inventory.ParseInput(@"5,10,1,2
                                                    5,2,1,2"); //this put the total request for day 5 at 12
            //act
            var actual = inventory.CheckInventory(eventRequest);

            //assert
            Assert.AreEqual(false, actual);
        }
    }

    //todo: rename to something better
    public class Inventory
    {
        public List<Tux> Tuxedos { get; private set; }

        public Inventory()
        {
            Tuxedos = CreateInventory();
        }

        /// <summary>
        /// assuming input is comma separated in the correct format day,small,medium,large
        /// </summary>
        /// <param name="input"></param>
        public List<EventRequest> ParseInput(string input)
        {
            var eventRequest = new List<EventRequest>();
            var values = input.Split(',');
            for (int i = 0; i < values.Length -1; i += 3)
            {
                if (i == 0)
                {
                    var largeValue = values[i + 3].Split(Environment.NewLine)[0];
                    //normally I would valid the input but I'm trusting the caller of this function :)
                    eventRequest.Add(new EventRequest
                    {
                        Day = int.Parse(values[i]),
                        DesiredSmall = int.Parse(values[i + 1]),
                        DesiredMedium = int.Parse(values[i + 2]),
                        DesiredLarge = int.Parse(largeValue)
                    });
                }
                else
                {
                    var dayValue = values[i].Split(Environment.NewLine)[1];
                    var largeValue = values[i + 3].Split(Environment.NewLine)[0];
                    //normally I would valid the input but I'm trusting the caller of this function :)
                    eventRequest.Add(new EventRequest
                    {
                        Day = int.Parse(dayValue),
                        DesiredSmall = int.Parse(values[i + 1]),
                        DesiredMedium = int.Parse(values[i + 2]),
                        DesiredLarge = int.Parse(largeValue)
                    });
                }
            }

            return eventRequest;
        }

        /// <summary>
        /// return true if all request can be met
        /// </summary>
        /// <param name="eventRequests"></param>
        /// <returns></returns>
        public bool CheckInventory(List<EventRequest> eventRequests)
        {
            var weekDict = new Dictionary<int, Inventory>();
            //todo move the population of this
            weekDict.Add(0, new Inventory());
            weekDict.Add(1, new Inventory());
            weekDict.Add(2, new Inventory());
            weekDict.Add(3, new Inventory());
            weekDict.Add(4, new Inventory());
            weekDict.Add(5, new Inventory());
            weekDict.Add(6, new Inventory());

            foreach (var currentRequest in eventRequests)
            {
                var currentInventory = weekDict[currentRequest.Day];
                try
                {
                    var dayInventory = new List<Tux>();
                    var smallInventory = currentInventory.Tuxedos.Where(x => x.Size == Size.Small).ToList();
                    smallInventory.RemoveRange(0, currentRequest.DesiredSmall);
                    var mediumInventory = currentInventory.Tuxedos.Where(x => x.Size == Size.Medium).ToList();
                        mediumInventory.RemoveRange(0, currentRequest.DesiredMedium);
                    var largeInventory = currentInventory.Tuxedos.Where(x => x.Size == Size.Large).ToList();
                    largeInventory.RemoveRange(0, currentRequest.DesiredLarge);

                    dayInventory.AddRange(smallInventory);
                    dayInventory.AddRange(mediumInventory);
                    dayInventory.AddRange(largeInventory);

                    weekDict[currentRequest.Day].Tuxedos = dayInventory;
                }
                catch (Exception _)
                {
                    return false;
                }
            }
            return true;
        }

        private List<Tux> CreateInventory()
        {
            var inventory = new List<Tux>();
            inventory.AddRange(CreateXOfSizeAndStyle(5,Size.Small,Style.A));
            inventory.AddRange(CreateXOfSizeAndStyle(5,Size.Small,Style.B));
            inventory.AddRange(CreateXOfSizeAndStyle(5,Size.Medium,Style.A));
            inventory.AddRange(CreateXOfSizeAndStyle(5,Size.Medium, Style.B));
            inventory.AddRange(CreateXOfSizeAndStyle(5,Size.Large,Style.A));
            inventory.AddRange(CreateXOfSizeAndStyle(5,Size.Large,Style.B));
            return inventory;
        }

        private  List<Tux> CreateXOfSizeAndStyle(int number, Size size, Style style)
        {
            return Enumerable.Repeat(new Tux
            {
                Size = size,
                Style = style
            }, number).ToList();
        }

    }

    public class EventRequest
    {
        //would put this in an enum but I don't want to deal with parsing at the moment
        public int Day { get; set; }
        public int DesiredSmall { get; set; }
        public int DesiredMedium { get; set; }
        public int DesiredLarge { get; set; }
    }

    public class Tux
    {
        public Size Size { get;set;}
        public Style Style { get; set; }
    }

    public enum Size
    {
        Small,
        Medium,
        Large
    }

    public enum Style
    {
        A,
        B
    }
}
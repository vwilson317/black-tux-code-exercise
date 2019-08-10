using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
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
}
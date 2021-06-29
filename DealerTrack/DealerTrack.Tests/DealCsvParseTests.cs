using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using DealerTrack.Models.Deals;
using DealerTrack.Services;
using Moq;
using Xunit;

namespace DealerTrack.Tests
{
    public class DealCsvParseTests
    {
        private readonly IDealsCsvParserService _dealsCsvParserService;
        private readonly Mock<IVehicleService> _vehicleServiceMock = new Mock<IVehicleService>();
        private readonly Mock<IDealershipService> _dealershipServiceMock = new Mock<IDealershipService>();
        private readonly Mock<ICustomerService> _customServiceMock = new Mock<ICustomerService>();

        public DealCsvParseTests()
        {
            //_vehicleServiceMock
            //    .Setup(q => q.GetEntityByName(It.IsAny<string>())).Returns(Task.FromResult(new Vehicle
            //    {
            //        Id = 1,
            //        Name = "Test"
            //    }));
            //_vehicleServiceMock
            //    .Setup(q => q.SaveAsync(new VehicleDto { Id = 1, Name = "Test" })).Returns(Task.FromResult(new VehicleDto { Id = 1, Name = "Test" }));
            
            _dealsCsvParserService = new DealsCsvParserService(_vehicleServiceMock.Object, _dealershipServiceMock.Object, _customServiceMock.Object);
        }


        [Fact]
        public async Task BaseSuccessTest()
        {
            // Arrange
            const string dealNumberString1 = "123456";
            const string customerNameString1 = "Customer Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654321";
            const string dateString1 = "6/19/2018";

            const string dealNumberString2 = "123123";
            const string customerNameString2 = "Customer Name2";
            const string dealershipNameString2 = "Dealership Name2";
            const string vehicleNameString2 = "Vehicle Name2";
            const string priceString2 = "654456";
            const string dateString2 = "6/20/2020";

            var testcsv =
                "header1, header2,header3,header4,header5,header6\n" +
                $"{dealNumberString1},{customerNameString1},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1}\n" +
                $"{dealNumberString2},{customerNameString2},{dealershipNameString2},{vehicleNameString2},{priceString2},{dateString2}\n"
             ;

            
            using (var sr = new StringReader(testcsv))
            {
                // Act
                var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;


                //Assert
                Assert.Equal(2, deals.Count);

                Assert.Equal(dealNumberString1, deals[0].DealNumber.ToString());
                Assert.Equal(customerNameString1, deals[0].CustomerName);
                Assert.Equal(dealershipNameString1, deals[0].DealershipName);
                Assert.Equal(vehicleNameString1, deals[0].Vehicle);
                Assert.Equal(priceString1, deals[0].Price.ToString(CultureInfo.InvariantCulture));

                DateTime date1;
                bool parseDateOk = DateTime.TryParse(dateString1, out date1);
                if (parseDateOk)
                {
                    Assert.Equal(date1.ToString(CultureInfo.InvariantCulture), deals[0].Date.ToString(CultureInfo.InvariantCulture));
                }
                

                Assert.Equal(dealNumberString2, deals[1].DealNumber.ToString());
                Assert.Equal(customerNameString2, deals[1].CustomerName);
                Assert.Equal(dealershipNameString2, deals[1].DealershipName);
                Assert.Equal(vehicleNameString2, deals[1].Vehicle);
                Assert.Equal(priceString2, deals[1].Price.ToString(CultureInfo.InvariantCulture));

                DateTime date2;
                bool parseDate2Ok = DateTime.TryParse(dateString2, out date2);
                if (parseDate2Ok)
                {
                    Assert.Equal(date2.ToString(CultureInfo.InvariantCulture), deals[1].Date.ToString(CultureInfo.InvariantCulture));
                }

            }
        }

        [Fact]
        public async Task ShouldParseFieldWithCommaInQuotes()
        {
            // Arrange
            const string dealNumberString1 = "123456";
            const string customerNameString1 = "Customer, Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654321";
            const string dateString1 = "6/19/2018";


            const string quote = "\"";

            var testcsv =
                    "header1, header2,header3,header4,header5,header6\n" +
                    $"{dealNumberString1},{quote}{customerNameString1}{quote},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1}\n";

            using (var sr = new StringReader(testcsv))
            {
                // Act
                var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;


                //Assert
                Assert.Equal(1, deals.Count);

                Assert.Equal(dealNumberString1, deals[0].DealNumber.ToString());
                Assert.Equal(customerNameString1, deals[0].CustomerName);
                Assert.Equal(dealershipNameString1, deals[0].DealershipName);
                Assert.Equal(vehicleNameString1, deals[0].Vehicle);
                Assert.Equal(priceString1, deals[0].Price.ToString(CultureInfo.InvariantCulture));

                DateTime date1;
                bool parseDateOk = DateTime.TryParse(dateString1, out date1);
                if (parseDateOk)
                {
                    Assert.Equal(date1.ToString(CultureInfo.InvariantCulture), deals[0].Date.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        [Fact]
        public async Task FailMissingItemTest()
        {
            // Arrange
            const string dealNumberString1 = "123K456";
            const string customerNameString1 = "Customer Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654321";
            const string dateString1 = "6/19/2018";

            var testcsv =
                    "header1, header2,header3,header4,header5,header6\n" +
                    $"{dealNumberString1},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1}\n"
                ;


            using (var sr = new StringReader(testcsv))
            {

                // Act
                try
                {
                    var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;
                }
                catch (Exception e)
                {
                    Assert.Equal($"CSV Parsing Error: Line 2 format broken. Expected 6 values but got 5", e.Message);
                }
            }
        }

        [Fact]
        public async Task FailMoreItemTest()
        {
            // Arrange
            const string dealNumberString1 = "123K456";
            const string customerNameString1 = "Customer Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654321";
            const string dateString1 = "6/19/2018";

            var testcsv =
                    "header1, header2,header3,header4,header5,header6\n" +
                    $"{dealNumberString1},{customerNameString1},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1},asd\n"
                ;


            using (var sr = new StringReader(testcsv))
            {

                // Act
                try
                {
                    var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;
                }
                catch (Exception e)
                {
                    Assert.Equal($"CSV Parsing Error: Line 2 format broken. Expected 6 values but got 7", e.Message);
                }
            }
        }

        [Fact]
        public async Task FailIncorrectDealNumberFieldTest()
        {
            // Arrange
            const string dealNumberString1 = "123K456";
            const string customerNameString1 = "Customer Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654321";
            const string dateString1 = "6/19/2018";

            var testcsv =
                "header1, header2,header3,header4,header5,header6\n" +
                $"{dealNumberString1},{customerNameString1},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1}\n"
             ;


            using (var sr = new StringReader(testcsv))
            {

                // Act
                try
                {
                    var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;
                }
                catch (Exception e)
                {
                    Assert.Equal($"Csv parsing error: DealNumber is not number in line 2", e.Message);
                }
            }
        }

        [Fact]
        public async Task FailIncorrectPriceFieldTest()
        {
            // Arrange
            const string dealNumberString1 = "123456";
            const string customerNameString1 = "Customer Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654a321";
            const string dateString1 = "6/19/2018";

            var testcsv =
                    "header1, header2,header3,header4,header5,header6\n" +
                    $"{dealNumberString1},{customerNameString1},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1}\n"
                ;


            using (var sr = new StringReader(testcsv))
            {

                // Act
                try
                {
                    var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;
                }
                catch (Exception e)
                {
                    Assert.Equal($"Csv parsing error: Price is not correct in line 2", e.Message);
                }
            }
        }

        [Fact]
        public async Task FailIncorrectDateFieldTest()
        {
            // Arrange
            const string dealNumberString1 = "123456";
            const string customerNameString1 = "Customer Name";
            const string dealershipNameString1 = "Dealership Name";
            const string vehicleNameString1 = "Vehicle Name";
            const string priceString1 = "654321";
            const string dateString1 = "40/19/2018";

            var testcsv =
                    "header1, header2,header3,header4,header5,header6\n" +
                    $"{dealNumberString1},{customerNameString1},{dealershipNameString1},{vehicleNameString1},{priceString1},{dateString1}\n"
                ;


            using (var sr = new StringReader(testcsv))
            {

                // Act
                try
                {
                    var deals = await _dealsCsvParserService.ParseRequest(sr) as List<DealCsvDto>;
                }
                catch (Exception e)
                {
                    Assert.Equal($"Csv parsing error: Date is not correct in line 2", e.Message);
                }
            }
        }

    }
}
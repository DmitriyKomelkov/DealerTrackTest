using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DealerTrack.Models.Customers;
using DealerTrack.Models.Dealerships;
using DealerTrack.Models.Deals;
using DealerTrack.Models.Vehicles;

namespace DealerTrack.Services
{
    public interface IDealsCsvParserService
    {
        Task<ICollection<DealCsvDto>> ParseRequest(TextReader requestBodyStream);
        Task<DealDto> MapToDealDto(DealCsvDto csvDto);
    }

    public class DealsCsvParserService : IDealsCsvParserService
    {
        private readonly IVehicleService _vehicleService;
        private readonly IDealershipService _dealershipService;
        private readonly ICustomerService _customerService;

        public DealsCsvParserService(IVehicleService vehicleService, IDealershipService dealershipService, ICustomerService customerService)
        {
            _vehicleService = vehicleService;
            _dealershipService = dealershipService;
            _customerService = customerService;
        }

        public async Task<ICollection<DealCsvDto>> ParseRequest(TextReader requestBodyStream)
        {
            var currentLineIndex = 1;
            var resultList = new List<DealCsvDto>();

            string currentLine;
            while ((currentLine = await requestBodyStream.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(currentLine) || currentLineIndex == 1)
                {
                    currentLineIndex++;
                    continue;
                }

                Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                var parts = csvParser.Split(currentLine);

                // clean up the fields (remove " and leading spaces)
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].TrimStart(' ', '"');
                    parts[i] = parts[i].TrimEnd('"');
                }

                if (parts.Length != 6)
                    throw new ArgumentException(
                        $"CSV Parsing Error: Line {currentLineIndex} format broken. Expected 6 values but got {parts.Length}");
                var dealNumberString = parts[0];
                var customerNameString = parts[1];
                var dealershipNameString = parts[2];
                var vehicleNameString = parts[3];
                var priceString = parts[4];
                var dateString = parts[5];

                if (string.IsNullOrWhiteSpace(dealNumberString))
                    throw new ArgumentException($"Csv parsing error: DealNumber is empty in line {currentLineIndex}");

                if (string.IsNullOrWhiteSpace(customerNameString))
                    throw new ArgumentException($"Csv parsing error: CustomerName is empty in line {currentLineIndex}");

                if (string.IsNullOrWhiteSpace(dealershipNameString))
                    throw new ArgumentException($"Csv parsing error: DealershipName is empty in line {currentLineIndex}");

                if (string.IsNullOrWhiteSpace(vehicleNameString))
                    throw new ArgumentException($"Csv parsing error: VehicleName is empty in line {currentLineIndex}");

                if (string.IsNullOrWhiteSpace(priceString))
                    throw new ArgumentException($"Csv parsing error: Price is empty in line {currentLineIndex}");

                if (string.IsNullOrWhiteSpace(dateString))
                    throw new ArgumentException($"Csv parsing error: Date is empty in line {currentLineIndex}");

                int dealNumber;
                bool parseDealNumberOk = int.TryParse(dealNumberString, out dealNumber);
                if (!parseDealNumberOk)
                    throw new ArgumentException($"Csv parsing error: DealNumber is not number in line {currentLineIndex}");

                double price;
                bool parsePriceOk = double.TryParse(priceString, out price);
                if (!parsePriceOk)
                    throw new ArgumentException($"Csv parsing error: Price is not correct in line {currentLineIndex}");

                DateTime date;
                bool parseDateOk = DateTime.TryParse(dateString, out date);
                if (!parseDateOk)
                    throw new ArgumentException($"Csv parsing error: Date is not correct in line {currentLineIndex}");


                var model = new DealCsvDto
                {
                    DealNumber = dealNumber,
                    Vehicle = vehicleNameString,
                    CustomerName = customerNameString,
                    DealershipName = dealershipNameString,
                    Price = price,
                    Date = date,
                };

                resultList.Add(model);
            }

            return resultList;
        }

        public async Task<DealDto> MapToDealDto(DealCsvDto csvDto)
        {

            var vehicle = await _vehicleService.GetEntityByName(csvDto.Vehicle);
            int? vehicleId;

            if (vehicle == null)
            {
                vehicleId = (await _vehicleService.SaveAsync(new VehicleDto { Id = null, Name = csvDto.Vehicle })).Id;
            }
            else
            {
                vehicleId = vehicle.Id;
            }

            var customer = await _customerService.GetEntityByName(csvDto.CustomerName);
            int? customerId;

            if (customer == null)
            {
                customerId = (await _customerService.SaveAsync(new CustomerDto { Id = null, Name = csvDto.Vehicle })).Id;
            }
            else
            {
                customerId = customer.Id;
            }

            var dealership = await _dealershipService.GetEntityByName(csvDto.DealershipName);
            int? dealershipId;

            if (dealership == null)
            {
                dealershipId = (await _dealershipService.SaveAsync(new DealershipDto() { Id = null, Name = csvDto.Vehicle })).Id;
            }
            else
            {
                dealershipId = dealership.Id;
            }

            if (!vehicleId.HasValue || customerId.HasValue || !dealershipId.HasValue)
            {
                new ApplicationException("Can not create some entity");
            }

            return new DealDto
            {
                Id = null,
                CustomerId = customerId.Value,
                DealershipId = dealershipId.Value,
                VehicleId = vehicleId.Value,
                Price = csvDto.Price,
                DealNumber = csvDto.DealNumber,
                Date = csvDto.Date
            };
        }
    }
}
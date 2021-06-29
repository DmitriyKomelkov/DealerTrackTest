using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealerTrack.Models.Deals;
using DealerTrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DealerTrack.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealController : ControllerBase
    {
        private readonly IDealService _dealService;
        private readonly IDealsCsvParserService _dealsCsvParserService;

        public DealController(IDealService dealService, IDealsCsvParserService dealsCsvParserService)
        {
            _dealService = dealService;
            _dealsCsvParserService = dealsCsvParserService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DealDto>))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dealService.GetAllAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadDeals()
        {
            var contentTypeHeader = Request.Headers["Content-Type"];
            if (contentTypeHeader != "text/csv")
            {
                return BadRequest("Invalid content type. Must be text/csv");
            }

            using var reader = new StreamReader(Request.Body, Encoding.UTF8);

            try
            {
                var deals = await _dealsCsvParserService.ParseRequest(reader);

                if (!deals.Any())
                    throw new ArgumentException(
                        "No data found. Check if csv file is empty or has the wrong format?");

                foreach (var dtoCsv in deals)
                {
                    var dto = await _dealsCsvParserService.MapToDealDto(dtoCsv);
                    await _dealService.SaveAsync(dto);
                }

            }
            catch (Exception e)
            {
                return BadRequest($"{e.Message}");
            }

            return NoContent();
        }
    }
}
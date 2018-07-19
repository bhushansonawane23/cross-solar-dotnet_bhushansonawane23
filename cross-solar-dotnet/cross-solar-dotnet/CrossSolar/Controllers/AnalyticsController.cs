using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossSolar.Controllers
{
    [Route("api/[controller]")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository;
        private readonly IDayAnalyticsRepository _dayAnalyticsRepository;
        private readonly IPanelRepository _panelRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository, IPanelRepository panelRepository
            ,IDayAnalyticsRepository dayAnalyticsRepository)
        {
            _dayAnalyticsRepository = dayAnalyticsRepository;
            _analyticsRepository = analyticsRepository;
            _panelRepository = panelRepository;

        }

        // GET panel/XXXX1111YYYY2222/analytics
        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get( string panelId)
        {
            var panel = await _panelRepository.Query().AsNoTracking().FirstOrDefaultAsync(x => x.Serial.Equals(panelId, StringComparison.CurrentCultureIgnoreCase));
            
            if (panel == null) return NotFound();

            var analytics = await _analyticsRepository.Query()
                .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase))
                .AsNoTracking().ToListAsync();

            var result = new OneHourElectricityListModel
            {
                OneHourElectricitys = analytics.Select(c => new OneHourElectricityModel
                {
                    Id = c.Id,
                    PanelId =c.PanelId,
                    KiloWatt = c.KiloWatt,
                    Watt = c.Watt,
                    DateTime = c.DateTime
                })
            };

            return Ok(result);
        }

        // GET panel/XXXX1111YYYY2222/analytics/day
        [Route("DayResults")]
        [HttpGet]
        public async Task<IActionResult> DayResults( string panelId)
        {
            var panel = await _panelRepository.Query().AsNoTracking().FirstOrDefaultAsync(x => x.Serial.Equals(panelId, StringComparison.CurrentCultureIgnoreCase));

            if (panel == null) return NotFound();

            var result = new List<OneDayElectricityModel>();

            var OneDayanaly = await _dayAnalyticsRepository.Query()
          .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase))
          .AsNoTracking().ToListAsync();

            foreach (var oneDay in OneDayanaly)
            {
                OneDayElectricityModel oneDayElectricityModel = new OneDayElectricityModel();
                oneDayElectricityModel.Id = oneDay.Id;
                oneDayElectricityModel.PanelId = oneDay.PanelId;
                oneDayElectricityModel.DateTime = oneDay.DateTime;
                oneDayElectricityModel.Sum = oneDay.Sum;
                oneDayElectricityModel.Average = oneDay.Average;
                oneDayElectricityModel.Minimum = oneDay.Minimum;
                oneDayElectricityModel.Maximum = oneDay.Maximum;
                result.Add(oneDayElectricityModel);
            }     
                  
            return Ok(result);
        }

        // POST panel/XXXX1111YYYY2222/analytics
        [Route("Post")]
        // [HttpPost("{panelId}/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Post( string panelId, [FromBody] OneHourElectricityModel value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var panel = await _panelRepository.Query().AsNoTracking().FirstOrDefaultAsync(x => x.Serial.Equals(panelId, StringComparison.CurrentCultureIgnoreCase));

            if (panel == null) return BadRequest("panelid not exist");


            var oneHourElectricityContent = await Task<Panel>.Run(() => new OneHourElectricity
            {
                PanelId = panelId,
                KiloWatt = value.KiloWatt,
                Watt =value.KiloWatt*1000,
                DateTime = DateTime.UtcNow
            });

            await _analyticsRepository.InsertAsync(oneHourElectricityContent);

            var result = await Task<OneHourElectricityModel>.Run(() => new OneHourElectricityModel
            {
                Id = oneHourElectricityContent.Id,
                PanelId = oneHourElectricityContent.PanelId,
                KiloWatt = oneHourElectricityContent.KiloWatt,
                Watt = oneHourElectricityContent.Watt,
                DateTime = oneHourElectricityContent.DateTime
            });



            var res = await _analyticsRepository.Query()
                .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase)
                && x.DateTime.Date == DateTime.UtcNow.Date).ToListAsync();

            
            var OneDayanaly = await _dayAnalyticsRepository.Query()
              .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase) 
              && x.DateTime== res.Select(y => y.DateTime.Date).FirstOrDefault()).AsNoTracking().ToListAsync();

            var OneDayElectricitys = await Task<OneDayElectricity>.Run(() => new OneDayElectricity()

            {
                Id = OneDayanaly.Count>0 ?  OneDayanaly.Select(y => y.Id).FirstOrDefault():0,
                PanelId = res.Select(x => x.PanelId).FirstOrDefault(),
                Sum = res.Select(x => Convert.ToDecimal(x.KiloWatt)).Sum(),
                Average = res.Select(x => Convert.ToDecimal(x.KiloWatt)).Average(),
                Minimum = res.Select(x => Convert.ToDecimal(x.KiloWatt)).Min(),
                Maximum = res.Select(x => Convert.ToDecimal(x.KiloWatt)).Max(),
                DateTime = res.Select(x => x.DateTime.Date).FirstOrDefault()
            });

            if (OneDayanaly.Count > 0)
                await _dayAnalyticsRepository.UpdateAsync(OneDayElectricitys);
            else
                await _dayAnalyticsRepository.InsertAsync(OneDayElectricitys);



            return Created($"panel/{panelId}/analytics/{result.Id}", result);
        }
    }
}
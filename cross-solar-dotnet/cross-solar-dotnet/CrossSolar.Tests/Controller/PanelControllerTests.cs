using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrossSolar.Tests.Controller
{
    public class PanelControllerTests
    {
        public PanelControllerTests()
        {
            _panelController = new PanelController(_panelRepositoryMock.Object);
            _analyticsController = new AnalyticsController(_AnalyticsRepositoryMock.Object,
                _panelRepositoryMock.Object,_DayAnalyticsRepositoryMock.Object
                );
        }

        private readonly PanelController _panelController;
        private readonly AnalyticsController _analyticsController;

        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();
        private readonly Mock<IAnalyticsRepository> _AnalyticsRepositoryMock = new Mock<IAnalyticsRepository>();
        private readonly Mock<IDayAnalyticsRepository> _DayAnalyticsRepositoryMock = new Mock<IDayAnalyticsRepository>();


        [Fact]
        public async Task Register_ShouldInsertPanel()
        {
            var panel = new PanelModel
            {
                Brand = "Areva1",
                Latitude = 12.345679,
                Longitude = 98.7655431,
                Serial = "AAAA1111BBBB3333"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);

            //var OneHourElecticity = new OneHourElectricityModel
            //{
            //    PanelId = panel.Serial,
            //    DateTime = System.DateTime.UtcNow,
            //    KiloWatt = 1
            //};

            //var resOneHourElecticity = await _analyticsController.Post(panel.Serial, OneHourElecticity);

            //// Assert
            //Assert.NotNull(resOneHourElecticity);

            //var createdResultresOneHourElecticity = result as CreatedResult;
            //Assert.NotNull(createdResultresOneHourElecticity);
            //Assert.Equal(200, createdResultresOneHourElecticity.StatusCode);

            //var OneHourElecticity1 = new OneHourElectricityModel
            //{
            //    PanelId = panel.Serial,
            //    DateTime = System.DateTime.UtcNow,
            //    KiloWatt = 1
            //};

            //var resOneHourElecticity1 = await _analyticsController.Post(panel.Serial, OneHourElecticity1);

            //// Assert
            //Assert.NotNull(resOneHourElecticity1);

            //var createdResultresOneHourElecticity1 = result as CreatedResult;
            //Assert.NotNull(createdResultresOneHourElecticity1);
            //Assert.Equal(200, createdResultresOneHourElecticity1.StatusCode);


            //var resultGet = await _analyticsController.Get(panel.Serial);
            //var createdResultGet = resultGet as CreatedResult;
            //Assert.Equal(200, createdResultGet.StatusCode);

            //var resultDayResults = await _analyticsController.DayResults(panel.Serial);
            //var createdResultDayResults = resultGet as CreatedResult;
            //Assert.Equal(200, createdResultDayResults.StatusCode);

        }
    }
}
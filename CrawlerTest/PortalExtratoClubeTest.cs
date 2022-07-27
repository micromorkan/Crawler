using Crawler.Controllers;
using Crawler.Model;
using Crawler.Model.PortalExtratoClube;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CrawlerTest
{
    public class PortalExtratoClubeTest
    {
        private readonly IConfiguration _configuration;

        public PortalExtratoClubeTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        [Fact]
        public async Task GetBeneficiosTest()
        {
            var loggerMock = new Mock<ILogger<PortalExtratoClubeController>>();

            var controller = new PortalExtratoClubeController(loggerMock.Object, _configuration);

            var result = await controller.GetBeneficios(new ParamModel { Cpf = "18966470491", Username = "RodGom21", Password = "konsi2022*" });
            
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetBeneficiosParamTest()
        {
            var loggerMock = new Mock<ILogger<PortalExtratoClubeController>>();

            var controller = new PortalExtratoClubeController(loggerMock.Object, _configuration);

            var result = await controller.GetBeneficios(new ParamModel { Cpf = "", Username = "", Password = "" });

            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Informe o Cpf, Usuario e Senha", ((ApiResult<List<string>>)result.Value).ErroMessage);
        }

        [Fact]
        public async Task GetBeneficiosNoContent()
        {
            var loggerMock = new Mock<ILogger<PortalExtratoClubeController>>();

            var controller = new PortalExtratoClubeController(loggerMock.Object, _configuration);

            var result = await controller.GetBeneficios(new ParamModel { Cpf = "00000000000", Username = "RodGom21", Password = "konsi2022*" });

            Assert.Equal(204, result.StatusCode);
        }
    }
}
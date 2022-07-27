using Crawler.Model;
using Crawler.Model.PortalExtratoClube;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Crawler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortalExtratoClubeController : ControllerBase
    {
        private readonly ILogger<PortalExtratoClubeController> _logger;
        private readonly IConfiguration _configuration;

        public PortalExtratoClubeController(ILogger<PortalExtratoClubeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<JsonResult> GetBeneficios(ParamModel parametros)
        {
            var result = new ApiResult<List<string>>();

            try
            {
                if (ParametrosInvalidos(parametros))
                {
                    result.IsSuccess = false;
                    result.ErroMessage = "Informe o Cpf, Usuario e Senha";

                    return new JsonResult(result)
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }

                var baseAddress = new Uri(_configuration.GetSection("CrawlerWebSites").GetSection("PortalExtratoClube")["UrlWebsite"]);

                var cookieContainer = new CookieContainer();

                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                {
                    using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                    {
                        AplicarHeaders(client);

                        var homePageResult = await client.GetAsync("/");

                        if (!homePageResult.IsSuccessStatusCode)
                        {
                            throw new Exception("Url indisponível");
                        }

                        var data = new Dictionary<string, string>
                        {
                            {"login",parametros.Username},
                            {"senha",parametros.Password}
                        };

                        var jsonData = JsonConvert.SerializeObject(data);
                        var content = new StringContent(jsonData, null, "application/json");

                        var loginResult = await client.PostAsync(_configuration.GetSection("CrawlerWebSites").GetSection("PortalExtratoClube")["UrlLogin"], content);
                       
                        if (loginResult.IsSuccessStatusCode)
                        {
                            var token = loginResult.Headers.GetValues("Authorization").FirstOrDefault();

                            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);

                            HttpResponseMessage response = await client.GetAsync(_configuration.GetSection("CrawlerWebSites").GetSection("PortalExtratoClube")["UrlListagem"] + parametros.Cpf);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                PortalExtratoClube model = JsonConvert.DeserializeObject<PortalExtratoClube>(responseBody);

                                if (model != null && model.beneficios.Count > 0)
                                {
                                    if (model.beneficios.Count == 1)
                                    {
                                        if (model.beneficios.First().id != 0)
                                        {
                                            result.Dados = model.beneficios.Select(x => x.nb).ToList();
                                        }
                                    }
                                    else
                                    {
                                        result.Dados = model.beneficios.Select(x => x.nb).ToList();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErroMessage = ex.Message;
                _logger.LogInformation(ex.Message);

                return new JsonResult(result)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return new JsonResult(result)
            {
                StatusCode = result.Dados == null ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.OK
            };
        }

        private bool ParametrosInvalidos(ParamModel parametros)
        {
            if (string.IsNullOrWhiteSpace(parametros.Cpf) ||
                string.IsNullOrWhiteSpace(parametros.Username) ||
                string.IsNullOrWhiteSpace(parametros.Password))
            {
                return true;
            }

            return false;
        }

        private static void AplicarHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "http://ionic-application.s3-website-sa-east-1.amazonaws.com");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "http://ionic-application.s3-website-sa-east-1.amazonaws.com/");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");
        }
    }
}
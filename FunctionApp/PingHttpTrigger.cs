using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;

namespace OCAProject
{
    public class PingHttpTrigger
    {
        private readonly IMyService _service;
        public PingHttpTrigger(IMyService service)
        { // 인터페이스를 받아오지만 외부 인스턴스를 끌고 들어오는 것. 걍 외부에서 꽂아주기,,

            this._service = service ?? throw new ArgumentNullException(nameof(service)); // 주입이 되어있지 않으면 에러 넘기기
        }

        [FunctionName(nameof(PingHttpTrigger))]

        [OpenApiOperation(operationId: "Ping", tags: new[] { "greeting" })] // 얘가 들어있어야 openapi에 등록이 된다.
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        // query로 할 거면 name 을 x-functions-key에서 code로 바꾸면된다.
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Description = "Name of person to greet.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ResponseMessage))]
        //일케 네줄 넣으면 openapi로 문서가 생김,, 사실 맨첫줄만넣어도되긴됨...
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ping/{name2}")] HttpRequest req, string name2,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var result = this._service.GetMessage(name);

            var res = new ResponseMessage() { Message = result };

            return new OkObjectResult(res);
        } // 컨테이너의 역할만 (1. 유효성 검사, 2. 비즈니ㅣ스 로직 검사)
    }

    public class ResponseMessage // 문자열을 바로 보내는 대신 json 형태로 보낸다.
    {
        [JsonProperty(
            "response_message"
        )] // json개체 모양 바꾸기
        public string Message { get; set; }

    }
}

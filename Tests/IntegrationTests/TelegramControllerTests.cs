using System.Net;
using System.Net.Http.Headers;
using Tests.IntegrationTests.Utils;
using Xunit;

namespace Tests.IntegrationTests;

public class TelegramControllerTests
{
    private readonly ApplicationFixture _application;

    public TelegramControllerTests()
    {
        _application = new ApplicationFixture();
    }

    [Fact]
    public async Task Get_ShouldReturnListResult()
    {
        // Act
        var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "api/TelegramBot");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer",
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiI5MDJkZmE2Ni00ZDU2LTQxYWYtYmY1Ni0zNDcxMTRhZmYwMzciLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vNjkyNjg4YmMtMzY4OS00YjViLTg4NWItM2Y2YmI3ODAzOGRjL3YyLjAiLCJpYXQiOjE2ODU2ODk4OTksIm5iZiI6MTY4NTY4OTg5OSwiZXhwIjoxNjg1Njk0NzA5LCJhaW8iOiJBYVFBVy84VEFBQUFSS21RL0ZQcHlFajVHU3dTcVJVdnBRZmRiTFZyWjFoWnV3QjNQaXdsR0F5SjhIV3o5ZS9nNmF0L2FrenYyQkxNYmRyNDI2TlV1aEthb1AwdE9QcHhhU1h5dFljcFk3MVRoWms4VjVrME1YQ1JCSFU2ZGthWk9QQlJXLyt3Z2VpWmhoYkNGREk0ZkhMdXVvTmI1eWpnYjVTMURsdm8rdXFnMzdydmt0RExmTHFSNVFmVS9pbkxkOHNjN09rbkJLcUdSK2trS3FuWmhUQWpDMFlEdGxiL1VnPT0iLCJhenAiOiI5MDJkZmE2Ni00ZDU2LTQxYWYtYmY1Ni0zNDcxMTRhZmYwMzciLCJhenBhY3IiOiIwIiwiaWRwIjoibGl2ZS5jb20iLCJuYW1lIjoiMTZkZWNlZDMtZDg1Mi00OTBiLTlkNGItZWRhODA2MTYwZDNhIDI3MTBhYjU3LTk5ZjgtNDYxZS05MTJjLWNmOTE1NWRlODM5YyIsIm9pZCI6ImY2M2Y5ZDVmLThlMTktNDc3Ni05MjdlLTU5ZGUyNGM3MWNkOCIsInByZWZlcnJlZF91c2VybmFtZSI6InRwbzloLmNhd2FAZ21haWwuY29tIiwicmgiOiIwLkFVNEF2SWdtYVlrMlcwdUlXejlydDRBNDNHYjZMWkJXVGE5QnYxWTBjUlN2OERlREFMRS4iLCJyb2xlcyI6WyJPd25lciJdLCJzY3AiOiJ1c2Vycy5hcGkuc2NvcGUiLCJzdWIiOiJwZDkzTmExQUJNVVJNV1FEc3Y5ZVljeWpnR05nbkRwYUJGRXRWcEdaTTE0IiwidGlkIjoiNjkyNjg4YmMtMzY4OS00YjViLTg4NWItM2Y2YmI3ODAzOGRjIiwidXRpIjoidllhUzEzSldTRWlUNzVRN3Q2ZnRBQSIsInZlciI6IjIuMCJ9.kleAKjAGGuCpmkBGiHbLlNqkbqnlxKfP8yx1cS-xLCpEXcSUZ6YU5IqR3rQ32W4e6-4CLqeQYhRzdCE81aSZdnUxbHADRGh4ivn90WbyZ-A1tnmOeYt-JsAEI6T2vU92vzqq45CRxhldzIir_UTuZKBh28cTIEnS5pfDcl6yeg7xST9xjyVzxBBIJnj6YF-ygcX1jvJ2oUbR81UQOtuUX36h0ROzYqKDN4NFvDaPlomz-j7zT1ru0ZcW9IYukl2XXjgLQSknepvi2NnLFaiPAGmet3hDLV5FEpNUd23AuT8kt1XelBo8qFiCSlRzBufm8eHQ2Ln5aTP5MHzjjmsxCg");
        var response = await _application.Client.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        //var models =

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
using Newtonsoft.Json;

namespace Utils;

public static class ExceptionExtensions
{
    public static string GetErrorMessageJson(this Exception ex)
    {
        return JsonConvert.SerializeObject(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
    }
}
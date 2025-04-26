using System.Web;

namespace Jenbot.MathsModule.WolframAlphaApi;

public class WolframApi
{
    private const string ENDPOINT = "http://api.wolframalpha.com/v1/simple";
    private const string LAYOUT_PARAM = "labelbar";
    private const int FONTSIZE_PARAM = 16;
    private const string UNITS_PARAM = "metric";

    private string _appId;
    private readonly HttpClient _client;

    public WolframApi(string appId)
    {
        _appId = appId;
        _client = new HttpClient(new HttpClientHandler()
            {
                UseProxy = false,
                Proxy = null
            }
        );
    }

    public async Task<IEnumerable<byte>> AskQuestionAsync(string question)
    {
        var url = CreateRequestUrl(question);
        return await _client.GetByteArrayAsync(url);
    }

    private string CreateRequestUrl(string question)
    {
        var urlParams = HttpUtility.ParseQueryString(string.Empty);
        urlParams["appid"] = _appId;
        urlParams["i"] = question;
        urlParams["layout"] = LAYOUT_PARAM;
        urlParams["fontsize"] = FONTSIZE_PARAM.ToString();
        urlParams["units"] = UNITS_PARAM;

        return $"{ENDPOINT}?{urlParams}";
    }
}

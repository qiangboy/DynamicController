using System.Runtime.CompilerServices;

namespace WebApi;

public static class StringExtensions
{
    public static async Task<string> GetAsync(this string remoteUrl)
    {
        var resp = await remoteUrl;

        return await resp.Content.ReadAsStringAsync();
    }

    public static TaskAwaiter<HttpResponseMessage> GetAwaiter(this string remoteUrl)
    {
        HttpClient client = new HttpClient();
        return client.GetAsync(remoteUrl).GetAwaiter();
    }
}
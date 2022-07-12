public class MyService : IMyService
{
    public string GetMessage(string name)
    {
        string responseMessage = string.IsNullOrEmpty(name)
    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
    : $"Hello, {name}. This HTTP triggered function executed successfully.";

        return responseMessage;
    }
}
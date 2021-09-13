
namespace EtronRtuServer;

public class PubService
{
    private readonly JsonLogger jsonLogger;

    public PubService(JsonLogger jsonLogger)
    {
        this.jsonLogger = jsonLogger;
    }


    public void PublishMessage(string purpose, object message)
    {

        // 보냈다 침.
        jsonLogger.Write("publish message", new
        {
            purpose,
            message
        });
        
    }
}
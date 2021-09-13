
namespace EtronRtuServer;

public class JsonLogger
{
    public void Write(string subject, object message)
    {
        var now = DateTime.Now;

        var jo1 = JObject.FromObject(new
        {
            subject,
            time = now.ToString("yyyy-MM-ddTHH:mm:ss.ffffff")
        });
        var jo2 = JObject.FromObject(message);

        jo1.Merge(jo2);

        Console.WriteLine(jo1.ToString());
    }
}


public class JsonSessionLogger
{
    private readonly Session session;

    public JsonSessionLogger(Session session)
    {
        this.session = session;
    }

    public void Write(string subject, object message)
    {
        var now = DateTime.Now;

        var jo1 = JObject.FromObject(new
        {
            subject,
            time = now.ToString("yyyy-MM-ddTHH:mm:ss.ffffff"),
            imei = session.Imei
        });
        var jo2 = JObject.FromObject(message);

        jo1.Merge(jo2);

        Console.WriteLine(jo1.ToString());
    }
}


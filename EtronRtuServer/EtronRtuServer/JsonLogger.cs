
namespace EtronRtuServer;

public class JsonLogger
{
    public void Write(string subject, object message)
    {
        var now = DateTime.Now;

        var jo1 = JObject.FromObject( new
        {
            subject,
            time = now.ToString("yyyy - MM - ddTHH:mm: ss.ffffff")
        });
        var jo2 = JObject.FromObject(message);

        jo1.Merge(jo2);

        Console.WriteLine(jo1.ToString());
    }
}


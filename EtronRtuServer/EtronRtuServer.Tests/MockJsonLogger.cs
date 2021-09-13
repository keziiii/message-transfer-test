
using Newtonsoft.Json.Linq;

namespace EtronRtuServer.Tests;

public class MockJsonLogger : JsonLogger
{
    public List<JObject> Logs = new List<JObject>();
    public override void Write(string subject, object message)
    {
        var jo1 = this.CreateMessage(subject, message);
        this.Logs.Add(jo1);
    }
}


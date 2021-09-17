using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtronRtuServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionsController : ControllerBase
    {
        private readonly RtuSocketServer socketServer;

        public ConnectionsController(RtuSocketServer socketServer)
        {
            this.socketServer = socketServer;
        }

        [HttpGet]
        public dynamic Get() => socketServer.Sessions.Values.Select(session => new
        {
            sessionId = session.SessionId,
            imei = session.Imei,
            connectedTime = session.ConnectedTime,
        });
    }
}

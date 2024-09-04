using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer
{
    internal class Server : DataServerInterface
    {
        private readonly Database _db = Database.Instance;
    }
}

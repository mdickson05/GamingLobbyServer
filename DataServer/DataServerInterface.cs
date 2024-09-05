using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataServer
{
    [ServiceContract]
    public interface DataServerInterface
    {

        [OperationContract]
        void GetValuesForUser(out string username, out string password);
    }
}

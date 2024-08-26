using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataServer
{
    [ServiceContract]
    internal interface GLSInterface
    {
        // TODO: Implement server interface
        // OperationContract tag to define as function in the contract
        [OperationContract]
        // NOT A REAL FUNCTION - REMOVE
        int foo();
    }
}

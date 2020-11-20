using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NSAPConnector.Extension
{

    public static class SapCommandExtension
    {

        public static RfcDestination GetConnfectionXXX(this SapCommand cmd)
        {
            return cmd.Connection.Destination;
        }

        public static IRfcFunction CreateFunction(this SapConnection conn, string name)
        {
            return conn.Destination.Repository.CreateFunction(name);
        }

        public static void CreateFunctionBAPIRfc(this SapConnection connection,

            ref IRfcStructure[] Structures, ref IRfcTable[] Tables)
        {


            var apAPI = connection.CreateFunction("BAPI_GOODSMVT_CREATE");
            var apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

            Structures[0] = apAPI.GetStructure("GOODSMVT_HEADER");
            Structures[1] = apAPI.GetStructure("GOODSMVT_CODE");

            Tables[0] = apAPI.GetTable("GOODSMVT_ITEM");

        }
    }
}
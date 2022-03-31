using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Controllers
{
    public class DBConfig
    {
        string connectionString = "data source=DESKTOP-TS7B0VF;initial catalog=EBS_DB;integrated security=True";
        public string getConnectionString() {
            return connectionString;
        }
    }
}
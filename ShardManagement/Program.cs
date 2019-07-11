using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShardManagement
{
    class Program
    {
        //Very bad, don't ever do this...

        private static string _shardServer = "orion\\sql2k17";
        private static string _shardMapManagerDb = "BlogShardManager";
        private static string _shardDb1 = "BlogShard1";
        private static string _shardDb2 = "BlogShard2";
        private static string _userName = "ONETUG_DEMO_USER";
        private static string _pasword = "do_not_do_this$";
        private static string _applicationName = "ONETUG_DEMO";

        private static int _firstTenantId = 14;
        private static int _secondTenantId = 29;
        private static int _thirdTenantId = 47;
        private static int _fourthTenantId = 65;

        static void Main(string[] args) {

            var connStrBldr = new SqlConnectionStringBuilder {
                UserID = _userName,
                Password = _pasword,
                IntegratedSecurity = false,
                ApplicationName = _applicationName
            };


            // Bootstrap the shard map manager, register shards, and store mappings of tenants to shards
            // Note that you can keep working with existing shard maps. There is no need to 
            // re-create and populate the shard map from scratch every time.
            Console.WriteLine("Checking for existing shard map and creating new shard map if necessary.");

            var sharding = new Sharding(_shardServer, _shardMapManagerDb, connStrBldr.ConnectionString);


            //Shard 1 will contain tenants 14, 47
            Console.WriteLine($"Registering first Shard {_shardDb1} with tenant:{_firstTenantId.ToString()}");
            sharding.RegisterNewShard(_shardServer, _shardDb1, connStrBldr.ConnectionString, _firstTenantId);

            Console.WriteLine($"Registering first Shard {_shardDb1} with tenant:{_thirdTenantId.ToString()}");
            sharding.RegisterNewShard(_shardServer, _shardDb1, connStrBldr.ConnectionString, _thirdTenantId);

            //Shard 2 will contain tenants 29, 65
            Console.WriteLine($"Registering second Shard {_shardDb2} with tenant:{_secondTenantId.ToString()}");
            sharding.RegisterNewShard(_shardServer, _shardDb2, connStrBldr.ConnectionString, _secondTenantId);

            Console.WriteLine($"Registering second Shard {_shardDb2} with tenant:{_fourthTenantId.ToString()}");
            sharding.RegisterNewShard(_shardServer, _shardDb2, connStrBldr.ConnectionString, _fourthTenantId);


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

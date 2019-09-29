﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.SqlDatabase.ElasticScale.Query;

namespace MsqDemo {
    class Program {


        private static string _shardServer = "YourSQLServerInstanceHere";
        private static string _shardMapManagerDb = "BlogShardManager";

        private static string _userName = "YourDBUserHere";
        private static string _pasword = "YourPasswordHere";
        private static string _applicationName = "ONETUG_DEMO";

        private static int _firstTenantId = 1;
        private static int _secondTenantId = 3;
        private static int _thirdTenantId = 2;
        private static int _fourthTenantId = 4;

        static void Main(string[] args) {

            var connStrBldr = new SqlConnectionStringBuilder {
                UserID = _userName,
                Password = _pasword,
                IntegratedSecurity = false,
                ApplicationName = _applicationName
            };

            //Get an instance of our ShardMapManager (contained in ShardingClient wrapper)
            Console.WriteLine("Checking for existing shard map and creating new shard map if necessary.");
            var sharding = new ShardingClient(_shardServer, _shardMapManagerDb, connStrBldr.ConnectionString);

            Console.WriteLine("Executing Multishard Query");
            var results = ExecuteMultiShardQuery(ref sharding, connStrBldr.ConnectionString);

            Console.WriteLine("Multishard Query Results *******************************");
            if (results.Count > 0) {
                foreach (var item in results) {
                    Console.WriteLine($"BlogId:{item.BlogId}\tBlogName:{item.Name}");
                }
            }



            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static List<dynamic> ExecuteMultiShardQuery(ref ShardingClient sharding, string connectionString) {

            dynamic result = new List<dynamic>();

            var multiShardConnection = new MultiShardConnection(sharding.ShardMap.GetShards(), connectionString);

            using (var cmd = multiShardConnection.CreateCommand()) {
                cmd.CommandText = @"SELECT * FROM dbo.Blogs";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandTimeoutPerShard = 60;

                using (var reader = cmd.ExecuteReader()) {
                    if (reader.HasRows) {
                        result = reader.ToExpandoList();

                    }
                }
            }

            return result;
        }
    }
}

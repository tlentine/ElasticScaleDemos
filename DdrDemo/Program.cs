using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DdrDemo {
    class Program {

        //Very bad, don't ever do this...

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


            // Create and save a new Blog 
            Console.Write($"Enter a name for a new Blog for Tenant:{_firstTenantId.ToString()} ");
            var name = Console.ReadLine();

            SqlDatabaseUtils.SqlRetryPolicy.ExecuteAction(() => {
                using (var db = new ElasticScaleContext<int>(sharding.ShardMap, _firstTenantId, connStrBldr.ConnectionString)) {
                    var blog = new Blog { Name = name, BlogId = _firstTenantId };
                    db.Blogs.Add(blog);
                    db.SaveChanges();
                }
            });

            //Retrieve Blogs for Tenant 14

            SqlDatabaseUtils.SqlRetryPolicy.ExecuteAction(() => {
                using (var db = new ElasticScaleContext<int>(sharding.ShardMap, _firstTenantId, connStrBldr.ConnectionString)) {
                    // Display all Blogs for tenant 1
                    var query = from b in db.Blogs
                                orderby b.Name
                                select b;

                    Console.WriteLine($"All blogs for tenant id: {_firstTenantId.ToString()}");
                    foreach (var item in query) {
                        Console.WriteLine(item.Name);
                    }
                }
            });

            // Create and save a new Blog 
            Console.Write($"Enter a name for a new Blog for Tenant:{_secondTenantId.ToString()} ");
            var name2 = Console.ReadLine();

            SqlDatabaseUtils.SqlRetryPolicy.ExecuteAction(() => {
                using (var db = new ElasticScaleContext<int>(sharding.ShardMap, _secondTenantId, connStrBldr.ConnectionString)) {
                    var blog = new Blog { Name = name2, BlogId = _secondTenantId };
                    db.Blogs.Add(blog);
                    db.SaveChanges();
                }
            });

            SqlDatabaseUtils.SqlRetryPolicy.ExecuteAction(() => {
                using (var db = new ElasticScaleContext<int>(sharding.ShardMap, _secondTenantId, connStrBldr.ConnectionString)) {
                    // Display all Blogs from the database 
                    var query = from b in db.Blogs
                                orderby b.Name
                                select b;

                    Console.WriteLine($"All blogs for tenant id: {_secondTenantId.ToString()}");
                    foreach (var item in query) {
                        Console.WriteLine(item.Name);
                    }
                }
            });

            //// Create and save a new Blog
            //Console.Write($"Enter a name for a new Blog for Tenant:{_thirdTenantId.ToString()} ");
            //var name3 = Console.ReadLine();

            //SqlDatabaseUtils.SqlRetryPolicy.ExecuteAction(() => {
            //    using (var db = new ElasticScaleContext<int>(sharding.ShardMap, _thirdTenantId, connStrBldr.ConnectionString)) {
            //        var blog = new Blog { Name = name3, BlogId = _thirdTenantId };
            //        db.Blogs.Add(blog);
            //        db.SaveChanges();
            //    }
            //});

            //SqlDatabaseUtils.SqlRetryPolicy.ExecuteAction(() => {
            //    using (var db = new ElasticScaleContext<int>(sharding.ShardMap, _thirdTenantId, connStrBldr.ConnectionString)) {
            //        // Display all Blogs from the database 
            //        var query = from b in db.Blogs
            //                    orderby b.Name
            //                    select b;

            //        Console.WriteLine($"All blogs for tenant id: {_thirdTenantId.ToString()}");
            //        foreach (var item in query) {
            //            Console.WriteLine(item.Name);
            //        }
            //    }
            //});

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }
    }
}

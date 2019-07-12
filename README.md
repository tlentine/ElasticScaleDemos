# ElasticScaleDemos

Samples from my talk at ONETUG about Building Scalable Cloud Databases

## Getting Started

1. Publish a copy of the ShardManager database to SQL Server. I'd suggest naming the database BlogShardManager as this is the name defined in the samples. You can of course name the database something else, just be sure to update `_shardMapManagerDb` in the samples with the name you've chosen.

2. Publish two copies of the BlogDb. For this, I'd suggest naming the databases `BlogShard1` and `BlogShard2`. If you use something different, update `_shardDb1` and `_shardDb2` in the ShardManagement project.

3. Update the `_shardServer`, `_userName` and `_pasword` member variables across the apps so we can connect to your instance of SQL Server. In a real production scenario you'd be reading this from keyVault or environment variables or a config file, but for the demos we're using worst-practices : )

4. Run the ShardManagement application. This will populate the ShardManager database with the necessary schema objects for Elastic Client Library to work, and will register your two Shards (databases from step 2 above)

5. With the ShardManagement application run, we're now able to create an instance of ShardMapManager and utilize it to perform data dependent routing and multi-shard queries. 
    * Start with the DdrDemo, and execute it to add some records to the various shards (in the form of Blogs)
    * Observe how, based on our ShardingKey, records are added to either `BlogShard1` or `BlogShard2` without the application having to worry about managing direct connections to those databases. This is the *magic* of Elastic Scale Client Libraries.

6. The final demo is to run the MsqDemo to perform a multi-shard query to return all blogs that are in our ShardSet (group of shards\databases);

﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MsqDemo {
    public class ShardingClient {
        public ShardMapManager ShardMapManager { get; private set; }
        public ListShardMap<int> ShardMap { get; private set; }


        // Bootstrap Elastic Scale by creating a new shard map manager and a shard map on 
        // the shard map manager database if necessary.
        public ShardingClient(string shardMapManagerServer, string shardMapManagerDb, string shardMapManagerConnString, string shardMapName = "ONETUG_DEMO") {
            // Connection string with administrative credentials for the root database
            var connStrBldr = new SqlConnectionStringBuilder(shardMapManagerConnString);
            connStrBldr.DataSource = shardMapManagerServer;
            connStrBldr.InitialCatalog = shardMapManagerDb;

            // Deploy shard map manager.
            if (!ShardMapManagerFactory.TryGetSqlShardMapManager(connStrBldr.ConnectionString, ShardMapManagerLoadPolicy.Lazy, out var existingShardMapManager)) {
                ShardMapManager = ShardMapManagerFactory.CreateSqlShardMapManager(connStrBldr.ConnectionString);
            } else {
                ShardMapManager = existingShardMapManager;
            }

            if (!ShardMapManager.TryGetListShardMap<int>(shardMapName, out var existingListShardMap)) {
                ShardMap = ShardMapManager.CreateListShardMap<int>(shardMapName);
            } else {
                ShardMap = existingListShardMap;
            }
        }
    }
}

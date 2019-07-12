using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See MicrosoftLICENSE file in the project root for full license information.

namespace DdrDemo {
    internal static class SqlDatabaseUtils {
        /// <summary>
        /// Gets the retry policy to use for connections to SQL Server.
        /// </summary>
        public static RetryPolicy SqlRetryPolicy {
            get { return new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(10, TimeSpan.FromSeconds(5)); }
        }
    }
}

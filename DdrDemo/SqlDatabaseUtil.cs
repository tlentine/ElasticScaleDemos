using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;


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

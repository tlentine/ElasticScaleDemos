using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Copyright (c) Tim Lentine. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MsqDemo {
    internal static class SqlExtensions {
        public static List<dynamic> ToExpandoList(this IDataReader reader) {
            var result = new List<dynamic>();
            while (reader.Read()) {
                result.Add(reader.RecordToExpando());
            }
            return result;
        }
        public static dynamic RecordToExpando(this IDataReader reader) {

            dynamic e = new ExpandoObject();

            var d = (IDictionary<string, object>)e;
            var values = new object[reader.FieldCount];

            reader.GetValues(values);
            for (var i = 0; i < values.Length; i++) {
                var v = values[i];
                d.Add(reader.GetName(i), DBNull.Value.Equals(v) ? null : v);
            }

            return e;

        }
    }


}

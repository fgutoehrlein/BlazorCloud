using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCloudCore.Logic.SQLite
{
    public interface ISqliteSetup: IDisposable
    {
        /// <summary>
        /// Checks if there is a database of the given name. If not it will create one.
        /// </summary>
        /// <param name="databaseName"></param>
        public void CreateDatabaseIfNonExistant(string databaseName);
        /// <summary>
        /// Extracts the name of the database from its connection string using the following regex: (?<= Filename=)(.*?)(?=.sqlite)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>
        /// Returns the first match of the pattern as string
        /// </returns>
        public string GetDatabaseNameFromConnectionString(string connectionString);
    }
}

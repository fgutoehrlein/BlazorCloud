using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;

namespace BlazorCloudCore.Logic.SQLite
{
    public class SqliteSetup : ISqliteSetup
    {
        private bool isDisposed;
        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            isDisposed = true;
        }
        /// <summary>
        /// Checks if there is a database of the given name. If not it will create one.
        /// </summary>
        /// <param name="databaseName"></param>
        public void CreateDatabaseIfNonExistant(string databaseName)
        {
            if (File.Exists(databaseName + ".db"))
            {
                return;
            }
            else
            {
                SQLiteConnection.CreateFile(databaseName + ".db");
            }
        }
        /// <summary>
        /// Extracts the name of the database from its connection string using the following regex: (?<= Filename=)(.*?)(?=.sqlite)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>
        /// Returns the first match of the pattern as string
        /// </returns>
        public string GetDatabaseNameFromConnectionString(string connectionString)
        {
            if (connectionString.Contains(".sqlite"))
            {
                Regex pattern = new Regex("(?<= Data Source=)(.*?)(?=.db)");

                if (pattern.IsMatch(connectionString))
                {
                    return pattern.Match(connectionString).Value;
                }
            }
            return null;
        }
    }
}

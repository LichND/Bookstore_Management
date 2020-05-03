using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using Dapper;


namespace BookStore_Management
{
    public static class SQLiteDataAccess<T>
    {
        private static string LoadConnectionString(String id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static List<T> Select(string Query)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<T>(Query, new DynamicParameters());
                    return output.ToList();
                }
            }
            catch { return null; }
        }
        public static T Select1(string Query)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<T>(Query, new DynamicParameters());
                    return output.Count() > 0 ? output.ToArray()[0] : default(T);
                }
            }
            catch { return default(T); }
        }
    }

    public static class SQLiteDataAccess
    {
        private static string LoadConnectionString(String id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static bool Update(string Query)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Execute(Query, new DynamicParameters());
                }
                return true;
            }
            catch { return false; }
        }
        public static bool Update(string Table, string What, string Value, int ID)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    string Query = "UPDATE " + Table + " SET " + What + "=\"" + Value + "\" WHERE ID=" + ID;
                    cnn.Execute(Query, new DynamicParameters());
                }
                return true;
            }
            catch { return false; }
        }
        public static bool Update(string Table, string What, long Value, int ID)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    string Query = "UPDATE " + Table + " SET " + What + "=" + Value + " WHERE ID=" + ID;
                    cnn.Execute(Query, new DynamicParameters());
                }
                return true;
            }
            catch { return false; }
        }
        public static bool Insert(string Query)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Execute(Query, new DynamicParameters());
                }
                return true;
            }
            catch { return false; }
        }
        public static bool Insert(string Where, object What)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var query = "INSERT INTO " + Where + " VALUES " + What.ToString();
                    cnn.Execute(query, new DynamicParameters());
                }
                return true;
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); return false; }
        }
        public static bool Delete(string Query)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    cnn.Execute(Query, new DynamicParameters());
                }
                return true;
            }
            catch { return false; }
        }
        public static bool Delete(string Where, int ID)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var query = "DELETE FROM " + Where + " WHERE ID=" + ID;
                    cnn.Execute(query, new DynamicParameters());
                }
                return true;
            }
            catch { return false; }
        }

        public static bool IsHaveData(string Query)
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<object>(Query, new DynamicParameters());
                    return output?.Count() > 0;
                }
            }
            catch { return false; }
        }
    }
}

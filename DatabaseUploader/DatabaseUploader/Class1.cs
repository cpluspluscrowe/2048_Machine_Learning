using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTiles;

namespace DatabaseUploader
{
    public class Uploader : IDisposable
    {
        private SQLiteConnection Conn;
        public Uploader()
        {
            Conn =
                new SQLiteConnection(
                    @"Data Source=C:\Users\CCrowe\Documents\2048\training_data.db;Version=3;");
            Conn.Open();
            if (!File.Exists(@"C:\Users\CCrowe\Documents\2048\training_data.db"))
            {
                CreateDatabase();
            }
        }

        public void Dispose()
        {
            Conn.Close();
        }
        public void Upload(List<TrainSwipeData> trainData)
        {
            string sql = @"INSERT INTO TRAINING(hash,classification,delta,";
            string queryString;
            for (int i = 0; i < 16; i++)
            {
                sql += "v" + i.ToString();
                if (!(i == 16 - 1))
                {
                    sql += ",";
                }
                else
                {
                    sql += ") values ";
                }
            }
            string valueString = "";
            foreach (var move in trainData)
            {
                valueString += "(" + move.GetHash().ToString() + ",'" + move.GetClassifierString() + "'," + move.GetScoreDelta() + ",";
                foreach(var number in move.GetConfiguration().GetBoardConfiguration())
                {
                    valueString += number.ToString();
                    valueString += ",";
                }
                valueString = valueString.Substring(0, valueString.Length - 1); //extra comma
                valueString += "),";
                if (valueString.Length > 3000) //queries can only be a certain length
                {
                    queryString = sql + valueString;
                    queryString = queryString.Substring(0, queryString.Length - 1);
                    queryString += ";";
                    InsertStringIntoDatabase(queryString);
                    valueString = "";
                }
            }
            if (valueString.Length > 100)
            {
                queryString = sql + valueString;
                queryString = queryString.Substring(0, queryString.Length - 1);
                queryString += ";";
                InsertStringIntoDatabase(queryString);
                valueString = "";
            }

        }

        protected void InsertStringIntoDatabase(string sql)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sql, Conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
        protected void CreateDatabase()
        {
            SQLiteConnection.CreateFile(@"C:\Users\CCrowe\Documents\2048\training_data.db");
            SQLiteConnection conn =
                new SQLiteConnection(
                    @"Data Source=C:\Users\CCrowe\Documents\2048\training_data.db;Version=3;");
            conn.Open();
            string sql = @"CREATE TABLE TRAINING(hash real,classification text,delta int,";
            for (int i = 0; i < 16; i++)
            {
                sql += "v" + i.ToString() + " int";
                if (!(i == 16 - 1))
                {
                    sql += ",";
                }
                else
                {
                    sql += ");";
                }
            }
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
/*
                SQLiteDataReader reader = cmd.ExecuteReader();
                do
                {
                    while (reader.Read())
                    {
                        string[] colNames = new string[2];
                        reader.GetValues(colNames);
                        fileDescription = colNames[0];
                    }
                } while (reader.NextResult());
                reader.Close();
*/
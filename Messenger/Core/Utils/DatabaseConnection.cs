using System;
using System.Data;
using System.Windows;
using Messenger.MVVM.Model;
using Npgsql;

namespace Messenger.Core.Utils
{
    internal class DatabaseConnection
    {
        private static string _connectionString =
            "Host=localhost;Username=postgres;Password=Nittispermus123;Database=Messenger";

        private readonly NpgsqlConnection _sqlConnection = new NpgsqlConnection(_connectionString);

        public void DbConnection()
        {
            try
            {
                _sqlConnection.Open();

                try
                {
                    string query = @"CREATE TABLE users(
                            id SERIAL PRIMARY KEY,
                            username VARCHAR(30) NOT NULL UNIQUE, 
                            phoneNumber BIGINT NOT NULL UNIQUE,
                            status VARCHAR(40))";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, _sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException)
                {
                }
            }

            catch (NpgsqlException ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public void CreateNewUser(string username, long phoneNumber, string status)
        {
            _sqlConnection.Open();

            string query =
                "INSERT INTO users(username, phoneNumber, status) VALUES(@username, @phoneNumber, @status)";
            NpgsqlCommand command = new NpgsqlCommand(query, _sqlConnection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@status", status);
            command.ExecuteNonQuery();
        }

        public UserModel InitializeUser(long phoneNumber)
        {
            try
            {
                UserModel user = new UserModel();
                using (NpgsqlConnection myConnection = new NpgsqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM users WHERE phoneNumber = @phoneNumber";
                    NpgsqlCommand command = new NpgsqlCommand(query, myConnection);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    myConnection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Username = reader["username"].ToString();
                            user.PhoneNumber = phoneNumber;
                            user.Status = reader["status"].ToString();
                        }

                        myConnection.Close();
                    }
                }
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
using System;
using System.Data;
using System.Windows;
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
                            id SERIAL PRIMARY KEY ,
                            login varchar(20) NOT NULL, 
                            password varchar(30) NOT NULL, 
                            username varchar(30) NOT NULL, 
                            email varchar(40) NOT NULL,
                            status varchar(40))";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, _sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException) {}
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public void CreateNewUser(string login, string password, string username, string email, string status)
        {
            _sqlConnection.Open();

            string query =
                "INSERT INTO users(login, password, username, email, status) VALUES(@login, @password, @username, @email, @status)";
            NpgsqlCommand command = new NpgsqlCommand(query, _sqlConnection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@status", status);
            command.ExecuteNonQuery();
        }
    }
}
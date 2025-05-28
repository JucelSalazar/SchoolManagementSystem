using MySql.Data.MySqlClient;
using System;

public class DatabaseHelper
{
    private readonly string connectionString = "Server=127.0.0.1;Port=3306;Database=school_management;User=root;Password=root;";

    public bool TestConnection()
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                return true; 
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

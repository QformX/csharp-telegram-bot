using Microsoft.Data.SqlClient;

namespace SQLDatabase
{
    public class SQLHandler
    {
        private SqlConnection _connection;
        //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\QForm\Desktop\csharp-telegram-bot\SQLDatabase\UsersSQLDatabase.mdf;Integrated Security=True
        public void IniInitialize()
        {
            _connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\SQLDatabase\\UsersSQLDatabase.mdf;Integrated Security=True");
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open) { Console.WriteLine("HELLO"); }
        }

        public void Insert(UserData data)
        {
            SqlCommand cmd = new SqlCommand($"INSERT INTO [UsersData] (city, country, temp_c, feelslike, is_day, wind_speed, wind_dir, clouth, Id) VALUES (N'{data._city}', N'{data._country}', '{data._temp_c}', '{data._feelslike}', '{data._is_day}', '{data._wind_speed}', '{data._wind_dir}', N'{data._clouth}', '{data._chat_id}')", _connection);
            cmd.ExecuteNonQuery();
            Console.WriteLine(data.ToString());
        }
    }
}
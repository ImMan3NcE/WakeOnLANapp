using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WakeOnLAN.MVVM.Models;

namespace WakeOnLAN.Repositories
{
    public class BaseRepository
    {

        SQLiteConnection connection;
        public string StatusMessage;

        public BaseRepository()
        {
            connection = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
            connection.CreateTable<Address>();
        }

        public void Add(Address newAddress)
        {
            int result = 0;
            try
            {
                result=connection.Insert(newAddress);
                StatusMessage = $"{result} rows added";
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}"; ;
            }
        }

    }
}

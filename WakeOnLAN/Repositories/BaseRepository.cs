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
            connection.CreateTable<TempAddress>();
        }



        public void AddOrUpdate(Address address)
        {
            int result = 0;
            try
            {
                if(address.IdAddress != 0)
                {
                    result = connection.Update(address);
                    StatusMessage = $"{result} rows updated";
                }
                else
                {
                    result = connection.Insert(address);
                    StatusMessage = $"{result} rows added";
                }
                
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}"; ;
            }
        }


        public List<Address> GetAllAdresses()
        {
            try
            {
                return connection.Query<Address>("SELECT * FROM ConnectionAddress").ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}"; 
            }
            return null;
        }

        public Address GetOneAddress(int id)
        {
            try
            {
                return connection.Table<Address>().FirstOrDefault(x => x.IdAddress == id);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            return null;
        }

        public void DeleteAddress(int id)
        {
            try
            {
                var address = GetOneAddress(id);
                connection.Delete(address);
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
        }

        public List<TempAddress> GetAllTempAdresses()
        {
            try
            {
                return connection.Query<TempAddress>("SELECT * FROM ConnectionTempAddress").ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = $"Error: {ex.Message}";
            }
            return null;
        }

    }
}

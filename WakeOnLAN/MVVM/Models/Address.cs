using SQLite;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeOnLAN.MVVM.Models
{
    [Table("ConnectionAddress")]
    public class Address
    {
        [PrimaryKey,AutoIncrement]
        public int IdAddress { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
    }
}

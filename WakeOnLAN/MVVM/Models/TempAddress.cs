using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeOnLAN.MVVM.Models
{
    [Table("ConnectionTempAddress")]
    public class TempAddress
    {
        [PrimaryKey, AutoIncrement]
        public int IdTempAddress { get; set; }
        public string NameTempAddress { get; set; }
        public string MacAddressTempAddress { get; set; }
        public string IpAddressTempAddress { get; set; }
        public string PortTempAddress { get; set; }


    }
}

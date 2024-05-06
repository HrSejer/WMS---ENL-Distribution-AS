using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS___ENL_Distribution_AS
{
    public class Location
    {
        public int rowNum { get; set; }
        public int shelfNum { get; set; }

        public Location()
        {

        }

        public string Placement()
        {
            return $"{rowNum}. {shelfNum}";
        }
    }
}
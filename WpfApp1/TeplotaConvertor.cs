using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class TeplotaConvertor
    {
        public int Convertor(string teplota)
        {

            
            int number1 = 0;
            bool canConvert = int.TryParse(teplota, out number1);
            if (canConvert == true)
                return Int32.Parse(teplota);
            else
                throw new ArgumentException();


           
        }
    }
}

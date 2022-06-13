using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExploreCSharp.HWs;

namespace ExploreCSharp
{
    class Program
    {
        static void Main()
        {
            BankApplication bankApplication = new BankApplication();
            bankApplication.mainmenu();

            Console.ReadLine();
        }
    }
}

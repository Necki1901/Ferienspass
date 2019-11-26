using Ferienspaß.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Child test = new Child("Michi", "Plasser", Convert.ToDateTime("19.08.2000"));

            Console.WriteLine(test.CreateChild().ToString());

        }
    }
}

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
            Child test = new Child("Michi", "Lol", Convert.ToDateTime("19.08.2000"));

            
            //Console.WriteLine("Create: " + test.CreateChild().ToString());
            Console.WriteLine("Delete: " + test.DeleteChild().ToString());
            //Console.WriteLine("Update: " + test.UpdateChild("Michael", "Reiter", Convert.ToDateTime("19.08.2000")).ToString());
            Console.ReadLine();

        }
    }
}

using System;
using System.Collections.Generic;

namespace OS {
    public class Program {

        // Examples from our docs
        public static List<string> Example1 = new List<string> {
            "SD00 JP00$",
            "JP00",
        };

        // Testing program for fking losers
        public static List<string> TestExample = new List<string> {
            "MCHR",
        };

        public static void Main(string[] args) {
            Console.WriteLine("Yo what you girls doin tonight is what I wanted to say");

            var cpu = CentralCPU.Instance();
            var vm = cpu.CreateVM();


            //vm.StoreCommandsInMemory(Example2);
            //Sita ant loop gal?
            vm.ExecuteCommandFromFile();
            vm.ReadFromMemory();
        }
    }
}

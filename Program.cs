using System;
using System.Collections.Generic;

namespace OS {
    public class Program {

        // Examples from our docs
        public static List<string> Example1 = new List<string> {
            "SD00 JP00$",
            "JP00",
        };

        public static List<string> Example2 = new List<string> { // TODO change pdf and word for this example, because we do not have RDBX anymore
            "SD00 Iveskite pirma skaiciu$",
            "SD30 Iveskite antra skaiciu$",
            "SD50 Suma: $",
            "PA00",
            "RDAX",
            "PA30",
            "SWAP",
            "RDAX",
            "_ADD",
            "PA50",
            "PRAX",
            "HALT",
        };

        // Testing program for fking losers
        public static List<string> TestExample = new List<string> {
            "MCHR",
        };

        public static void Main(string[] args) {
            Console.WriteLine("i saw these 2 fine bitches walking my way");

            var cpu = CentralCPU.Instance();
            var vm = cpu.CreateVM();


            vm.StoreCommandsInMemory(Example2);
            vm.ReadFromMemory();
        }
    }
}

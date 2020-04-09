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

        // Testing program for devs <- DEVS OMEGALUL
        public static List<string> TestExample = new List<string> {
            "PRAX",
            "PRAX",
            "PRAX",
            "PRAX",
            "PRAX",
            "PRAX",
        };

        public static void Main(string[] args) {
            Console.WriteLine("you want some more of this bitch?");

            Console.WriteLine("My planned usage: write line by line commands");
            Console.WriteLine("e.g. \"PRAX\" will execute PRAX command, try it out\n");

            var cpu = new CentralCPU();
            var parser = new Parser(cpu);

            var vm = new VM(parser);

            vm.HardCodedInput(Example2);
            //vm.LineByLineInput();
        }
    }
}

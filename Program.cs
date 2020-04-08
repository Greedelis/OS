using System;

namespace OS {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("you want some more of this bitch?");

            Console.WriteLine("My planned usage: write line by line commands");
            Console.WriteLine("e.g. \"PRAX\" will execute PRAX command, try it out\n");

            var cpu = new CentralCPU();
            var parser = new Parser(cpu);

            var vm = new VM(parser);
            vm.LineByLineInput();
        }
    }
}

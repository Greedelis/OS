using System;

namespace OS {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("you want some more of this bitch?");

            var cpu = new CentralCPU();
            var parser = new Parser(cpu);

            var vm = new VM(parser);
            vm.LineByLineInput();

        }
    }
}

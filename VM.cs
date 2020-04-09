using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OS {
    public class VM {

        private readonly Parser m_parser;

        public VM (Parser parser) {
            m_parser = parser;
        }

        public void LineByLineInput() {
            string input;

            while (true) {
                input = Console.ReadLine();
                if (input == "stop")
                    break;

                m_parser.ExecuteCommand(input);
            }
        }

        public void ReadFromFileInput(string filepath) {
            var lines = File.ReadLines(filepath).ToList();
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

        public void HardCodedInput(List<string> lines) {
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

        public void Output() {

        }

    }
}

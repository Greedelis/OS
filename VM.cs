﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OS {
    public class VM {

        private readonly CentralCPU m_cpu;
        private readonly Parser m_parser;
        private readonly Memory m_memory = new Memory();

        public VM (CentralCPU cpu, Parser parser) {
            m_cpu = cpu;
            m_parser = parser;
        }

        public void StoreCommandsInMemory(List<string> lines) {
            var wordCounter = 0;

            lines.ForEach(line => {
                switch (line.Length) {
                    case int n when n < 4:
                        throw new Exception($"StoreCommandsInMemory: line length is less than 4: '{line}'");
                    case int n when n == 4:
                        m_memory.PutToMemory(wordCounter++, line);
                        break;
                    case int n when n > 4:
                        Parser.SplitToParts(line.Remove(4, 1), 4) // If command is more than 4 bytes, then 5th byte is space before additional data
                            .ToList()
                            .ForEach(part => m_memory.PutToMemory(wordCounter++, part));
                        break;
                }
            });
        }

        public void ReadFromMemory() {
            
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

        public void ReadFromFileInput(string filepath) { // TODO: Change this to work from HDD instead
            var lines = File.ReadLines(filepath).ToList();
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

        public void HardCodedInput(List<string> lines) {
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

    }
}

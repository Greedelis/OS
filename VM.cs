using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OS {
    public class VM {

        private readonly CentralCPU m_cpu;
        private readonly Parser m_parser;
        private readonly Memory m_memory;
        private readonly SoftDisk m_disk;
        private readonly List<int> m_allowedBlocks;

        public VM (CentralCPU cpu, Parser parser, Memory memory, SoftDisk disk, List<int> allowedBlocks) {
            m_cpu = cpu;
            m_parser = parser;
            m_memory = memory;
            m_allowedBlocks = allowedBlocks;
            m_disk = disk;
        }

        public void StoreCommandsInMemory(List<string> lines) {
            var wordCounter = 0;
            var currentBlock = 0;

            lines.ForEach(line => {
                if (wordCounter > 15) { // Jump into next block
                    wordCounter = 0;
                    currentBlock++;
                }

                switch (line.Length) {
                    case int n when n < 4:
                        throw new Exception($"StoreCommandsInMemory: line length is less than 4: '{line}'");
                    case int n when n == 4:
                        m_memory.PutToMemory(m_allowedBlocks[currentBlock], wordCounter++, line);
                        break;
                    case int n when n > 4:
                        Parser.SplitToParts(line.Remove(4, 1), 4) // If command is more than 4 bytes, then 5th byte is space before additional data
                            .ToList()
                            .ForEach(part => m_memory.PutToMemory(m_allowedBlocks[currentBlock], wordCounter++, part));
                        break;
                }
            });
        }

        public void ReadFromMemory() {
            var currentBlock = 0;
            var currentWord = 0;

            while (true) {
                var x = m_memory.GetWordAsString(m_allowedBlocks[currentBlock], currentWord);

            }
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

        public void ReadFromFileInput(string filename) { // TODO: Change this to work from HDD instead
            var lines = m_disk.OpenFile(filename); //atidarys root/filename
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

        public void HardCodedInput(List<string> lines) {
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

    }
}

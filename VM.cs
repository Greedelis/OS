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
        private int m_currentBlock = 0;
        private int m_currentWord = 0;

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
                            .ForEach(part => {
                                if (wordCounter > 15) { // Jump into next block
                                    wordCounter = 0;
                                    currentBlock++;
                                }
                                m_memory.PutToMemory(m_allowedBlocks[currentBlock], wordCounter++, part);
                            });
                        break;
                }
            });
        }

        private string GetNextWord() {
            m_currentWord++;

            if (m_currentWord > 15) {
                m_currentWord = 0;
                m_currentBlock++;
            }

            if (m_currentBlock > m_allowedBlocks.Count)
                throw new OutOfMemoryException($"ERROR: VM ran out of memory: {m_currentBlock} block, {m_currentWord} word");

            return m_memory.GetWordAsString(m_allowedBlocks[m_currentBlock], m_currentWord);
        }

        public void ReadFromMemory() {
            while (true) {
                var str = GetNextWord();

                if (m_parser.IsAdditionalBytesNeeded(str)) {
                    str += " ";
                    while (!str.Contains('$'))
                        str += GetNextWord();
                }

                m_parser.ExecuteCommand(str);
                m_cpu.Test();
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

        public void ReadFromFileInput(string filename) { 
            var lines = m_disk.OpenFile(filename); //atidarys root/filename
            StoreCommandsInMemory(lines);
        }

        public bool ExecuteCommandFromFile() //returns true if store in memory happened
        {
            //----------------Sita vieta probably reikes keist su kokia ekrano ir klaviaturos klasem
            Console.WriteLine("Type the name of a program that you'd like to run"); 
            m_cpu.SDIR();
            var input = Console.ReadLine();
            //------------------
            var fileContents = m_disk.OpenFile(input);
            if (fileContents == null) return false;
            StoreCommandsInMemory(fileContents);
            return true;

        }

        public void HardCodedInput(List<string> lines) {
            lines.ForEach(line => m_parser.ExecuteCommand(line));
        }

    }
}

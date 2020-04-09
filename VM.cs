﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OS {
    public class VM {

        private readonly Parser m_parser;

        public VM (Parser parser) {
            m_parser = parser;
        }

        public void LineByLineInput() {
            string input = string.Empty;

            while (true) {
                input = Console.ReadLine();
                if (input == "stop")
                    break;

                m_parser.ExecuteCommand(input);
            }
        }

        public void Output() {

        }

    }
}
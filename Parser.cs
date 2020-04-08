using System;
using System.Collections.Generic;

namespace OS {

    public enum CommandType {
        ERROR, // For bad commands

        CMP,

        LA,
        SA,
        PRAX,
        PR,
        PA,
        RDAX,
        RD,
        RDA,
        SWAP,

        JP,
        JM,
        JL,
        JE,
        JN,
        JX,
        JY,

        HALT,
        SD
    }

    public class Parser {

        private readonly CentralCPU m_cpu;

        public Parser (CentralCPU cpu) {
            m_cpu = cpu;
        }

        private static readonly Dictionary<string, CommandType> CommandRepresentations = new Dictionary<string, CommandType> {
            { "_CMP", CommandType.CMP },
            { "LA", CommandType.LA },
            // ...
        };

        public void ExecuteCommand (string line) {
            var type = ParseCommandType(line);

            switch (type) {
                case CommandType.CMP:
                    m_cpu.CMP();
                    break;
                case CommandType.LA:
                    var parsedParams = ParseParams(line);
                    m_cpu.LA(parsedParams.Item1, parsedParams.Item2);
                    break;
                // ...
                case CommandType.ERROR:
                    throw new Exception ("Bad command");
                default:
                    throw new NotImplementedException ($"Command type: {type} is not implemented");
            }
        }

        private CommandType ParseCommandType(string line) {
            if (line.Length != 4) // All commands have 4 symbols
                return CommandType.ERROR;

            foreach (var command in CommandRepresentations) {
                if (line.StartsWith(command.Key))
                    return command.Value;
            }

            return CommandType.ERROR;
        }

        private (int, int) ParseParams (string line) {
            if (line.Length != 4)
                return (0, 0);

            return (int.Parse(line[2].ToString()), int.Parse(line[3].ToString()));
        }
    }
}

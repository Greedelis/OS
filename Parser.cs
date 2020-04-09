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
            { "SA", CommandType.SA },
            // ...
            { "PRAX", CommandType.PRAX },
            { "PR", CommandType.PR },
            //jumps
            { "JP", CommandType.JP },
            { "JM", CommandType.JM },
            { "JL", CommandType.JL },
            { "JE", CommandType.JN },
            { "JN", CommandType.JE },
            { "JX", CommandType.JX },
            { "JY", CommandType.JY },
            //...
            { "HALT", CommandType.HALT },
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
                case CommandType.PRAX:
                    m_cpu.PRAX();
                    break;
                // ...
                case CommandType.ERROR:
                    Console.WriteLine("Bad command, no exception for now");
                    break;
                case CommandType.SA:
                    parsedParams = ParseParams(line);
                    m_cpu.SA(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.PR:
                    parsedParams = ParseParams(line);
                    m_cpu.PR(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.PA:
                    break;
                case CommandType.RDAX:
                    break;
                case CommandType.RD:
                    break;
                case CommandType.RDA:
                    break;
                case CommandType.SWAP:
                    m_cpu.SWAP();
                    break;
                case CommandType.JP:
                    parsedParams = ParseParams(line);
                    m_cpu.JP(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.JM:
                    parsedParams = ParseParams(line);
                    m_cpu.JM(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.JL:
                    parsedParams = ParseParams(line);
                    m_cpu.JL(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.JE:
                    parsedParams = ParseParams(line);
                    m_cpu.JE(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.JN:
                    parsedParams = ParseParams(line);
                    m_cpu.JN(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.JX:
                    parsedParams = ParseParams(line);
                    m_cpu.JX(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.JY:
                    parsedParams = ParseParams(line);
                    m_cpu.JY(parsedParams.Item1, parsedParams.Item2);
                    break;
                case CommandType.HALT:
                    m_cpu.HALT();
                    break;
                case CommandType.SD:
                    break;
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
            if (line.Length != 4) // All commands have 4 symbols
                return (-1, -1);

            return (int.Parse(line[2].ToString()), int.Parse(line[3].ToString()));
        }
    }
}

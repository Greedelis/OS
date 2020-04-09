using System;
using System.Collections.Generic;

namespace OS {

    public enum CommandType {
        ERROR, // For bad commands

        ADD,
        SUB,
        MUL,
        DIV,

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
            { "_ADD", CommandType.ADD },
            { "_SUB", CommandType.SUB },
            { "_MUL", CommandType.MUL },
            { "_DIV", CommandType.DIV },

            { "_CMP", CommandType.CMP },

            { "LA", CommandType.LA },
            { "SA", CommandType.SA },
            { "PRAX", CommandType.PRAX },
            { "PR", CommandType.PR },
            { "PA", CommandType.PA },
            { "RDAX", CommandType.RDAX },
            { "RD", CommandType.RD },
            { "RDA", CommandType.RDA },
            { "SWAP", CommandType.SWAP },

            { "JP", CommandType.JP },
            { "JM", CommandType.JM },
            { "JL", CommandType.JL },
            { "JE", CommandType.JE },
            { "JN", CommandType.JN },
            { "JX", CommandType.JX },
            { "JY", CommandType.JY },

            { "HALT", CommandType.HALT },
            { "SD", CommandType.SD },
        };

        public void ExecuteCommand (string line) {
            var type = ParseCommandType(line);
            (int, int) duoParams;
            int singleParam;
            string stringParam;

            switch (type) {
                case CommandType.ADD:
                    m_cpu.ADD();
                    break;                
                case CommandType.SUB:
                    m_cpu.SUB();
                    break;                
                case CommandType.MUL:
                    m_cpu.MUL();
                    break;                
                case CommandType.DIV:
                    m_cpu.DIV();
                    break;
                case CommandType.CMP:
                    m_cpu.CMP();
                    break;
                case CommandType.LA:
                    duoParams = ParseParams(line);
                    m_cpu.LA(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.SA:
                    duoParams = ParseParams(line);
                    m_cpu.SA(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.PRAX:
                    m_cpu.PRAX();
                    break;
                case CommandType.PR:
                    duoParams = ParseParams(line);
                    m_cpu.PR(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.PA:
                    duoParams = ParseParams(line);
                    m_cpu.PA(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.RDAX:
                    m_cpu.RDAX();
                    break;
                case CommandType.RD:
                    duoParams = ParseParams(line);
                    m_cpu.RD(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.RDA:
                    singleParam = ParseSingleParam(line);
                    m_cpu.RDA(singleParam);
                    break;
                case CommandType.SWAP:
                    m_cpu.SWAP();
                    break;
                case CommandType.JP:
                    duoParams = ParseParams(line);
                    m_cpu.JP(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.JM:
                    duoParams = ParseParams(line);
                    m_cpu.JM(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.JL:
                    duoParams = ParseParams(line);
                    m_cpu.JL(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.JE:
                    duoParams = ParseParams(line);
                    m_cpu.JE(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.JN:
                    duoParams = ParseParams(line);
                    m_cpu.JN(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.JX:
                    duoParams = ParseParams(line);
                    m_cpu.JX(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.JY:
                    duoParams = ParseParams(line);
                    m_cpu.JY(duoParams.Item1, duoParams.Item2);
                    break;
                case CommandType.HALT:
                    m_cpu.HALT();
                    break;
                case CommandType.SD:
                    stringParam = ParseStringData(line);
                    m_cpu.SD(stringParam);
                    break;
                case CommandType.ERROR:
                    Console.WriteLine("Bad command, no exception for now");
                    break;
                default:
                    throw new NotImplementedException ($"Command type: {type} is not implemented");
            }
        }

        private CommandType ParseCommandType(string line) {
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

        private int ParseSingleParam (string line) {
            if (line.Length != 4) // All commands have 4 symbols
                return -1;

            return int.Parse(line[3].ToString());
        }

        private string ParseStringData (string line) {
            if (line.Length <= 4) // Command is 4 symbols, data comes after that until char '$'
                return string.Empty;

            var endCharLocation = line.IndexOf("$");
            return endCharLocation > 4
                ? line[5..endCharLocation]
                : string.Empty;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Globalization;

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
        PRBX,
        PR,
        PA,
        RDAX,
        RDBX,
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
        MINT,
        MCHR,
        
        SD,
        //-----HARD DISK COMMANDS, NOT SURE AR CIA, JEI KAS PAKEISIM
        CHDR,
        SDIR,
        
    }

    public class Parser {

        private readonly CentralCPU m_cpu;

        public Parser (CentralCPU cpu) {
            m_cpu = cpu;
        }

        // We need to check which command is in line, but some commands have overlapping letters
        // e.g. RD is start of RDA and RDAX too
        // So we need command priorities

        // Full 4 symbol commands
        private static readonly Dictionary<string, CommandType> FirstPriorityCheckCommands = new Dictionary<string, CommandType> {
            { "_ADD", CommandType.ADD },
            { "_SUB", CommandType.SUB },
            { "_MUL", CommandType.MUL },
            { "_DIV", CommandType.DIV },
            { "_CMP", CommandType.CMP },
            { "PRAX", CommandType.PRAX },
            { "PRBX", CommandType.PRBX },
            { "RDAX", CommandType.RDAX },
            { "RDBX", CommandType.RDBX },
            { "SWAP", CommandType.SWAP }, 
            { "HALT", CommandType.HALT },
            { "MINT", CommandType.MINT },
            { "MCHR", CommandType.MCHR },
            { "CHDR", CommandType.CHDR },
            { "SDIR", CommandType.SDIR },
        };

        // 3 symbol commands
        private static readonly Dictionary<string, CommandType> SecondPriorityCheckCommands = new Dictionary<string, CommandType> {
            { "RDA", CommandType.RDA },
        };

        // 2 symbol commands
        private static readonly Dictionary<string, CommandType> ThirdPriorityCheckCommands = new Dictionary<string, CommandType> {
            { "LA", CommandType.LA },
            { "SA", CommandType.SA },
            { "PR", CommandType.PR },
            { "PA", CommandType.PA },
            { "RD", CommandType.RD },
            { "JP", CommandType.JP },
            { "JM", CommandType.JM },
            { "JL", CommandType.JL },
            { "JE", CommandType.JE },
            { "JN", CommandType.JN },
            { "JX", CommandType.JX },
            { "JY", CommandType.JY },
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
                    duoParams = ParseParams(line);
                    stringParam = ParseStringData(line);
                    m_cpu.SD(duoParams.Item1, duoParams.Item2, stringParam);
                    break;
                case CommandType.ERROR:
                    Console.WriteLine("Bad command, no exception for now");
                    break;
                case CommandType.PRBX:
                    m_cpu.PRBX();
                    break;
                case CommandType.RDBX:
                    m_cpu.RDBX();
                    break;
                case CommandType.MINT:
                    m_cpu.MINT();
                    break;
                case CommandType.MCHR:
                    m_cpu.MCHR();
                    break;
                case CommandType.CHDR:
                    stringParam = ParseStringData(line);
                    m_cpu.CHDR(stringParam);
                    break;
                case CommandType.SDIR:
                    m_cpu.SDIR();
                    break;
                default:
                    throw new NotImplementedException ($"Command type: {type} is not implemented");
            }
        }

        public bool IsAdditionalBytesNeeded (string line) { // Returns true if additional bytes is needed
            return ParseCommandType(line) switch {
                CommandType.SD => true, // Only command to need additional bytes
                CommandType.ERROR => throw new Exception($"Line '{line}' is not correct"),
                _ => false,
            };
        }

        public bool IsJump (CommandType type) {
            switch (type) {
                case CommandType.JE:
                case CommandType.JL:
                case CommandType.JM:
                case CommandType.JN:
                case CommandType.JP:
                case CommandType.JX:
                case CommandType.JY:
                    return true;
                default:
                    return false;
            }
        }

        public CommandType ParseCommandType(string line) {
            foreach (var command in FirstPriorityCheckCommands) {
                if (line.StartsWith(command.Key))
                    return command.Value;
            }

            foreach (var command in SecondPriorityCheckCommands) {
                if (line.StartsWith(command.Key))
                    return command.Value;
            }

            foreach (var command in ThirdPriorityCheckCommands) {
                if (line.StartsWith(command.Key))
                    return command.Value;
            }

            return CommandType.ERROR;
        }

        public (int, int) ParseParams (string line) {
            return (int.Parse(line[2].ToString(), NumberStyles.HexNumber), int.Parse(line[3].ToString(), NumberStyles.HexNumber));
        }

        private int ParseSingleParam (string line) {
            if (line.Length != 4) // All commands have 4 symbols
                return -1;

            return int.Parse(line[3].ToString(), NumberStyles.HexNumber);
        }

        private string ParseStringData (string line) {
            if (line.Length <= 4) // Command is 4 symbols, data comes after that until char '$'
                return string.Empty;

            var endCharLocation = line.IndexOf("$");
            return endCharLocation > 4
                ? line[5..(endCharLocation + 1)]
                : string.Empty;
        }

        public static IEnumerable<string> SplitToParts(string data, int partLength) {
            for (var i = 0; i < data.Length; i += partLength)
                yield return data.Substring(i, Math.Min(partLength, data.Length - i));
        }
    }
}

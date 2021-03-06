﻿using System;
using System.Collections.Generic;

namespace OS {
        class Word{
        private byte[] _word = new byte[4]{0, 0, 0, 0};

        public Word()
        {

        }
        public Word(UInt32 val)
        {
            SetValue(val);
        }

        public UInt32 ToInt32(){
            return (UInt32)Math.Pow(16,6) * _word[0] + (UInt32)Math.Pow(16,4) * _word[1] + (UInt32)Math.Pow(16,2) * _word[2] + _word[3];
        }
        public String IntoString(){
            char[] temp_arr = {(char)_word[0],(char)_word[1],(char)_word[2],(char)_word[3]};
            return new string(temp_arr);
        }
        public void SetValue(UInt32 newVal){
            _word[0] = (byte)(newVal/((UInt32)Math.Pow(16,6)));
            newVal -= _word[0] * (UInt32)Math.Pow(16,6);
            _word[1] = (byte)(newVal/((UInt32)Math.Pow(16,4)));
            newVal -= _word[1] * (UInt32)Math.Pow(16,4);
            _word[2] = (byte)(newVal/((UInt32)Math.Pow(16,2)));
            _word[3] = (byte)(newVal - _word[2] * (UInt32)Math.Pow(16,2));
        }
        public void SetValue(string newVal){
            int leng = newVal.Length;
            _word = new byte[]{0, 0, 0 ,0};
            if(leng >= 4){
                for(int i = 0; i < 4; i++){
                    _word[i] = (byte)newVal[i];
                }
            }else if(leng < 4){
                for(int i = 0; i < leng; i++){
                    _word[i] =  (byte)newVal[i];
                }
            }
        }
        public void PrintValues(){
            foreach (var item in _word){
                Console.Write("{0} ", item);
            }
            Console.WriteLine();
        }
        public void PrintCharValues(){
            foreach (var item in _word){
                Console.Write("{0} ", (char)item);
            }
            Console.WriteLine();
        }

        public byte GetByte(int bytePointer) {
            if (bytePointer < 0 || bytePointer >= 4)
                throw new Exception($"GetByte: Byte pointer is not in [0, 3]: {bytePointer}");

            return _word[bytePointer];
        }

        public void SetByte(int bytePointer, byte value) {
            if (bytePointer < 0 || bytePointer >= 4)
                throw new Exception($"SetByte: Byte pointer is not in [0, 3]: {bytePointer}");

            _word[bytePointer] = value;
        }

    }
    public class Memory {
        private static int wordsPerBlock = 16;
        private static int blocks = 80;
        private List<Word> m_data = new List<Word>(wordsPerBlock * blocks);

        private int m_pointer = -1;

        public Memory() {
            for (var i = 0; i < m_data.Capacity; i++) {
                m_data.Add(new Word());
            }
        }
        public string GetWordAsString(int blockNumber, int wordNumber){
            return m_data[16*blockNumber+wordNumber].IntoString();
        }

        public uint GetMemoryPointer() {
            if (m_pointer < 0 || m_pointer > wordsPerBlock * blocks)
                throw new ArgumentOutOfRangeException($"Memory pointer is out of range: {m_pointer}");

            return m_data[m_pointer].ToInt32();
        }

        public string GetMemoryPointerStr() {
            if (m_pointer < 0 || m_pointer > wordsPerBlock * blocks)
                throw new ArgumentOutOfRangeException($"Memory pointer is out of range: {m_pointer}");

            return m_data[m_pointer].IntoString();
        }

        public void SetMemoryPointer(int blockNumber, int wordNumber) {
            m_pointer = 16 * blockNumber + wordNumber;
        }

        public uint GetFromMemory(int blockNumber, int wordNumber) {
            return m_data[16*blockNumber+wordNumber].ToInt32();
        }

        public uint GetFromMemory(int blockNumber, int wordNumber, int bytePointer) { // Gets word and takes one of its bytes
            return m_data[16*blockNumber+wordNumber].GetByte(bytePointer);
        } 

        public void PutToMemory(int blockNumber, int wordNumber, uint data)
        {
            try
                {m_data[16*blockNumber+wordNumber].SetValue(data);}
            catch(ArgumentOutOfRangeException e){
                Console.WriteLine(e);
            }
        }

        public void PutToMemory(int blockNumber, int wordNumber, string data) 
        {   
            try
                {m_data[16*blockNumber+wordNumber].SetValue(data);}
            catch(ArgumentOutOfRangeException e){
                Console.WriteLine(e);
            }
            
        }

        public void PutToMemory(int blockNumber, int wordNumber, int bytePointer, byte value) {
            m_data[16*blockNumber+wordNumber].SetByte(bytePointer, value);
        }

        public void PrintMemoryInts()
        {
            const int spaceSeparatorInt = 4;
            const int blockSepararotInt = 16;
            const int newLineSeparatorInt = 80;
            for (var i = 0; i < m_data.Count; i++)
            {
                if(i % spaceSeparatorInt == 0 && i != 0)
                    Console.Write(" | ");
                if(i % blockSepararotInt == 0 && i != 0)
                    Console.Write("|");
                if(i % newLineSeparatorInt == 0 && i != 0)
                    Console.Write("\n");
                Console.Write($"{m_data[i].ToInt32()} ");
            }
            Console.Write(" |\n");
        }
        public List<int> ReserveMemory(){
            List<int> array = new List<int>();
            uint count = 0;
            int i = 0;
            while(count < 15 && i < 80){
                if(IsBlockEmpty(i)){
                    array.Add(i);
                    count++;
                }
                i++;
            }
            if(count < 15){
                throw new Exception ($"not enought free memory");
            }
            return array;
        }
        public void FreeMemory(List<int> array){
            for(int i = 0; i < 16; i++){
                for(int y = 0; y < 16; y++){
                    m_data[16*(int)array[i]+y].SetValue(0);
                }
            }
        }
        public bool IsBlockEmpty(int block){
            for(int i = 0; i < 16; i++){
                try{
                if(m_data[16*block+i].ToInt32() != 0){
                    return false;
                }}catch(ArgumentOutOfRangeException e){
                    Console.WriteLine(e);
                }
            }
            return true;
        }
        public void PrintMemoryChars()
        {
            const int spaceSeparatorInt = 1;
            const int blockSepararotInt = 4;
            const int newLineSeparatorInt = 40;
            for (var i = 0; i < m_data.Count; i++)
            {
                if(i % spaceSeparatorInt == 0 && i != 0)
                    Console.Write("|");
                if(i % blockSepararotInt == 0 && i != 0)
                    Console.Write("|");
                if(i % newLineSeparatorInt == 0 && i != 0)
                    Console.Write("\n");
                var byteString = m_data[i].IntoString().Replace((char)0, '0');
                Console.Write(byteString);
            }
            Console.Write("|\n");
        }
    }
}

using System;
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
        //private Word[] m_data = new Word[wordsPerBlock*blocks]; // I don't fucking know
        private List<Word> m_data = new List<Word>(wordsPerBlock * blocks);
        private int memoryPointer = 0;

        public Memory() {
            for (var i = 0; i < m_data.Capacity; i++) {
                m_data.Add(new Word());
            }
        }

        public bool ChangeMemoryPointer(int newPointer) {
            if (newPointer < 0 && newPointer >= m_data.Count) //add conditions here I guess
                return false;
            memoryPointer = newPointer;
            return true;
        }

        public uint GetFromMemory(int pointer) {
            return m_data[pointer].ToInt32();
        }

        public uint GetFromMemory(int wordPointer, int bytePointer) { // Gets word and takes one of its bytes
            return m_data[wordPointer].GetByte(bytePointer);
        } 

        public void PutToMemory(int pointer, uint data)
        {
            m_data[pointer].SetValue(data);
        }

        public void PutToMemory(int pointer, string data) 
        {
            m_data[pointer].SetValue(data);
        }

        public void PutToMemory(int wordPointer, int bytePointer, byte value) {
            m_data[wordPointer].SetByte(bytePointer, value);
        }

        public void PrintMemoryInts()
        {
            const int spaceSeparatorInt = 4;
            const int newLineSeparatorInt = 80;
            for (var i = 0; i < m_data.Count; i++)
            {
                if(i % spaceSeparatorInt == 0)
                    Console.Write(" | ");
                if(i % newLineSeparatorInt == 0)
                    Console.Write("\n");
                Console.Write($"{m_data[i].ToInt32()} ");
            }
            Console.Write(" |\n");
        }
        public uint[] ReserveMemory(){
            uint[] array = new uint[16];
            uint count = 0;
            uint i = 0;
            while(count < 15 || i < 80){
                if(IsBlockEmpty(i)){
                    array[count] = i;
                    count++;
                }
                i++;
            }
            if(count < 15){
                throw new Exception ($"not enought free memory");
            }
            return array;
        }
        public void FreeMemory(uint[] array){
            for(int i = 0; i < 16; i++){
                for(int y = 0; y < 16; y++){
                    m_data[16*(int)array[i]+y].SetValue(0);
                }
            }
        }
        public bool IsBlockEmpty(uint block){
            for(int i = 0; i < 16; i++){
                if(m_data[16*(int)block+i].ToInt32() == 0){
                    return false;
                }
            }
            return true;
        }
        public void PrintMemoryChars()
        {
            const int spaceSeparatorInt = 1;
            const int newLineSeparatorInt = 40;
            for (var i = 0; i < m_data.Count; i++)
            {
                if(i % spaceSeparatorInt == 0 && i != 0)
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

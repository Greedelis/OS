using System;

namespace OS {
        class word{
        private byte[] wordd = new byte[4]{0, 0, 0, 0};

        public UInt32 toInt32(){
            return (UInt32)Math.Pow(16,6) * wordd[0] + (UInt32)Math.Pow(16,4) * wordd[1] + (UInt32)Math.Pow(16,2) * wordd[2] + wordd[3];
        }
        public String toString(){
            char[] temp_arr = {(char)wordd[0],(char)wordd[1],(char)wordd[2],(char)wordd[3]};
            return new string(temp_arr);
        }
        public void setValue(UInt32 newVal){
            wordd[0] = (byte)(newVal/((UInt32)Math.Pow(16,6)));
            newVal -= wordd[0] * (UInt32)Math.Pow(16,6);
            wordd[1] = (byte)(newVal/((UInt32)Math.Pow(16,4)));
            newVal -= wordd[1] * (UInt32)Math.Pow(16,4);
            wordd[2] = (byte)(newVal/((UInt32)Math.Pow(16,2)));
            wordd[3] = (byte)(newVal - wordd[2] * (UInt32)Math.Pow(16,2));
        }
        public void setValue(string newVal){
            int leng = newVal.Length;
            if(leng >= 4){
                for(int i = 0; i < 4; i++){
                    wordd[i] = (byte)newVal[i];
                }
            }else if(leng < 4){
                for(int i = 0; i < leng; i++){
                    wordd[3-i] =  (byte)newVal[i];
                }
                for(int y = leng-1; y < 4; y++){
                    wordd[y]=0;
                }
            }
        }
        public void printValues(){
            foreach (var item in wordd){
                Console.Write("{0} ", item);
            }
            Console.WriteLine();
        }
        public void printCharValues(){
            foreach (var item in wordd){
                Console.Write("{0} ", (char)item);
            }
            Console.WriteLine();
        }
    }
    public class Memory {
        private static int wordsPerBlock = 16;
        private static int blocks = 80;
        private word[] m_data = new word[wordsPerBlock*blocks]; // I don't fucking know
        private int memoryPointer = 0;

        public bool ChangeMemoryPointer(int newPointer)
        {
            if (newPointer < 0 && newPointer >= m_data.Length) //add conditions here I guess
                return false;
            memoryPointer = newPointer;
            return true;
        }
        
        public uint GetFromMemory(int pointer)
        {
            return m_data[pointer].toInt32();
        }
        
        public void PutToMemory(int pointer, uint data)
        {
            m_data[pointer].setValue(data);
        }
        
        public void PrintMemory()
        {
            var spaceSeparatorInt = 4;
            var newLineSeparatorInt = 80;
            for (var i = 0; i < m_data.Length; i++)
            {
                if(i % spaceSeparatorInt == 0)
                    Console.Write(" ");
                if(i % newLineSeparatorInt == 0)
                    Console.Write("\n");
                Console.Write(m_data[i]);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace OS {
    public class CentralCPU{

        private bool OF = false;
        private bool CF = false;
        private bool ZF = false;

        private bool MODE = false;

        private uint AX = 0;
        private uint BX = 0;

        private int TI = 0;
        private int PI = 0;
        private int SI = 0;

        private uint[] PTR = new uint[4];

        private readonly Memory m_memory = new Memory(); // Idk how this will work, want to reuse same class for VM's
        
        public void Test() { // Checks for interupts
            if (TI == 0) {
                TimerInterupt();
            }


        }

        public void TimerInterupt() {

        }

        //------------------------------------------------------------------- Palyginimas
        public void CMP(){ 
            Int64 temp = (Int64)AX - (Int64)BX;
            if (temp == 0){
                ZF = true;
                CF = false;
            }
            else if(temp < 0){
                ZF = false;
                CF = true;
            }
            else{
                ZF = false;
                CF = false;
            }
        }

    //------------------------------------------------------------------- Aritmetines
        public void ADD(){
            AX+=BX;   // jeigu AX overflowina, PI = 1, kuris reikštų perpildymą;
        }
        public void SUB(){
            AX-=BX; // jeigu AX underflowina, PI = 1, kuris reikštų perpildymą;
        }
        public void MUL(){
            AX*=BX; // jeigu AX overflowina, PI = 1, kuris reikštų perpildymą;
        }
        public void DIV(){
            if (BX == 0){
                PI = 0; // Meta programinį pertraukimą, kuris reiškia dalybą iš 0 (reik pataisyt dokumentaciją)
                return;
            }
            AX/=BX;
        }
        
        //-------------------------------------------------------------------Print memory

        public void MEMR()
        {
            m_memory.PrintMemory();
        }
        
        //-------------------------------------------------------------------


        //------------------------------------------------------------------- Darbas su Duomenimis
        /* TODO:
        •	LAx1x2 – į registrą AX užkrauna žodį iš adreso 16 * x1 + x2
        •	SAx1x2 – iš registro AX užkrauna žodį į adresą 16 * x1 + x2
        •	PRx1x2 – atspausdinama į ekraną 16 * x1 + x2 adrese esantis žodis
        •	PAx1x2 – atspausdinami visi simboliai į ekraną pradedant adresu 16 * x1 + x2 iki simbolio $
        •	RDAX – nuskaitoma iš klaviatūros reikšmė ir padedama AX
        •	RDx1x2 – nuskaitoma iš klaviatūros reikšmė ir padedama adrese 16 * x1 + x2
        •	RDAx1 – konstanta x1 užkraunama į registrą AX
        •	SDx1x2 data$ - įdeda pradedant adresu 16 * x1 + x2 visą data, kuri baigiasi simboliu $ (store data)
        */

        public void LA(int x1, int x2)
        {
            AX = m_memory.GetFromMemory(16 * x1 + x2);
        }

        public void PRAX(){
            Console.WriteLine(AX);
        }

        public void RDAX()
        {
            uint.TryParse(Console.ReadLine(), out var value);
            AX = value;
        }

        public void PR(int x1, int x2)
        {
            Console.WriteLine(m_memory.GetFromMemory(16 * x1 + x2));
        }
        
        public void PA (int x1, int x2)
        {
            var i = 0;
            while (true)
            {
                var buffer = m_memory.GetFromMemory(16 * x1 + x2 + i);
                var word = new Word(buffer);
                var substring = word.IntoString();
                Console.Write(substring);
                if (substring.Contains('$')) 
                    break;
                i++;
            }
            Console.Write('\n');
        }
        
        public void SD (int x1, int x2, string data)
        {
            var word = new Word();
            var padCount = 0;
            if(data.Length % 4 != 0)
                padCount = 4 - data.Length % 4;
            data = data.PadRight(data.Length + padCount, (char)0);
            var count = 0;
            while (count < data.Length)
            {
                word.SetValue(data.Substring(count,4));
                m_memory.PutToMemory(16 * x1 + x2 + count/4, word.ToInt32());
                count += 4;
            }
        }

        public void SA(int x1, int x2)
        {
            m_memory.PutToMemory(16 * x1 + x2, AX);
        }

        public void RD (int x1, int x2) 
        {
            uint.TryParse(Console.ReadLine(), out var value);
            m_memory.PutToMemory(16 * x1 + x2, value);
        }

        public void RDA (int x1)
        {
            AX = (uint)x1;
        }

        public void SWAP(){
            (AX,BX) = (BX, AX);  // Sakyčiau visai cool swapas apsirašo
        }
        
        //------------------------------------------------------------------- Jump'ai, Not sure are correct mano mastymas cia
        
        //nesąlyginis jump į komandą adresu 16 * x + y
        public void JP(int x, int y)
        {
            m_memory.ChangeMemoryPointer(16 * x + y);
        }

        //JMxy – Jump More, jei AX > BX (ZF ir CF yra 0), šoka į komandą adresu 16 * x + y
        public void JM(int x, int y)
        {
            if (!ZF && !CF) //su flagais comparint ar AX > BX? // Yes, CMP paskutinis else tą padaro (pirmas if ==, antras AX < BX, paskutinis AX > BX)
            {
                m_memory.ChangeMemoryPointer(16 * x + y);
            }
        }

        //JLxy – Jump Less, jei AX < BX (CF = 1),  šoka į komandą adresu 16 * x + y
        public void JL(int x, int y)
        {
            if (CF)
            {
                m_memory.ChangeMemoryPointer(16 * x + y);
            }
        }
        
        //JExy – Jump Equal, jei AX = BX (ZF = 1),  šoka į komandą adresu 16 * x + y
        public void JE(int x, int y)
        {
            if (ZF)
            {
                m_memory.ChangeMemoryPointer(16 * x + y);
            }
        }
        
        //JNxy – Jump Not Equal, jei AX != BX (ZF = 0), šoka į komandą adresu 16 * x + y
        public void JN(int x, int y)
        {
            if (!ZF)
            {
                m_memory.ChangeMemoryPointer(16 * x + y);
            }
        }
        
        //JXxy – Jump More or Equal, jei AX >= BX, šoka į komandą adresu 16 * x + y
        public void JX(int x, int y)
        {
            if ((!ZF && !CF) || ZF) //fixed
            {
                m_memory.ChangeMemoryPointer(16 * x + y);
            }
        }
        
        //JYxy – Jump Less or Equal, jei AX <= BX, šoka į komandą adresu 16 * x + y
        public void JY(int x, int y)
        {
            if (CF || ZF) // fixed
            {
                m_memory.ChangeMemoryPointer(16 * x + y);
            }
        }
    //-------------------------------------------------------------------

    
    public void HALT() //what does halt even do? closes the machine, resets it??? 
    {
        Environment.Exit(0);
    }

    

    /* TODO:
    •	HALT – programos valdymo pabaiga
    •	SDx1x2 data$ - įdeda pradedant adresu 16 * x1 + x2 visą data, kuri baigiasi simboliu $ (store data)

    

    */

    }
}

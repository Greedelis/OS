using System;

namespace OS {
    public class CentralCPU {

        private static readonly CentralCPU m_instance = new CentralCPU();
        private readonly Parser m_parser;

        public static CentralCPU Instance() => m_instance;

        private CentralCPU() {
            m_parser = new Parser(this);
        }

        private bool OF = false;
        private bool CF = false;
        private bool ZF = false;

        private bool MODE = false;

        private uint AX = 0;
        private uint BX = 0;

        private int TI = 150;
        private int PI = 0;
        private int SI = 0;

        private uint[] PTR = new uint[4];

        private readonly Memory m_memory = new Memory(); // Idk how this will work, want to reuse same class for VM's
        private SoftDisk hard = new SoftDisk(); //not sure ar hardas cia, jei ka galesim movint


        public VM CreateVM() {
            return new VM(this, m_parser, m_memory, hard, m_memory.ReserveMemory());
        }

        public void Test() { // Checks for interupts
            if (TI <= 0)
                TimerInterupt();

            switch (SI) {
                case 0: // Normal value
                    break;
                case 1: // Prints AX
                    Console.WriteLine(AX);
                    break;
                case 2: // Prints BX
                    Console.WriteLine(BX);
                    break;
            }

            
        }

        public void TimerInterupt() {
            Console.WriteLine("Timer is 0!");
            TI = 150;
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

            TI--;
        }

    //------------------------------------------------------------------- Aritmetines
        public void ADD(){
            try{AX+=BX; }
            catch(OverflowException){
                OF = true;
            }
            
        }
        public void SUB(){
            try{AX-=BX; }
            catch(OverflowException){
                OF = true;
            }
        }
        public void MUL(){
            try{AX*=BX; }
            catch(OverflowException){
                OF = true;
            }
        }
        public void DIV(){
            if (BX == 0){
                PI = 0; 
                return;
            }
            AX/=BX;
        }
        
        //-------------------------------------------------------------------Print memory

        public void MINT()
        {
            m_memory.PrintMemoryInts();
        }
        public void MCHR()
        {
            m_memory.PrintMemoryChars();
        }
        
        //-------------------------------------------------------------------
        
        //------------------------------------------------------------------- Darbas su hard drive
        public void CHDR(string folder) //Change dir (cd)
        {
            hard.ChangeDir(folder.Remove(folder.Length-1));
        }
        
        public void SDIR() //ShowDir (dir)
        {
            foreach (var directory in hard.GetAllDirectories())
            {
                Console.WriteLine(directory);
            }
            foreach (var file in hard.GetAllFileInfo())
            {
                Console.WriteLine($"{file.Name} | {file.Length} Bytes");
            }
            Console.WriteLine();
        }


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
            AX = m_memory.GetFromMemory(x1, x2);
        }

        public void PRAX(){
            Console.WriteLine(AX);
        }
        
        public void PRBX(){
            Console.WriteLine(BX);
        }

        public void RDAX()
        {
            uint.TryParse(Console.ReadLine(), out var value);
            AX = value;
        }
        
        public void RDBX()
        {
            uint.TryParse(Console.ReadLine(), out var value);
            BX = value;
        }

        public void PR(int x1, int x2)
        {
            Console.WriteLine(m_memory.GetFromMemory(x1, x2));
        }
        
        public void PA (int x1, int x2)
        {
            var i = 0;
            while (true)
            {
                var buffer = m_memory.GetFromMemory(x1, x2 + i);
                var word = new Word(buffer);
                var substring = word.IntoString();
                if (substring.Contains('$'))
                {
                    substring = substring.Replace('$', (char)0);
                    Console.Write(substring);
                    break;
                }
                Console.Write(substring);
                    
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
                m_memory.PutToMemory(x1, x2 + count/4, word.ToInt32());
                count += 4;
            }
        }

        public void SA(int x1, int x2)
        {
            m_memory.PutToMemory(x1, x2, AX);
        }

        public void RD (int x1, int x2) 
        {
            uint.TryParse(Console.ReadLine(), out var value);
            m_memory.PutToMemory(x1, x2, value);
        }

        public void RDA (int x1)
        {
            AX = (uint)x1;
        }

        public void SWAP(){
            (AX,BX) = (BX, AX);  // Sakyčiau visai cool swapas apsirašo
        }

        /** Jumps **/

        // Simple jump
        public void JP(int x, int y) {
            m_memory.SetMemoryPointer(x, y);
        }

        // Jump More, jei AX > BX (ZF ir CF yra 0)
        public void JM(int x, int y) {
            if (!ZF && !CF)
                m_memory.SetMemoryPointer(x, y);
        }

        // Jump Less, jei AX < BX (CF = 1)
        public void JL(int x, int y) {
            if (CF)
                m_memory.SetMemoryPointer(x, y);
        }
        
        // Jump Equal, jei AX = BX (ZF = 1)
        public void JE(int x, int y) {
            if (ZF)
                m_memory.SetMemoryPointer(x, y);
        }
        
        // Jump Not Equal, jei AX != BX (ZF = 0)
        public void JN(int x, int y) {
            if (!ZF)
                m_memory.SetMemoryPointer(x, y);
        }
        
        // Jump More or Equal, jei AX >= BX
        public void JX(int x, int y) {
            if ((!ZF && !CF) || ZF)
                m_memory.SetMemoryPointer(x, y);
        }
        
        // Jump Less or Equal, jei AX <= BX
        public void JY(int x, int y) {
            if (CF || ZF)
                m_memory.SetMemoryPointer(x ,y);
        }

    //-------------------------------------------------------------------

    
        public void HALT() //what does halt even do? closes the machine, resets it??? 
        {
            Environment.Exit(0);
        }

    }
}

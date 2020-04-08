using System;

namespace OS {
    public class CentralCPU{
    
        public CentralCPU(){  // costructor, duh
        }
        private Boolean OF = false, CF = false, ZF = false, // SF's  gal galima ir kitaip aprašyt :D
                        MODE = false; // 0 if user, 1 if supervisor
        private int AX, BX; //registers
        private uint TI, PI, SI; // Timer register, Programing Interup register and Supervisor Interupt register (paskutinius du reik apsirašyt dokumentacijoje normaliai)
        private uint[] PTR = new uint[4];


    //------------------------------------------------------------------- Palyginimas
        private void CMP(){ 
            int temp = AX - BX;
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


        //------------------------------------------------------------------- Darbas su Duomenimis
        /* TODO:
        •	LAx1x2 – į registrą AX užkrauna žodį iš adreso 16 * x1 + x2
        •	SAx1x2 – iš registro AX užkrauna žodį į adresą 16 * x1 + x2
        •	PRx1x2 – atspausdinama į ekraną 16 * x1 + x2 adrese esantis žodis
        •	PAx1x2 – atspausdinami visi simboliai į ekraną pradedant adresu 16 * x1 + x2 iki simbolio $
        •	RDAX – nuskaitoma iš klaviatūros reikšmė ir padedama AX
        •	RDx1x2 – nuskaitoma iš klaviatūros reikšmė ir padedama adrese 16 * x1 + x2
        •	RDAx1 – konstanta x1 užkraunama į registrą AX
        */

        public void LA(int x1, int x2) {

        }

        public void PRAX(){
            Console.WriteLine(AX);
        }
        public void SWAP(){
            (AX,BX) = (BX, AX);  // Sakyčiau visai cool swapas apsirašo
        }
    //------------------------------------------------------------------- Jump'ai
    /* TODO:
    •	Jump‘ai: 
        *	JPxy – nesąlyginis jump į komandą adresu 16 * x + y
        *	JMxy – Jump More, jei AX > BX (ZF ir CF yra 0), šoka į komandą adresu 16 * x + y
        *	JLxy – Jump Less, jei AX < BX (CF = 1),  šoka į komandą adresu 16 * x + y
        *	JExy – Jump Equal, jei AX = BX (ZF = 1),  šoka į komandą adresu 16 * x + y
        *	JNxy – Jump Not Equal, jei AX != BX (ZF = 0), šoka į komandą adresu 16 * x + y
        *	JXxy – Jump More or Equal, jei AX >= BX, šoka į komandą adresu 16 * x + y
        *	JYxy – Jump Less or Equal, jei AX <= BX, šoka į komandą adresu 16 * x + y
    •	HALT – programos valdymo pabaiga
    •	SDx1x2 data$ - įdeda pradedant adresu 16 * x1 + x2 visą data, kuri baigiasi simboliu $ (store data)

    */

    }
}

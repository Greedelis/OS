using System;
public class CentralCPU{
    
    public CentralCPU(){  // costructor, duh
    }
    private Boolean OF = false, CF = false, ZF = false, // SF's  gal galima ir kitaip aprašyt :D
                    MODE = false; // 0 if user, 1 if supervisor
    private int AX, BX; //registers
    private uint TI; // Timer register
    private uint[] PTR = new uint[4];

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

    public void ADD(){
        AX+=BX;
    }
    public void SUB(){
        AX-=BX;
    }
    public void MUL(){
        AX*=BX;
    }
    public void DIV(){
        if (BX == 0){
            ZF = true;
            return;
        }
        AX/=BX;
    }
}
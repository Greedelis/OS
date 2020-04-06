using System;
public class CentralCPU{
    
    public CentralCPU(){  // costructor, duh
        OF = false;
        CF = false;
        ZF = false;
        MODE = false;
    }
    private Boolean OF, CF, ZF, // SF's  gal galima ir kitaip aprašyt :D
                    MODE; // 0 if user, 1 if supervisor
    private int AX, BX; //registers
    private uint TI; // Timer register
    private uint[] PTR = new uint[4];
}
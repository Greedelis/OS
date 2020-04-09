﻿using System;

namespace OS {
    public class Memory {

        private int[] m_data = new int[16 * 16 * 80]; // I don't fucking know
        private int MemoryPointer = 0;

        public bool ChangeMemoryPointer(int newPointer)
        {
            if (newPointer < 0) //add conditions here I guess
                return false;
            MemoryPointer = newPointer;
            return true;
        }
        public void printMemory()
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

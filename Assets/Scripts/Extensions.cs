using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Extensions
{
    public static int TMProNonAlloc(this int self, char[] output, int start=0)
    {
        int num = Math.Abs(self);
        int digitsNum = (int)System.Math.Log10(num) + 1;
        int zero = '0';
        if (self == 0)
        {
            output[0] = '0';
            return 1;
        }
        if (self < 0)
        {
            output[0] = '-';
            digitsNum++;
            //start++;
        }

        for(int i=digitsNum-1; i>=0; i--)
        {
            int digit = num % 10;
            output[start + i] = (char)(digit + zero);
            num /= 10;
        }
        if (self < 0)
        {
            output[0] = '-';
            //start++;
        }
        //Debug.Log(new string(output));
        return digitsNum;
    }
}

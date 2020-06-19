using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static public void SetZero(BigNumber mm)
        {
            mm.signum = 0;
            mm.mantissa = new byte[1];
            mm.mantissa[0] = 0;
            mm.exponent = 0;
            mm.dataLength = 1;
        }
        
        static private void SetFromDouble(BigNumber atm, double doubleValue)
        {            
            BigNumber.SetFromString(atm, "" + doubleValue);
        }

        static private void SetFromLong(BigNumber atm, long mm)
        {             
            if (mm == 0)
            {
                BigNumber.SetZero(atm);
                return;
            }

            String ascii_number = mm.ToString();

            atm.signum = 1;

            if (mm < 0)
            {
                atm.signum = -1;                
                ascii_number = ascii_number.Replace("-", "");
            }
           
            atm.exponent = ascii_number.Length;

            if ((atm.exponent % 2) != 0)
            {
                // append a 0 to  least significant nibble in case of odd length    
                ascii_number += '0';                       
            }

            int nbytes = (atm.exponent + 1) >> 1;    // we display 2 digits per byte

            // allocate data array
            atm.mantissa = new byte[atm.exponent + 1];

            atm.dataLength = atm.exponent;
           
            for (int ii = 0, p = 0; ii < nbytes; ii++)
            {
                byte ch = (byte)(ascii_number[p++] - '0');
                atm.mantissa[ii] = (byte)((byte)(10 * ch) + (byte)(ascii_number[p++] - '0'));
            }
        }

        static private void InitializehexLookup()
        {
            if (hexLookup.Count == 0)
            {
                hexLookup['0'] = (int)0;
                hexLookup['1'] = (int)1;
                hexLookup['2'] = (int)2;
                hexLookup['3'] = (int)3;
                hexLookup['4'] = (int)4;
                hexLookup['5'] = (int)5;
                hexLookup['6'] = (int)6;
                hexLookup['7'] = (int)7;
                hexLookup['8'] = (int)8;
                hexLookup['9'] = (int)9;
                hexLookup['a'] = (int)10;
                hexLookup['b'] = (int)11;
                hexLookup['c'] = (int)12;
                hexLookup['d'] = (int)13;
                hexLookup['e'] = (int)14;
                hexLookup['f'] = (int)15;
            }
        }

        static private void SetFromHexString(  BigNumber atm, String hexvalue)
        {
            int length = hexvalue.Length-1;
            BigNumber CurValue = new BigNumber();
            BigNumber.SetZero(CurValue);

            BigNumber CurDigitBaseValue = 1;
            InitializehexLookup();

            try
            {
                while (length >= 0)
                {
                    char digit = hexvalue[length];
                    int value = (int)hexLookup[digit];
                    CurValue +=  (CurDigitBaseValue * value);
                    CurDigitBaseValue *=16;
                    length--;
                }

                BigNumber.Copy(CurValue, atm);
               
            }
            catch
            {
                // illegal input string
                throw new BigNumberException("illegal input string");
            }
        }

        static private void SetFromBinaryString(BigNumber atm, String binvalue)
        {
            int length = binvalue.Length - 1;
            BigNumber CurValue = new BigNumber();
            BigNumber.SetZero(CurValue);

            BigNumber CurDigitBaseValue = 1;
            InitializehexLookup();

            try
            {
                while (length >= 0)
                {
                    char digit = binvalue[length];
                    int value = (int)hexLookup[digit];
                    CurValue += (CurDigitBaseValue * value);
                    CurDigitBaseValue *= 2;
                    length--;
                }

                BigNumber.Copy(CurValue, atm);

            }
            catch
            {
                // illegal input string
                throw new BigNumberException("illegal input string");
            }
        }

        static private void SetFromString(BigNumber atm, String value)
        {
           
            SetZero(atm);

            int p = 0;
            sbyte sign = 1;
            int exponent = 0;

            value = value.Trim();               // trim whitespace
            value = value.ToLower();            // convert to lower

            int cp = value.IndexOf("0x");       // hexadezimalnumber
            if (cp >= 0)
            {
                value = value.Substring(cp + 2, value.Length - (cp + 2));
                BigNumber.SetFromHexString(  atm, value);
                return;
            }

            cp = value.IndexOf("%");       // binary format number
            if (cp >= 0)
            {
                value = value.Substring(cp + 1, value.Length - (cp + 1));
                BigNumber.SetFromBinaryString(atm, value);
                return;
            }

            cp = value.IndexOf("e");
            if (cp > 0) // e cannot be leading
            {
                String ex = value.Substring(cp + 1, value.Length - (cp + 1));
                exponent = Convert.ToInt32(ex);
                value = value.Substring(0, cp);
            }


            if (value.Contains("+"))
            {
                value = value.Replace("+", ""); // remove optional '+' character
            }
            else if (value.Contains("-"))       
            {
                sign = -1;
                value = value.Replace("-", "");
            }
                         
            value = value.Replace(",", ".");

            int j = value.IndexOf(".");
            if (j == -1)
            {
                value = value+'.';
                j = value.IndexOf(".");
            }

            if (j > 0)
            {
                exponent += j;
                value = value.Replace(".", "");
            }

           
            int i = value.Length;
            atm.dataLength = i;

            if ((i % 2) != 0)
            {
                value =  value + '0';
            }

            j = value.Length >> 1;

            if (value.Length > atm.mantissa.Length)
            {
                BigNumber.Expand(atm, atm.dataLength + 28);
            }

           
            byte ch = 0;

            int zflag = 1;
            for (i = 0, p = 0; i < j; i++)
            {
                ch = (byte)(value[p++] - '0');

                if ((ch = (byte)((byte)(10 * ch) + (byte)(value[p++] - '0'))) != 0)
                {
                    zflag = 0;
                }

                if (((int)ch & 0xFF) >= 100)
                {
                    // Error!
                    SetZero(atm);
                    return;
                }

                atm.mantissa[i] = ch;
                atm.mantissa[i + 1] = 0;
            }

            atm.exponent = exponent;
            atm.signum = sign;

            if (zflag != 0)
            {
                atm.exponent = 0;
                atm.signum = 0;
                atm.dataLength = 1;
                atm.mantissa[0] = 0;
            }
            else
            {
                Normalize(atm);
            }
        }
    }
}
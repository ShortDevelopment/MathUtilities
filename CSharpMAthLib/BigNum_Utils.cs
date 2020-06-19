using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        public class BigNumberException : Exception
        {
            public BigNumberException()
            {

            }

            public BigNumberException(String msg) : base(msg)
            {

            }
        }

        static readonly int sMinPrecision = 10000;

        static int IsInteger(BigNumber atmp)
        {
            if (atmp.signum == 0)
                return (1);

            if (atmp.exponent >= atmp.dataLength)
                return (1);
            else
                return (0);
        }

        static int GetSizeofInt()
        {
            return (sizeof(int));
        }

        static int NextPowerOfTwo( int n )
        {
            int ct, k;
            int size_flag = GetSizeofInt();
            int bit_limit = 8 * size_flag + 1;

            if (n <= 2)
                return (n);

            k = 2;
            ct = 0;

            while (true)
            {
                if (k >= n)
                    break;

                k = k << 1;

                if (++ct == bit_limit)
                {
                    throw (new BigNumberException(" 'NextPowerOfTwo ', ERROR :sizeof(int) too small  "));                     
                }
            }

            return (k);

        }
        static private int SignificantDigits(BigNumber atm)
        {
            return atm.dataLength;
        }

        static private int Digits(BigNumber atm)
        {
            return atm.dataLength < sMinPrecision ? sMinPrecision : atm.dataLength;
        }

        static private int MaxDigits(BigNumber a, BigNumber b)
        {
            if (Digits(a) < Digits(b))
            {
                return Digits(b);
            }
            else
            {
                return Digits(a);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctmp"></param>
        /// <param name="count"></param>
        static private void Scale(BigNumber ctmp, int count)
        {
            int ii, numb, ct;
            ct = count;
            byte numdiv=0, numdiv2 = 0, numrem = 0;

            ii = (ctmp.dataLength + ct + 1) >> 1;

            if (ii > ctmp.mantissa.Length)
            {
                BigNumber.Expand(ctmp, (ii + 32));                 
            }

            if ((ct & 1) != 0)          /* move odd number first */
            {
                ct--;
                
                ii = ((ctmp.dataLength + 1) >> 1) - 1;

                if ((ctmp.dataLength & 1) == 0)
                {
                    /*
                     *   original datalength is even:
                     *
                     *   uv  wx  yz   becomes  -->   0u  vw  xy  z0
                     */

                    numdiv = 0;

                    while (true)
                    {
                        Unpack(ctmp.mantissa[ii], ref  numdiv2, ref numrem);

                        ctmp.mantissa[ii + 1] = (byte)(10 * numrem + numdiv);
                        numdiv = numdiv2;

                        if (ii == 0)
                        {
                            break;
                        }

                        ii--;
                    }

                    ctmp.mantissa[0] = numdiv2;
                }
                else
                {
                    /*
                     *   original datalength is odd:
                     *
                     *   uv  wx  y0   becomes  -->   0u  vw  xy
                     */

                    Unpack(ctmp.mantissa[ii], ref  numdiv2, ref numrem);
                  
                    if (ii == 0)
                    {
                        ctmp.mantissa[0] = numdiv2;
                    }
                    else
                    {
                        while (true)
                        {
                            Unpack(ctmp.mantissa[ii-1], ref  numdiv, ref numrem);

                            ctmp.mantissa[ii] = (byte)(10 * numrem + numdiv2);
                            numdiv2 = numdiv;

                            if (--ii == 0)
                            {
                                break;
                            }
                        }

                        ctmp.mantissa[0] = numdiv;
                    }
                }

                ctmp.exponent++;
                ctmp.dataLength++;
            }

            /* ct is even here */

            if (ct > 0)
            {
                numb = (ctmp.dataLength + 1) >> 1;
                ii = ct >> 1;

                byte[] newData = new byte[ctmp.mantissa.Length];
                Array.Copy(ctmp.mantissa, 0, newData,ii, numb);

                for (int i = 0; i < ii; i++)
                {
                    newData[i] = 0;   
                }

                ctmp.mantissa = newData;
              
                ctmp.dataLength += ct;
                ctmp.exponent += ct;
            }
            
        }
                
        /// <summary>
        /// expand mantissa array with given length and copy old content
        /// throws: ArgumentOutOfRangeException
        /// </summary>
        /// <param name="atm"></param>
        /// <param name="newLength"></param>
        static public void Expand(BigNumber atm, int newLength)
        {
            if (newLength > atm.mantissa.Length)
            {
                byte[] newdata = new byte[newLength];
                Array.Copy(atm.mantissa, newdata, atm.mantissa.Length);
                atm.mantissa = newdata;
            }
            else
            {
                throw new BigNumberException("'Expand', newLength smaller than current length");
            }
        }

        /// <summary>
        /// unpacks the msb and lsb nibbles of a packed byte
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="ndiv"></param>
        /// <param name="nrem"></param>
        static private void Unpack(byte packed, ref byte msb, ref byte lsb)
        {
            msb = s_MsbLookup[packed%100];
            lsb = s_LsbLookup[packed%10];
        }

        static private void UnpackMult(int tbl_lookup, ref byte msb, ref byte lsb)
        {
            msb = s_MsbLookupMult[tbl_lookup];
            lsb= s_LsbLookupMult[tbl_lookup];
        }
         
        static private void Pad(BigNumber atm, int length)
        {
            int ct = length;
            byte numdiv = 0, numref = 0;

            if (atm.dataLength >= ct)
            {
                return;
            }

            int numb = (ct + 1) >> 1;

            if (numb > atm.mantissa.Length)
            {
                BigNumber.Expand(atm, numb + 32);
            }

            int num1 = (atm.dataLength + 1) >> 1;

            if ((atm.dataLength % 2) != 0)
            {
                Unpack(atm.mantissa[num1 - 1], ref numdiv, ref numref);
                atm.mantissa[num1 - 1] = (byte)(10 * numdiv);
            }

            for (int i = num1; i < numb; i++)
            {
                atm.mantissa[i] = 0;
            }

            atm.dataLength = ct;

        }

        /// <summary>
        /// normalize number
        /// </summary>
        /// <param name="atm"></param>
        static private void Normalize(BigNumber atm)
        {
            if (atm.signum == 0)
            {
                return;
            }
            int ucp = 0;
            int i;
            int index;
            int datalength = atm.dataLength;
            int exponent = atm.exponent;
            byte numdiv = 0, numrem = 0, numrem2 = 0;

            BigNumber.Pad(atm, datalength + 3);

            // remove leading zeroes
            while (true)
            {
                // extract first 2 nibbles
                Unpack(atm.mantissa[0], ref numdiv, ref numrem);

                // if first digit is greater 1 we are done
                if (numdiv >= 1)
                {
                    break;
                }

                // otherwise we have leading zeroes
                index = (datalength + 1) >> 1;

                if (numrem == 0)    // both nibbles are 0
                {
                    i = 0;
                    ucp = 0;

                    while (true)
                    {
                        if (atm.mantissa[ucp] != 0)
                        {
                            break;
                        }
                        ucp++;
                        i++;
                    }

                    byte[] copy = new byte[atm.mantissa.Length];
                    Array.Copy(atm.mantissa, ucp, copy, 0, (index + 1 - i));
                    atm.mantissa = copy;

                    datalength -= 2 * i;
                    exponent -= 2 * i;
                }
                else
                {
                    for (i = 0; i < index; i++)
                    {
                        Unpack(atm.mantissa[i + 1], ref numdiv, ref numrem2);
                        atm.mantissa[i] = (byte)(10 * numrem + numdiv);
                        numrem = numrem2;
                    }

                    datalength--;
                    exponent--;
                }
            }

            // remove trailing zeroes
            while (true)
            {
                index = ((datalength + 1) >> 1) - 1;

                if ((datalength & 1) == 0)   /* back-up full bytes at a time if the */
                {				            /* current length is an even number    */
                    ucp = index;
                    if (atm.mantissa[ucp] == 0)
                    {
                        while (true)
                        {
                            datalength -= 2;
                            index--;
                            ucp--;

                            if (atm.mantissa[ucp] != 0)
                            {
                                break;
                            }
                        }
                    }
                }

                Unpack(atm.mantissa[index], ref numdiv, ref numrem);

                if (numrem != 0)		/* last digit non-zero, all done */
                    break;

                if ((datalength & 1) != 0)   /* if odd, then first char must be non-zero */
                {
                    if (numdiv != 0)
                        break;
                }

                if (datalength == 1)
                {
                    atm.signum = 0;
                    exponent = 0;
                    break;
                }

                datalength--;
            }

            atm.dataLength = datalength;
            atm.exponent = exponent;
        }

       
    }

}
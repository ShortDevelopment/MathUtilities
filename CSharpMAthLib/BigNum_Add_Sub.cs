using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {        
        static public void Add(BigNumber src, BigNumber dst, BigNumber result)
        {
            int j, carry, aexp, bexp, adigits, bdigits;
            sbyte sign;
            BigNumber A=0,B=0;
            
            if (src.signum == 0)
            {
                BigNumber.Copy(dst, result);
                return;
            }

            if (dst.signum == 0)
            {
                BigNumber.Copy(src, result);
                return;
            }

            if (src.signum == 1 && dst.signum == -1)
            {
                dst.signum = 1;
                Sub(src, dst, result);
                dst.signum = -1;
                return;
            }

            if (src.signum == -1 && dst.signum == 1)
            {
                src.signum = 1;
                Sub(dst, src, result);
                src.signum = -1;
                return;
            }

            sign = src.signum;
            aexp = src.exponent;
            bexp = dst.exponent;

            Copy(src, A);
            Copy(dst, B);

            if (aexp == bexp)
            {
                // scale (shift) 2 digits                
                BigNumber.Scale(A, 2);  
                BigNumber.Scale(B, 2);
            }
            else
            {
                // scale to larger exponent
                if (aexp > bexp)
                {
                    BigNumber.Scale(A, 2);
                    BigNumber.Scale(B, (aexp - bexp  + 2));
                }
                else
                {
                    BigNumber.Scale(B, 2);
                    BigNumber.Scale(A, (bexp - aexp + 2));
                }
            }

            adigits = A.dataLength;
            bdigits = B.dataLength;

            if (adigits >= bdigits)
            {
                Copy(A, result);

                j = (bdigits + 1) >> 1;

                carry = 0;

                while (true)
                {
                    j--;
                    result.mantissa[j] += (byte)(carry + B.mantissa[j]);

                    if (result.mantissa[j] >= 100)
                    {
                        result.mantissa[j] -= 100;
                        carry = 1;
                    }
                    else
                    {
                        carry = 0;
                    }

                    if (j == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                Copy(B, result);

                j = (adigits + 1) >> 1;

                carry = 0;

                while (true)
                {
                    j--;
                    result.mantissa[j] += (byte)(carry + A.mantissa[j]);

                    if (result.mantissa[j] >= 100)
                    {
                        result.mantissa[j] -= 100;
                        carry = 1;
                    }
                    else
                    {
                        carry = 0;
                    }

                    if (j == 0)
                    {
                        break;
                    }
                }
            }

            result.signum = sign;
            Normalize(result);
        }
        
        static public void Sub(BigNumber src, BigNumber dst, BigNumber result)
        {
            int itmp, j, ChangeOrderFlag, icompare,  aexp, bexp,  borrow, adigits, bdigits;
            sbyte sign;
            BigNumber A = 0, B = 0;

            if (dst.signum == 0)
            {
                BigNumber.Copy(src,result);
                return;
            }

            if (src.signum == 0)
            {
                BigNumber.Copy(dst,result);
                return;
            }

            if (src.signum == 1 && dst.signum == -1)
            {
                dst.signum = 1;
                Add(src, dst, result);
                dst.signum = -1;
                return;
            }

            if (src.signum == -1 && dst.signum == 1)
            {
                dst.signum = -1;
                Add(src, dst, result);
                dst.signum = 1;
                return;
            }

            BigNumber.Abs(A, src);
            BigNumber.Abs(B, dst);

            if ((icompare = Compare(A, B)) == 0)
            {
                SetZero(result);
                return;
            }

            if (icompare == 1)  /*  |a| > |b|  (do A-B)  */
            {
                ChangeOrderFlag = 1;
                sign = src.signum;     
            }
            else                /*  |b| > |a|  (do B-A)  */
            {
                ChangeOrderFlag = 0;
                sign = (sbyte)-(src.signum); 
            }

            aexp =  A.exponent;
            bexp =  B.exponent;

            if (aexp > bexp)
            {
                Scale(B, (aexp - bexp));
            }
            if (aexp < bexp)
            {
                Scale(A, (bexp - aexp));
            }

            adigits = A.dataLength;
            bdigits = B.dataLength;

            if (adigits > bdigits)
            {
                Pad(B, adigits);
            }

            if (adigits < bdigits)
            {
                Pad(A, bdigits);
            }

            if (ChangeOrderFlag == 1)		 // |a| > |b|  (do A-B)
            {
                Copy( A, result);

                j = (result.dataLength + 1) >> 1;

                borrow = 0;

                while (true)
                {
                    j--;
                    itmp =  ((int)result.mantissa[j] - ((int) B.mantissa[j] + borrow));

                    if (itmp >= 0)
                    {
                        result.mantissa[j] = (byte)itmp;
                        borrow = 0;
                    }
                    else
                    {
                        result.mantissa[j] = (byte)(100 + itmp);
                        borrow = 1;
                    }

                    if (j == 0)
                        break;
                }
            }
            else   		// |b| > |a|  (do B-A) instead
            {
                Copy( B, result);

                j = (result.dataLength + 1) >> 1;
                borrow = 0;

                while (true)
                {
                    j--;
                    itmp = (int)result.mantissa[j] - ((int) A.mantissa[j] + borrow);

                    if (itmp >= 0)
                    {
                        result.mantissa[j] = (byte)itmp;
                        borrow = 0;
                    }
                    else
                    {
                        result.mantissa[j] = (byte)(100 + itmp);
                        borrow = 1;
                    }

                    if (j == 0)
                        break;
                }
            }

            result.signum = sign;

            BigNumber.Normalize(result);
        }

      
    }
}
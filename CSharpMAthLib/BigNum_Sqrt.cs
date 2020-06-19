using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static void	SQrtGuess( BigNumber a, BigNumber r)
        {
            /* sqrt algorithm actually finds 1/sqrt */
            double  dd;
            String buf = BigNumber.ToExpString(a,15);

            scci.NumberFormat.NumberDecimalSeparator = "."; 
              
            dd = Convert.ToDouble(buf,scci);                    
            BigNumber.SetFromDouble(r, (1.0 / Math.Sqrt(dd)));
        }

        static  void Sqrt(BigNumber src,  BigNumber dst, int places)
        {                         
            int ii,  nexp, tolerance, dplaces;
            bool bflag;

            if (src.signum <= 0)
            {
                if (src.signum == -1)
                {
                    throw new BigNumberException("'Sqrt',Invalid Argument");
                }
            }

            BigNumber last_x = new BigNumber();
            BigNumber guess = new BigNumber();
            BigNumber tmpN = new BigNumber();
            BigNumber tmp7 = new BigNumber();
            BigNumber tmp8 = new BigNumber();
            BigNumber tmp9 = new BigNumber();

            BigNumber.Copy(src, tmpN);
 
            nexp = src.exponent / 2;
            tmpN.exponent -= 2 * nexp;

            BigNumber.SQrtGuess( tmpN, guess );    

            tolerance = places + 4;
            dplaces = places + 16;
            bflag = false;

            BigNumber.Neg( BigNumber.Ten,last_x);
 
            ii = 0;

            while (true)
            {
                BigNumber.Mul( tmpN, guess,tmp9);
                BigNumber.Mul(tmp9, guess, tmp8);
                BigNumber.Round(tmp8, tmp7, dplaces);
                BigNumber.Sub( BigNumber.Three, tmp7,tmp9);
                BigNumber.Mul( tmp9, guess,tmp8);
                BigNumber.Mul( tmp8, BigNumber.BN_OneHalf,tmp9);

                if (bflag)
                {
                    break;
                }

                BigNumber.Round( tmp9,guess,dplaces);
             
                if (ii != 0)
                {
                    BigNumber.Sub( guess, last_x,tmp7);

                    if (tmp7.signum == 0)
                        break;

                    /* 
                     *   if we are within a factor of 4 on the error term,
                     *   we will be accurate enough after the *next* iteration
                     *   is complete.  (note that the sign of the exponent on 
                     *   the error term will be a negative number).
                     */

                    if ((-4 * tmp7.exponent) > tolerance)
                        bflag = true;
                }

                BigNumber.Copy( guess,last_x);
                ii++;
            } 

            BigNumber.Mul( tmp9, tmpN, tmp8);
            BigNumber.Round(tmp8,dst,places);
            dst.exponent += nexp;
             
        }
    }
}

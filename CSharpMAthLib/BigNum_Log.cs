using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static private int MM_lc_log_digits = 128;
      
        static void	M_get_log_guess( BigNumber a, BigNumber r)
        {
            double  dd;

            String buf = BigNumber.ToExpString(a,15 );
            dd = BigNumber.ExpStringToDouble(buf);          
            BigNumber.SetFromDouble(r, (1.00001 * Math.Log(dd)));        /* induce error of 10 ^ -5 */
        }

        static void M_log_solve_cubic(  BigNumber nn, BigNumber rr, int places)
        {
            BigNumber tmp0, tmp1, tmp2, tmp3, guess;
            int ii, maxp, tolerance, local_precision;

            guess = new BigNumber();
            tmp0 = new BigNumber();
            tmp1 = new BigNumber();
            tmp2 = new BigNumber();
            tmp3 = new BigNumber();

            BigNumber.M_get_log_guess(nn,guess);

            tolerance = -(places + 4);
            maxp = places + 16;
            local_precision = 18;
 
            ii = 0;

            while (true)
            {
                BigNumber.Exp(guess, tmp1, local_precision  );

                BigNumber.Sub( tmp1, nn,tmp3);
                BigNumber.Add( tmp1, nn,tmp2);

                BigNumber.Div(tmp3, tmp2, tmp1, local_precision);
                BigNumber.Mul(BigNumber.Two, tmp1,tmp0);
                BigNumber.Sub(guess, tmp0,tmp3);

                if (ii != 0)
                {
                    if (((3 * tmp0.exponent) < tolerance) || (tmp0.signum == 0))
                        break;
                }

                BigNumber.Round(tmp3, guess, local_precision);

                local_precision *= 3;

                if (local_precision > maxp)
                    local_precision = maxp;

                ii = 1;
            }

            BigNumber.Round(tmp3, rr, places);
            
        }

        static void M_log_basic_iteration(BigNumber nn, BigNumber rr, int places  )
        {
            BigNumber tmp0, tmp1, tmp2, tmpX;

            if (places < 360)
            {
                BigNumber.M_log_solve_cubic(nn, rr, places);
            }
            else
            {
                tmp0 = new BigNumber();
                tmp1 = new BigNumber();
                tmp2 = new BigNumber();
                tmpX = new BigNumber();

                BigNumber.M_log_solve_cubic(nn, tmpX, 110);
                BigNumber.Neg(tmpX, tmp0  );
                BigNumber.Exp( tmp0, tmp1, (places + 8));
                BigNumber.Mul( tmp1, nn, tmp2);
                BigNumber.Sub( tmp2, BigNumber.One,tmp1);

                BigNumber.M_log_near_1(tmp1, tmp0, (places - 104));

                BigNumber.Add( tmpX, tmp0,tmp1);
                BigNumber.Round( tmp1,rr ,places);
 
            }
        }

        /****************************************************************************/

        /*
         *	define a notation for a function 'R' :
         *
         *
         *
         *                                    1
         *      R (a0, b0)  =  ------------------------------
         *
         *                          ----
         *                           \ 
         *                            \     n-1      2    2
         *                      1  -   |   2    *  (a  - b )
         *                            /              n    n
         *                           /
         *                          ----
         *                         n >= 0
         *
         *
         *      where a, b are the classic AGM iteration :
         *
         *     
         *      a    =  0.5 * (a  + b )
         *       n+1            n    n
         *
         *
         *      b    =  sqrt(a  * b )
         *       n+1          n    n
         *
         *
         *
         *      define a variable 'c' for more efficient computation :
         *
         *                                      2     2     2
         *      c    =  0.5 * (a  - b )    ,   c  =  a  -  b
         *       n+1            n    n          n     n     n
         *
         */

        /****************************************************************************/
        static void	LogAGMRFunc(BigNumber aa, BigNumber bb, BigNumber rr, int places  )
        {
            BigNumber   tmp1, tmp2, tmp3, tmp4, tmpC2, sum, pow_2, tmpA0, tmpB0;
            int	tolerance, dplaces;

            tmpA0 = new BigNumber();
            tmpB0 = new BigNumber();
            tmpC2 = new BigNumber();
            tmp1  = new BigNumber();
            tmp2  = new BigNumber();
            tmp3  = new BigNumber();
            tmp4  = new BigNumber();
            sum   = new BigNumber();
            pow_2 = new BigNumber();

            tolerance = places + 8;
            dplaces   = places + 16;

            BigNumber.Copy( aa, tmpA0 );
            BigNumber.Copy( bb, tmpB0 );
            BigNumber.Copy( BigNumber.BN_OneHalf, pow_2 );

            BigNumber.Mul( aa, aa,tmp1);		    /* 0.5 * [ a ^ 2 - b ^ 2 ] */
            BigNumber.Mul( bb, bb,tmp2);
            BigNumber.Sub( tmp1, tmp2,tmp3);
            BigNumber.Mul( BigNumber.BN_OneHalf, tmp3,sum);

            while (true)
            {
                BigNumber.Sub( tmpA0, tmpB0,tmp1);      /* C n+1 = 0.5 * [ An - Bn ] */
                BigNumber.Mul( BigNumber.BN_OneHalf, tmp1,tmp4);      /* C n+1 */
                BigNumber.Mul( tmp4, tmp4,tmpC2 );       /* C n+1 ^ 2 */

                /* do the AGM */

                BigNumber.Add(tmpA0, tmpB0,tmp1);
                BigNumber.Mul( BigNumber.BN_OneHalf, tmp1,tmp3);

                BigNumber.Mul( tmpA0, tmpB0,tmp2);
                BigNumber.Sqrt(tmp2, tmpB0, dplaces  );

                BigNumber.Round(tmp3, tmpA0, dplaces  );

                /* end AGM */

                BigNumber.Mul( BigNumber.Two, pow_2,tmp2);
                BigNumber.Copy( tmp2, pow_2);

                BigNumber.Mul(tmpC2, pow_2,tmp1);
                BigNumber.Add( sum, tmp1,tmp3);

                if ((tmp1.signum == 0) || ((-2 * tmp1.exponent) > tolerance))
                 break;

                BigNumber.Round(tmp3, sum, dplaces  );
              }

            BigNumber.Sub( BigNumber.One, tmp3,tmp4);
            BigNumber.Reziprocal(tmp4, rr, places  );

            
        }

        /****************************************************************************/
        /*
	        calculate log (1 + x) with the following series:

                      x
	        y = -----      ( |y| < 1 )
	            x + 2


                    [ 1 + y ]                 y^3     y^5     y^7
	        log [-------]  =  2 * [ y  +  ---  +  ---  +  ---  ... ] 
                    [ 1 - y ]                  3       5       7 

        */

        static void	M_log_near_1(BigNumber xx, BigNumber rr, int places  )
        {
            BigNumber   tmp0, tmp1, tmp2, tmpS, term;
            int	tolerance, dplaces, local_precision;
            long    m1;

            tmp0 = new BigNumber();
            tmp1 = new BigNumber();
            tmp2 = new BigNumber();
            tmpS = new BigNumber();
            term = new BigNumber();

            tolerance = xx.exponent - (places + 6);
            dplaces   = (places + 12) - xx.exponent;

            BigNumber.Add( xx, BigNumber.Two,tmp0);
            BigNumber.Div( xx, tmp0, tmpS,(dplaces + 6));
            BigNumber.Copy( tmpS,term);
            BigNumber.Mul(tmpS, tmpS ,tmp0);
            BigNumber.Round(  tmp0,tmp2,(dplaces + 6));

            m1 = 3L;

            while (true)
            {
                BigNumber.Mul( term, tmp2,tmp0);

                if ((tmp0.exponent < tolerance) || (tmp0.signum == 0))
                 break;

                local_precision = dplaces + tmp0.exponent;

                if (local_precision < 20)
                 local_precision = 20;

                BigNumber.SetFromLong(tmp1, m1);
                BigNumber.Round( tmp0,term,local_precision);
                BigNumber.Div( term, tmp1,tmp0,local_precision);
                BigNumber.Add( tmpS, tmp0,tmp1);
                BigNumber.Copy( tmp1,tmpS);
                m1 += 2;
            }

            BigNumber.Mul( BigNumber.Two, tmpS,tmp0);
            BigNumber.Round(tmp0, rr, places  );
             
        }

        static void CheckLogPlaces(int places)
        {
            BigNumber tmp6, tmp7, tmp8, tmp9;
            int dplaces;

            dplaces = places + 4;

            if (dplaces > MM_lc_log_digits)
            {
                MM_lc_log_digits = dplaces + 4;

                tmp6 = new BigNumber();
                tmp7 = new BigNumber();
                tmp8 = new BigNumber();
                tmp9 = new BigNumber();

                dplaces += 6 + (int)Math.Log10 ((double)places);

                BigNumber.Copy( BigNumber.One, tmp7);
                tmp7.exponent = -places;

                BigNumber.LogAGMRFunc( BigNumber.One, tmp7,tmp8,dplaces);

                BigNumber.Mul( tmp7, BigNumber.BN_OneHalf,tmp6);

                BigNumber.LogAGMRFunc(BigNumber.One, tmp6, tmp9, dplaces);

                BigNumber.Sub(tmp9, tmp8, BN_lc_log2);                

                tmp7.exponent -= 1;                           

                BigNumber.LogAGMRFunc(  BigNumber.One, tmp7,tmp9,dplaces);

                BigNumber.Sub( tmp9, tmp8,BN_lc_log10);               
                BigNumber.Reziprocal(BN_lc_log10R, BN_lc_log10, dplaces  ); 
                 
            }
        }

        static void Log10( BigNumber src,BigNumber dst,int places)
        {             
            BigNumber tmp8 = new BigNumber();
            BigNumber tmp9 = new BigNumber();
            int dplaces = places + 4;

            BigNumber.CheckLogPlaces(dplaces + 45);
            BigNumber.Log(src, tmp9, dplaces );
            BigNumber.Mul( tmp9, BN_lc_log10R,tmp8);
            BigNumber.Round(tmp8,dst, places);           
        }

        static void Log(BigNumber src, BigNumber dst, int places)
        {
            BigNumber tmp0, tmp1, tmp2;
            int mexp, dplaces;

            if (src.signum <= 0)
            {
                throw new BigNumberException(" 'Log', Negative argument");               
            }

            tmp0 = new BigNumber();
            tmp1 = new BigNumber();
            tmp2 = new BigNumber();

            dplaces = places + 8;
            
            mexp = src.exponent;

            if (mexp == 0 || mexp == 1)
            {
                BigNumber.Sub( src, BigNumber.One,tmp0);

                if (tmp0.signum == 0)    /* is input exactly 1 ?? */
                {                           /* if so, result is 0    */
                    BigNumber.SetZero(dst);
                    return;
                }

                if (tmp0.exponent <= -4)
                {
                    M_log_near_1(tmp0, dst, places  );
                    return;
                }
            }

            /* make sure our log(10) is accurate enough for this calculation */
            /* (and log(2) which is called from M_log_basic_iteration) */

            BigNumber.CheckLogPlaces(dplaces + 25);

            if (Math.Abs(mexp) <= 3)
            {
                M_log_basic_iteration(src,dst, places);
            }
            else
            {
                /*
                 *  use log (x * y) = log(x) + log(y)
                 *
                 *  here we use y = exponent of our base 10 number.
                 *
                 *  let 'C' = log(10) = 2.3025850929940....
                 *
                 *  then log(x * y) = log(x) + ( C * base_10_exponent )
                 */

                BigNumber.Copy(src,tmp2 );

                mexp = tmp2.exponent - 2;
                tmp2.exponent        = 2;               

                M_log_basic_iteration(tmp2, tmp0, dplaces );

                BigNumber.SetFromLong(tmp1, (long)mexp);
                BigNumber.Mul( tmp1, BN_lc_log10,tmp2);
                BigNumber.Add( tmp2, tmp0,tmp1);

                BigNumber.Round(tmp1, dst, places);
            }
 
        }
    }
}

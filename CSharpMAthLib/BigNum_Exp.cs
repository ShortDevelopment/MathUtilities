using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        

        static private void Exp(BigNumber src, BigNumber dst, int places)
        {
            BigNumber A=0, B=0, C=0;
            int dplaces, nn = 0, ii = 0;
        
            if (src.signum == 0)		 
            {
                BigNumber.Copy(BigNumber.One, dst);
                return;
            }

            if (src.exponent <= -3)  
            {
                M_raw_exp(src, C,(places + 6));
                BigNumber.Round(C, dst, places);
                return;
            }            

            if (M_exp_compute_nn(ref nn, A, src) != 0)
            {
                throw new BigNumberException("'Exp', Input too large, Overflow");
            }

            dplaces = places + 8;

            BigNumber.CheckLogPlaces(dplaces);
            BigNumber.Mul(A, BN_lc_log2, B);
            BigNumber.Sub( src, B,A);
           
            while (true)
            {
                if (A.signum != 0)
                {
                    if (A.exponent == 0)
                        break;
                }

                if (A.signum >= 0)
                {
                    nn++;
                    BigNumber.Sub(A, BN_lc_log2, B);
                    BigNumber.Copy(B, A);
                }
                else
                {
                    nn--;
                    BigNumber.Add(A, BN_lc_log2, B);
                    BigNumber.Copy(B, A);
                }
            }

            BigNumber.Mul(A, BN_exp_512R, C);           

            M_raw_exp(C, B, dplaces);
             
            ii = 9;

            while (true)
            {
                BigNumber.Mul(B, B, C);
                BigNumber.Round(C, B, dplaces);

                if (--ii == 0)
                {
                    break;
                }
            }
           
            BigNumber.IntPow(dplaces, BigNumber.Two, nn, A);
            BigNumber.Mul(A, B, C);
            BigNumber.Round(C, dst, places);

        }
      
        static int M_exp_compute_nn(ref int n, BigNumber b, BigNumber a)
        {
            BigNumber	tmp0, tmp1;
             
            String cp = "";
            int	kk;

            n   = 0;
                      
            tmp0 = new BigNumber();
            tmp1 = new BigNumber();

            BigNumber.Mul(BN_exp_log2R, a , tmp1 );
           
            if (tmp1.signum >= 0)
            {
                BigNumber.Add( tmp1, BigNumber.BN_OneHalf,tmp0);
                BigNumber.Floor(tmp1, tmp0);
            }
            else
            {
                BigNumber.Sub( tmp1, BigNumber.BN_OneHalf,tmp0);
                BigNumber.Ceil(tmp1, tmp0);
            }

            kk = tmp1.exponent;
            

            cp = BigNumber.ToIntString(tmp1);
            n = Convert.ToInt32(cp);

            BigNumber.SetFromLong(b, (long)(n));

            kk = BigNumber.Compare(b, tmp1);
             
            return(kk);
        }
        
        static private void M_raw_exp(BigNumber xx, BigNumber rr, int places  )
        {
            BigNumber tmp0, digit, term;
            int tolerance, local_precision, prev_exp;
            long m1;

            tmp0 = new BigNumber();
            term = new BigNumber();
            digit = new BigNumber();

            local_precision = places + 8;
            tolerance = -(places + 4);
            prev_exp = 0;

            BigNumber.Add( BigNumber.One, xx,rr);
            BigNumber.Copy( xx,term );

            m1 = 2L;

            while (true)
            {
                BigNumber.SetFromLong(digit, m1);
                BigNumber.Mul( term, xx,tmp0 );
                BigNumber.Div(tmp0, digit, term, local_precision);
                BigNumber.Add( rr, term,tmp0 );
                BigNumber.Copy(tmp0, rr);

                if ((term.exponent < tolerance) || (term.signum == 0))
                    break;

                if (m1 != 2L)
                {
                    local_precision = local_precision + term.exponent - prev_exp;

                    if (local_precision < 20)
                        local_precision = 20;
                }

                prev_exp = term.exponent;
                m1++;
            }
             
        }
    }

   
}

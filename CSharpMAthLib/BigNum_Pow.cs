using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {

       

        static private void IntPow( int places, BigNumber src, int mexp,BigNumber dst)
        {
            BigNumber A, B, C;
            int nexp, ii, signflag, local_precision;

            if (mexp == 0)
            {
                BigNumber.Copy(BigNumber.One, dst);
                return;
            }
            else
            {
                if (mexp > 0)
                {
                    signflag = 0;
                    nexp = mexp;
                }
                else
                {
                    signflag = 1;
                    nexp = -mexp;
                }
            }

            if (src.signum == 0)
            {
                BigNumber.SetZero(dst);
                return;
            }

            A = new BigNumber();
            B = new BigNumber();
            C = new BigNumber();

            local_precision = places + 8;

            BigNumber.Copy(BigNumber.One,B);
            BigNumber.Copy(src,C);

            while (true)
            {
                ii = nexp & 1;
                nexp = nexp >> 1;

                if (ii != 0)                       /* exponent -was- odd */
                {
                    BigNumber.Mul( B, C,A);
                    BigNumber.Round(A,B, local_precision  );

                    if (nexp == 0)
                        break;
                }

                BigNumber.Mul( C, C, A);
                BigNumber.Round(A,C, local_precision);
            }

            if (signflag>0)
            {
                BigNumber.Reziprocal(B, dst, places  );
            }
            else
            {
                BigNumber.Round(B,dst, places  );
            }
             
        }

        static public void Power(BigNumber xx, BigNumber yy, BigNumber rr, int places)
        {
            int	iflag ;
            BigNumber   tmp8 = new BigNumber();
            BigNumber   tmp9= new BigNumber();
            int M_size_flag = BigNumber.GetSizeofInt();

            if (yy.signum == 0)
            {
                BigNumber.Copy( BigNumber.One, rr);
                return;
            }
      
            if (xx.signum == 0)
            {
                BigNumber.SetZero(rr);
                return;
            }
                      
            if (BigNumber.IsInteger(yy)>0)
            {
               iflag = 0;

                if (M_size_flag == 2)            /* 16 bit compilers */
                {
                    if (yy.exponent <= 4)
                    iflag = 1;
                }
                else                             /* >= 32 bit compilers */
                {
                    if (yy.exponent <= 7)
                    iflag = 1;
                }

                if (iflag>0)
                {
                    String sbuf = BigNumber.ToIntString(yy);
                    int Exp = Convert.ToInt32(sbuf);
                    BigNumber.IntPow(places, xx, Exp, rr);
                    return;
                }
            }

            tmp8 = new BigNumber();
            tmp9 = new BigNumber();

            BigNumber.Log(xx, tmp9, (places + 8)  );
            BigNumber.Mul(tmp9, yy, tmp8);
            BigNumber.Exp(tmp8, rr, places);
         
        }
    }
}

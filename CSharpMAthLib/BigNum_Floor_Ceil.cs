using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
       
        static void Floor(BigNumber dst, BigNumber src)
        {          
            BigNumber.Copy( src, dst );

            if (BigNumber.IsInteger(dst) > 0)           
            {
                return;
            }
            if (dst.exponent <= 0)       /* if |bb| < 1, result is -1 or 0 */
            {
                if (dst.signum < 0)
                {
                    BigNumber.Neg(BigNumber.One, dst);
                }
                else
                {
                    BigNumber.SetZero(dst);
                }

                return;
            }

            if (dst.signum < 0)
            {
                BigNumber mtmp = new BigNumber();
                BigNumber.Neg(dst,mtmp    );

                mtmp.dataLength = mtmp.exponent;

                BigNumber.Normalize(mtmp);

                BigNumber.Add( mtmp, BigNumber.One, dst);
                dst.signum = -1;                
            }
            else
            {
                dst.dataLength = dst.exponent;
                BigNumber.Normalize(dst);
            }
        }

        static void Ceil(BigNumber dst, BigNumber src)
        {
            BigNumber mtmp;

            BigNumber.Copy( src,dst);

            if (IsInteger(dst) > 0)          /* if integer, we're done */
            {
                return;
            }
            if (dst.exponent <= 0)       /* if |bb| < 1, result is 0 or 1 */
            {
                if (dst.signum < 0)
                    BigNumber.SetZero(dst);
                else
                    BigNumber.Copy(BigNumber.One, dst  );

                return;
            }

            if (dst.signum < 0)
            {
                dst.dataLength = dst.exponent;
                BigNumber.Normalize(dst);
            }
            else
            {
                mtmp = new BigNumber();
                BigNumber.Copy(dst, mtmp);

                mtmp.dataLength = mtmp.exponent;
                BigNumber.Normalize(mtmp);

                BigNumber.Add( mtmp, BigNumber.One,dst);
               
            }
        }
    }
}

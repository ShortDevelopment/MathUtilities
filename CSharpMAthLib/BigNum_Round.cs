using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {        
        static public void Round(BigNumber src, BigNumber dst, int places)
        {            
            BigNumber t0_5 = new BigNumber();
            BigNumber.Copy(Five, t0_5);
            int ii = places + 1;

            if (src.dataLength <= ii)
            {
                Copy(src, dst);
                return;
            }

            t0_5.exponent = src.exponent - ii;

            if (src.signum > 0)
            {
                BigNumber.Add(src, t0_5, dst);
            }
            else
            {
                BigNumber.Sub(src, t0_5, dst);
            }

            dst.dataLength = ii;
            BigNumber.Normalize(dst);
        }
    }
}

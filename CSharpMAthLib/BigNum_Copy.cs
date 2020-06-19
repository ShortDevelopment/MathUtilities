using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        /// <summary>
        /// copy src -> dst
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        static private void Copy(BigNumber src, BigNumber dst)
        {
            int j = (src.dataLength + 1) >> 1;

            if (j > dst.mantissa.Length)
            {
                BigNumber.Expand(dst, j + 32);
            }

            dst.dataLength = src.dataLength;
            dst.exponent = src.exponent;
            dst.signum = src.signum;

            Array.Copy(src.mantissa, dst.mantissa, j);

        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        /// <summary>
        /// compare 2 numbers
        /// </summary>
        /// <param name="ltmp"></param>
        /// <param name="rtmp"></param>
        /// <returns></returns>
        static public int Compare(BigNumber ltmp, BigNumber rtmp)
        {
            int llen, rlen, lsign, rsign, i, j, lexp, rexp;

            llen = ltmp.dataLength;
            rlen = rtmp.dataLength;

            lsign = ltmp.signum;
            rsign = rtmp.signum;

            lexp = ltmp.exponent;
            rexp = rtmp.exponent;

            if (rsign == 0)
                return (lsign);

            if (lsign == 0)
                return (-rsign);

            if (lsign == -rsign)
                return (lsign);

            if (lexp > rexp)
                goto E1;

            if (lexp < rexp)
                goto E2;

            if (llen < rlen)
                j = (llen + 1) >> 1;
            else
                j = (rlen + 1) >> 1;

            for (i = 0; i < j; i++)
            {
                if (ltmp.mantissa[i] > rtmp.mantissa[i])
                    goto E1;

                if (ltmp.mantissa[i] < rtmp.mantissa[i])
                    goto E2;
            }

            if (llen == rlen)
                return (0);
            else
            {
                if (llen > rlen)
                    goto E1;
                else
                    goto E2;
            }

        E1:

            if (lsign == 1)
                return (1);
            else
                return (-1);

        E2:

            if (lsign == 1)
                return (-1);
            else
                return (1);

        }
    }
}

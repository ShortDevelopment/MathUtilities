using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
       

        static void Neg(BigNumber s, BigNumber d)
        {
            BigNumber.Copy(s,d);

            if (d.signum != 0)
                d.signum = (sbyte)-(d.signum);
        }
    }
}

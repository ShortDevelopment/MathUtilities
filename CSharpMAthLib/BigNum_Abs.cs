using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        /// <summary>
        /// get absolute  value of number
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        static void Abs(BigNumber d, BigNumber s)
        {
            BigNumber.Copy(s,d);
            if (d.signum != 0)
            {
                d.signum = 1;
            }
        }

        
    }
}

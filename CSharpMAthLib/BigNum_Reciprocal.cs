using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static public void Reziprocal(BigNumber src, BigNumber dst, int places)
        {           
            BigNumber.Div( BigNumber.One, src, dst,places);
        }
    }
}

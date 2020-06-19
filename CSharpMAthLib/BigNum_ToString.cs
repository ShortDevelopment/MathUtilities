using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static CultureInfo scci = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

        

        static private String ToFullString(BigNumber atm)
        {
            String res = "";
            byte numdiv = 0, numrem = 0;

            int max_i = (atm.dataLength + 1) >> 1;
            int exp = atm.exponent;

             for (int i = 0; i < max_i; i++)
            {
                BigNumber.Unpack(atm.mantissa[i], ref numdiv, ref numrem);
                res += (char)('0' + numdiv);                 
                res += (char)('0' + numrem);
            }

            res = res.Substring(0, atm.dataLength);
         
            if (exp > 0)
            {               
                for (int i = res.Length; i < exp; i++)
                {
                    res += "0";
                }

                if (exp  < res.Length)
                {
                    res = res.Insert(exp , ".");
                }
            }
            else if (exp <= 0)
            {
                while (exp++ <= 0)
                {
                    res = "0" + res;                    
                }

                if (exp <= res.Length)
                {
                    res = res.Insert(1, ".");
                }               
            }
            

            return res;
        }
     
        static private String ToExpString(BigNumber atm, int digits)
        {
            int i, index, first, max_i, num_digits, dec_places;
            byte numdiv = 0, numrem = 0;

            BigNumber ctmp = new BigNumber();
            String res = "";

            dec_places = digits;

            if (dec_places < 0)
            {
                BigNumber.Copy(atm, ctmp);
            }
            else
            {
                BigNumber.Round(atm, ctmp, dec_places);
            }

            if (ctmp.signum == 0)
            {
                if (dec_places < 0)
                {
                    res = "0.0E+0";
                }
                else
                {
                    res = "0";

                    if (dec_places > 0)
                    {
                        res += ".";
                    }

                    for (i = 0; i < dec_places; i++)
                    {
                        res += "0";
                    }

                    res += "E+0";

                }
                return res;
            }

            max_i = (ctmp.dataLength + 1) >> 1;

            if (dec_places < 0)
            {
                num_digits = ctmp.dataLength;
            }
            else
            {
                num_digits = dec_places + 1;
            }
            if (ctmp.signum == -1)
            {
                res += '-';
            }

            first = 1;

            i = 0;
            index = 0;

            while (true)
            {
                if (index >= max_i)
                {
                    numdiv = 0;
                    numrem = 0;
                }
                else
                {
                    Unpack(ctmp.mantissa[index], ref numdiv, ref numrem);
                }

                index++;

                res += (char)('0' + numdiv);

                if (++i == num_digits)
                    break;

                if (first != 0)
                {
                    first = 0;
                    res += '.';
                }

                res += (char)('0' + numrem);

                if (++i == num_digits)
                    break;
            }

            i = ctmp.exponent - 1;

            if (i >= 0)
                res += "E+" + i;
            else if (i < 0)
                res += "E" + i;


            return res;

        }

        static double ExpStringToDouble(String src)
        {
            scci.NumberFormat.NumberDecimalSeparator = ".";
            return Convert.ToDouble(src, scci);
        }

        static private String ToIntString(BigNumber atm)
        {
            int ct, dl, numb, ii;
            ct = atm.exponent;
            dl = atm.dataLength;
            String result = String.Empty;

            byte numdiv = 0, numrem = 0;

            if (ct <= 0 || atm.signum == 0)
            {
                return "0";
            }

            if (ct > 112)
            {
                BigNumber.Expand(atm, ct + 32);
            }

            ii = 0;

            if (atm.signum == -1)
            {
                ii = 1;
                result += "-";
            }

            if (ct > dl)
            {
                numb = (dl + 1) >> 1;
            }
            else
            {
                numb = (ct + 1) >> 1;
            }

            int ucp = 0;

            while (true)
            {
                Unpack(atm.mantissa[ucp++], ref numdiv, ref numrem);

                result += (char)('0' + numdiv);
                result += (char)('0' + numrem);

                if (--numb == 0)
                {
                    break;
                }
            }

            if (ct > dl)
            {
                for (int i = 0; i < (ct + 1 - dl); i++)
                {
                    result += '0';
                }
            }

            result = result.Substring(0, ct + ii);


            return result;
        }
    }
}

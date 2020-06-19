using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static public int numDefaultPlaces = 32;

        /// <summary>
        /// BigNumber is represented in the scientific notation:
        /// 
        ///         S*Ax10^B
        ///  
        ///         S .... signum
        ///         A .... significand or mantissa
        ///         B .... exponent
        /// 
        /// </summary>
        sbyte signum=0;
        int exponent=0;
        byte[] mantissa=  {0};
        int dataLength=1;
        public BigNumber()
        {}
        public BigNumber(string s)
        {
            BigNumber.SetFromString(this, s);
        }
        static public BigNumber Parse(string s)
        {
            BigNumber A = new BigNumber();
            BigNumber.SetFromString(A, s);
            return A;
        }
        public BigNumber(BigNumber rh)
        {            
            BigNumber.Copy(rh,this);
        }

        public static implicit operator BigNumber(long intValue)
        {
            BigNumber A = new BigNumber();
            BigNumber.SetFromLong(A, intValue);
            return A;
        }

        public static implicit operator BigNumber(float floatValue)
        {
            BigNumber A = new BigNumber();
            BigNumber.SetFromString(A,floatValue.ToString());
            return A;
        }

        public static implicit operator BigNumber(double doubleValue)
        {
            BigNumber A = new BigNumber();
            BigNumber.SetFromDouble(A,doubleValue);
            return A;
        }

        public static implicit operator BigNumber(String value)
        {
            BigNumber A = new BigNumber();
            BigNumber.SetFromString(A, value);
            return A;
        }

        public static BigNumber operator +(BigNumber c1, BigNumber c2)
        {
            BigNumber res = new BigNumber();
            BigNumber.Add(c1, c2, res);
            return res;
        }

        public static BigNumber operator +(BigNumber c1, long c2)
        {
            BigNumber res = new BigNumber();
            BigNumber C2 = c2;
            BigNumber.Add(c1, C2, res);
            return res;
        }

        public static BigNumber operator -(BigNumber c1, BigNumber c2)
        {
            BigNumber res = new BigNumber();
            BigNumber.Sub(c1, c2, res);
            return res;
        }

        public static BigNumber operator -(BigNumber c1, long c2)
        {
            BigNumber res = new BigNumber();
            BigNumber C2 = c2;
            BigNumber.Sub(c1, C2, res);
            return res;
        }

        public static BigNumber operator *(BigNumber c1, BigNumber c2)
        {
            BigNumber res = new BigNumber();
            BigNumber.Mul(c1, c2, res);
            return res;
        }

        public static BigNumber operator *(BigNumber c1, int c2)
        {
            BigNumber res = new BigNumber();
            BigNumber C2 = c2;
            BigNumber.Mul(c1, C2, res);
            return res;
        }

        public static BigNumber operator *(BigNumber c1, double c2)
        {
            BigNumber res = new BigNumber();
            BigNumber C2 = c2;
            BigNumber.Mul(c1, C2, res);
            return res;
        }

        public static BigNumber operator /(BigNumber c1, BigNumber c2)
        {
            BigNumber res = new BigNumber();
            BigNumber.Div(c1, c2, res);
            return res;
        }

        public BigNumber Neg()
        {
            BigNumber res = 0;
            BigNumber.Neg(this, res);
            return res;
        }

        public BigNumber Abs()
        {
            BigNumber res = 0;
            BigNumber.Abs(res, this);
            return res;
        }

        public BigNumber Rez()
        {
            BigNumber res = 0;
            BigNumber.Reziprocal(this, res, numDefaultPlaces);
            return res;
        }

        public BigNumber Rez(int places)
        {
            BigNumber res = 0;
            BigNumber.Reziprocal(this, res, places);
            return res;
        }

        public BigNumber Round(int places)
        {
            BigNumber res = 0;
            BigNumber.Round(this, res, places);
            return res;
        }

        public BigNumber Sqrt(int places)
        {
            BigNumber res = 0;
            BigNumber.Sqrt(this, res, places);
            return res;
        }

        public BigNumber Sqrt()
        {
            BigNumber res = 0;
            BigNumber.Sqrt(this, res, numDefaultPlaces);
            return res;
        }

        public BigNumber Exp()
        {
            BigNumber res = 0;
            BigNumber.Exp(this, res, numDefaultPlaces);
            return res;
        }

        public BigNumber Exp(int places)
        {
            BigNumber res = 0;
            BigNumber.Exp(this, res, places);
            return res;
        }

        public BigNumber Pow(int exp)
        {
            BigNumber res = 0;
            BigNumber.IntPow(numDefaultPlaces, this, exp, res);
            return res;
        }

        public BigNumber Pow(BigNumber exp)
        {
            BigNumber res = 0;
            BigNumber.Power(this, exp, res, numDefaultPlaces);
            return res;
        }

        public BigNumber Pow(int exp, int places)
        {
            BigNumber res = 0;
            BigNumber.IntPow(places,this, exp, res);
            return res;
        }
         
        public BigNumber Pow(BigNumber exp, int places)
        {
            BigNumber res = 0;
            BigNumber.Power(this,  exp, res, places);
            return res;
        }

        public BigNumber Log10()
        {
            BigNumber dst = new BigNumber();
            BigNumber.Log10(this, dst, numDefaultPlaces);
            return dst;
        }

        public BigNumber Log10(int places)
        {
            BigNumber dst = new BigNumber();
            BigNumber.Log10(this, dst, places);
            return dst;
        }

        public BigNumber Log()
        {
            BigNumber dst = new BigNumber();
            BigNumber.Log(this, dst, numDefaultPlaces);
            return dst;

        }
        public BigNumber Log(int places)
        {
            BigNumber dst = new BigNumber();
            BigNumber.Log(this, dst, places);
            return dst;
        }

        public BigNumber Floor()
        {
            BigNumber dst = new BigNumber();
            BigNumber.Floor(dst, this);
            return dst;
        }

        public BigNumber Ceil()
        {
            BigNumber dst = new BigNumber();
            BigNumber.Ceil(dst, this);
            return dst;
        }


        public static bool operator >(BigNumber c1, BigNumber c2)
        {
            return BigNumber.Compare(c1, c2) > 0;
        }

        public static bool operator >=(BigNumber c1, BigNumber c2)
        {
            return BigNumber.Compare(c1, c2) >= 0;
        }

        public static bool operator <(BigNumber c1, BigNumber c2)
        {
            return BigNumber.Compare(c1, c2) < 0;
        }

        public static bool operator <=(BigNumber c1, BigNumber c2)
        {
            return BigNumber.Compare(c1, c2) <= 0;
        }

        public static bool operator ==(BigNumber c1, BigNumber c2)
        {
            return BigNumber.Compare(c1, c2) == 0;
        }

        public static bool operator !=(BigNumber c1, BigNumber c2)
        {
            return !(c1 == c2);

        }
        public string ToIntString()
        {
            return BigNumber.ToIntString(this);
        }

        public string ToFullString()
        {
            return BigNumber.ToFullString(this);
        }

        public override string ToString()
        {
            return BigNumber.ToExpString(this, -1);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
       
    }
}

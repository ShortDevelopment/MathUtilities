using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        public static void DoTest()
        {
            BigNumber A = 0, B = 0, C = 0;
            BigNumber PI= new BigNumber();

            // assignment by string
            A = "12345";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            // assignment from hexadecimal string
            A = "0xff";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            A = "0x1ff";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            A = "0x123456789";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            A = "0xFeDcBafedcba";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            // assignment from binary string
            A = "%10001000";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            A = "%11011011100111000111000111100011010101010101";
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            // assignment by double
            A = 123.45;
            B = 0.12345;
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString() + ")");

            // assignment by string in exponential form x = a*10^y. 10E3 = 10*10^^3 = 10000
            A = "10E3";     // 10E3 = 10*10^3 = 10000
            B = "1E4";      // 1E4  =  1*10^4 = 10000
            C = 10000;
            Console.WriteLine("assigned value was: " + A.ToFullString() + "(" + A.ToString()+")");
            Console.WriteLine("10000 = " + A.ToFullString() + " = " + B.ToFullString() + " = " + C.ToFullString());
            
            A = 1; B = 2; C = 0;
            C = A + B;
            Console.WriteLine("the result of " + A.ToFullString() + "+" + B.ToFullString() + "=" + C.ToFullString());
            // addition of BigNumber + double
            C = A + 3.2;
            // addition of double + BigNumber
            C = 3.1 + B;             
            A = "5.141592"; B = "2.91827";
            C = A - B;
            Console.WriteLine("the result of " + A.ToFullString() + "-" + B.ToFullString() + "=" + C.ToFullString());

            C = A * B;
            Console.WriteLine("the result of " + A.ToFullString() + "*" + B.ToFullString() + "=" + C.ToFullString());
            A = 5.0; B = 3.0;
            C = A * B;
            Console.WriteLine("the result of " + A.ToFullString() + "*" + B.ToFullString() + "=" + C.ToFullString());

            A = 4; B = 0.5;
            C = A.Pow(B);
            Console.WriteLine("the result of " + A.ToFullString() + " pow " + B.ToFullString() + "=" + C.ToFullString());

            A = 0.5; B = "5E-1";  
            C = A.Pow(B,16);
            Console.WriteLine("the result of " + A.ToFullString() + " pow " + B.ToFullString() + "=" + C.ToFullString());

            A = "1e3"; // "10E2"; //   "1E3 = 1000";
            C = A.Log10();
            Console.WriteLine("the result of " + A.ToFullString() + " Log10 =" + C.ToFullString());
           
            A = "10E3"; B = "1E4"; C=10000 ;

            A = BigNumber.BN_E;
            C = A.Log();
            Console.WriteLine("the result of " + A.ToString() + " Log =" + C.ToFullString());

            A = 3.0;
            C = A.Rez();
            Console.WriteLine("the result of " + A.ToString() + " Rez =" + C.ToFullString());

            int NumPlaces = 4;
            A = 1.53456;
            C = A.Round(NumPlaces);
            Console.WriteLine("the result of " + A.ToString() + " Round(" + NumPlaces + ") =" + C.ToFullString());

            NumPlaces = 2;
            C = A.Round(NumPlaces);
            Console.WriteLine("the result of " + A.ToString() + " Round(" + NumPlaces + ") =" + C.ToFullString());

            NumPlaces = 0;
            C = A.Round(NumPlaces);
            Console.WriteLine("the result of " + A.ToString() + " Round(" + NumPlaces + ") =" + C.ToFullString());

            NumPlaces = 16;
            A = 2.0;
            C = A.Sqrt(NumPlaces);
            Console.WriteLine("the result of " + A.ToString() + " Sqrt(" + NumPlaces + ") =" + C.ToFullString());

            A = 1.0; B = 0;
            try
            {
                C = A / B;
            }
            catch (BigNumberException ex)
            {
                Console.WriteLine("Exception in operation: " + ex.Message);
            }

            A = 1.0;
            for (int i = 1; i <= 1000; i++)
            {
                A = A * i;
            }
             
            Console.WriteLine("the result of 1000!=" + A.ToFullString());
            A = A.Round(numDefaultPlaces);
            Console.WriteLine("the result of 1000!=" + A.ToString());

            DateTime before = DateTime.Now;

            NumPlaces = 5000;
            CalculatePiAGM(PI, NumPlaces);

            TimeSpan ts = DateTime.Now - before;
            Console.WriteLine("time for "+NumPlaces+" digits of PI: " + ts.TotalMilliseconds + "[ms]");
            Console.WriteLine(PI.ToFullString());

            Console.WriteLine("Press 'x' key to quit test");
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey();
                if (i.KeyChar == 'x') break;
            }
        }

    }
}

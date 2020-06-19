using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {


        //
        //                  1          2          3                    k
        //        pi = 2 + --- * (2 + --- * (2 + --- * (2 + ...  (2 + ---- * (2 + ... ))...)))
        //                  3          5          7                   2k+1
        // Calculation of pi to 32372 decimal digits */
        // Size of program: 152 characters */
        // After Dik T. Winter, CWI Amsterdam */
        //
        //  unsigned a=1e4,b,c=113316,d,e,f[113316],g,h,i;
        //  main(){for(;b=c,c-=14;i=printf("%04d",e+d/a),e=d%a)
        //  while(g=--b*2)d=h*b+a*(i?f[b]:a/5),h=d/--g,f[b]=d-g*h;}
        //
        //  unsigned a=1e4,b,c=113316,d,e,f[113316],g,h,i;
        //
        //  main()
        //  {
        //      for(;b=c,c-=14;i=printf("%04d",e+d/a),e=d%a)
        //      {
        //          while(g=--b*2)
        //          {
        //              d   = h*b+a*(i?f[b]:a/5);
        //              h   = d/--g;
        //              f[b]= d-g*h;
        //          }
        //       }
        //  }

        static private void CalculatePiSpigot(BigNumber outv, int places)
        {

           

        }

       
         
        /****************************************************************************/
        /*
         *      Calculate PI using the AGM (Arithmetic-Geometric Mean)
         *
         *      Init :  A0  = 1
         *              B0  = 1 / sqrt(2)
         *              Sum = 1
         *
         *      Iterate: n = 1...
         *
         *
         *      A   =  0.5 * [ A    +  B   ]
         *       n              n-1     n-1
         *
         *
         *      B   =  sqrt [ A    *  B   ]
         *       n             n-1     n-1
         *
         *
         *      C   =  0.5 * [ A    -  B   ]
         *       n              n-1     n-1
         *
         *
         *                      2      n+1
         *     Sum  =  Sum  -  C   *  2
         *                      n
         *
         *
         *      At the end when C  is 'small enough' :
         *                       n
         *
         *                    2 
         *      PI  =  4  *  A    /  Sum
         *                    n+1
         *
         *          -OR-
         *
         *                       2
         *      PI  = ( A  +  B )   /  Sum
         *               n     n
         *
         */
        static private void CalculatePiAGM(BigNumber outv, int places)
        {
            int dplaces, nn;

            BigNumber tmp1 = new BigNumber();
            BigNumber tmp2 = new BigNumber();
            BigNumber a0 = new BigNumber();
            BigNumber b0 = new BigNumber();
            BigNumber c0 = new BigNumber();
            BigNumber a1 = new BigNumber();
            BigNumber b1 = new BigNumber();
            BigNumber sum = new BigNumber();
            BigNumber pow_2 = new BigNumber();

            dplaces = places + 16;

            BigNumber.Copy(BigNumber.One , a0);
            BigNumber.Copy(BigNumber.One , sum);
            BigNumber.Copy(BigNumber.Four, pow_2);
            BigNumber.Sqrt(BigNumber.BN_OneHalf, b0, dplaces);
         
            while (true)
            {
                BigNumber.Add(a0 , b0 ,tmp1);
                BigNumber.Mul(tmp1 ,BigNumber.BN_OneHalf,a1);
                BigNumber.Mul(a0 , b0 ,tmp1);
                BigNumber.Sqrt(tmp1 , b1 ,dplaces);
                BigNumber.Sub(a0 , b0 ,tmp1);
                BigNumber.Mul(BigNumber.BN_OneHalf, tmp1 ,c0 );                               
                BigNumber.Mul(c0 , c0 ,tmp1);
                BigNumber.Mul(tmp1 , pow_2 ,tmp2);
                BigNumber.Sub(sum , tmp2 ,tmp1);
                BigNumber.Round(tmp1 ,sum ,dplaces  );

                nn = -4 * c0.exponent;

                if (nn >= dplaces)
                {
                    break;
                }
               
                BigNumber.Copy(a1,a0);
                BigNumber.Copy(b1,b0);
                BigNumber.Mul(pow_2, BigNumber.Two, tmp1 );
                BigNumber.Copy(tmp1, pow_2);
            }

            BigNumber.Add( a1, b1,tmp1);
            BigNumber.Mul( tmp1, tmp1,tmp2);
            BigNumber.Div( tmp2, sum,tmp1, dplaces );
            BigNumber.Round(tmp1,outv, places);             
        }

        static private void CalculatePiMachin(BigNumber outv, int places)
        {
             BigNumber curanswer;
             BigNumber lastanswer;

             BigNumber term5;
             BigNumber term239;
             BigNumber term5m;
             BigNumber term239m;
             BigNumber diff;
             BigNumber lim;

             int n5;
             int n239;
             bool b5termdone = false;
             bool b239termdone = false;

             lim = 1;

             lim /= (Math.Pow(10,places));
             
             n5 = 0;
             n239 = 0;

             term5 = 16;
             term5 /= 5;

             term239 = 4;
             term239 /= 239;

             curanswer = 0;
             lastanswer = 0;
            
             while ( b5termdone == false &&  b239termdone == false)
             {
                 lastanswer = curanswer;
              
                 term5m = term5;
                 term5m /= n5 * 2 + 1;

                 term239m = term239;
                 term239m /= n239 * 2 + 1;

                 if (n5 % 2 == 0)
                 {
                     curanswer += term5m;  /* n5 is even */
                 }
                 else
                 {
                     curanswer -= term5m;  /* n5 is odd */
                 }
                    
                 if (n239 % 2 == 0)
                 {
                     curanswer -= term239m;  /* n239 is even */
                 }
                 else
                 {
                     curanswer += term239m;  /* n239 is odd */
                 }

                 term5 /= 25; // 5*5
                 n5++;

                 term239 /= 57121; //  239 * 239;
                 n239++;

                 diff = lastanswer - curanswer;
                 BigNumber.Abs(diff, diff);

                 if (diff < lim) break;
             }

             BigNumber.Round(curanswer, outv, places);

        }
    }
}

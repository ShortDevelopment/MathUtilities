using System;
using System.Collections.Generic;
using System.Text;

namespace BigNumbers
{
    public partial class BigNumber
    {
        static readonly int RDFT_LOOP_DIV = 64;
        static readonly double WR5000 = 0.707106781186547524400844362104849039284835937688;

        static private void M_bitrv2(int n, double[] a)
        {
            int j0, k0, j1, k1, l, m, i, j, k;
            double xr, xi, yr, yi;

            l = n >> 2;
            m = 2;
            while (m < l)
            {
                l >>= 1;
                m <<= 1;
            }
            if (m == l)
            {
                j0 = 0;
                for (k0 = 0; k0 < m; k0 += 2)
                {
                    k = k0;
                    for (j = j0; j < j0 + k0; j += 2)
                    {
                        xr = a[j];
                        xi = a[j + 1];
                        yr = a[k];
                        yi = a[k + 1];
                        a[j] = yr;
                        a[j + 1] = yi;
                        a[k] = xr;
                        a[k + 1] = xi;
                        j1 = j + m;
                        k1 = k + 2 * m;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m;
                        k1 -= m;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m;
                        k1 += 2 * m;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        for (i = n >> 1; i > (k ^= i); i >>= 1) ;
                    }
                    j1 = j0 + k0 + m;
                    k1 = j1 + m;
                    xr = a[j1];
                    xi = a[j1 + 1];
                    yr = a[k1];
                    yi = a[k1 + 1];
                    a[j1] = yr;
                    a[j1 + 1] = yi;
                    a[k1] = xr;
                    a[k1 + 1] = xi;
                    for (i = n >> 1; i > (j0 ^= i); i >>= 1) ;
                }
            }
            else
            {
                j0 = 0;
                for (k0 = 2; k0 < m; k0 += 2)
                {
                    for (i = n >> 1; i > (j0 ^= i); i >>= 1) ;
                    k = k0;
                    for (j = j0; j < j0 + k0; j += 2)
                    {
                        xr = a[j];
                        xi = a[j + 1];
                        yr = a[k];
                        yi = a[k + 1];
                        a[j] = yr;
                        a[j + 1] = yi;
                        a[k] = xr;
                        a[k + 1] = xi;
                        j1 = j + m;
                        k1 = k + m;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        for (i = n >> 1; i > (k ^= i); i >>= 1) ;
                    }
                }
            }
        }

        static private void M_cftfsub(int n, double[] a)
        {
            int j, j1, j2, j3, l;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;

            l = 2;
            if (n > 8)
            {
                M_cft1st(n, a);
                l = 8;
                while ((l << 2) < n)
                {
                    M_cftmdl(n, l, a);
                    l <<= 2;
                }
            }
            if ((l << 2) == n)
            {
                for (j = 0; j < l; j += 2)
                {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = a[j + 1] + a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = a[j + 1] - a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i + x2i;
                    a[j2] = x0r - x2r;
                    a[j2 + 1] = x0i - x2i;
                    a[j1] = x1r - x3i;
                    a[j1 + 1] = x1i + x3r;
                    a[j3] = x1r + x3i;
                    a[j3 + 1] = x1i - x3r;
                }
            }
            else
            {
                for (j = 0; j < l; j += 2)
                {
                    j1 = j + l;
                    x0r = a[j] - a[j1];
                    x0i = a[j + 1] - a[j1 + 1];
                    a[j] += a[j1];
                    a[j + 1] += a[j1 + 1];
                    a[j1] = x0r;
                    a[j1 + 1] = x0i;
                }
            }
        }

        static private void M_cftbsub(int n, double[] a)
        {
            int j, j1, j2, j3, l;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;

            l = 2;
            if (n > 8)
            {
                M_cft1st(n, a);
                l = 8;
                while ((l << 2) < n)
                {
                    M_cftmdl(n, l, a);
                    l <<= 2;
                }
            }
            if ((l << 2) == n)
            {
                for (j = 0; j < l; j += 2)
                {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = -a[j + 1] - a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = -a[j + 1] + a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i - x2i;
                    a[j2] = x0r - x2r;
                    a[j2 + 1] = x0i + x2i;
                    a[j1] = x1r - x3i;
                    a[j1 + 1] = x1i - x3r;
                    a[j3] = x1r + x3i;
                    a[j3 + 1] = x1i + x3r;
                }
            }
            else
            {
                for (j = 0; j < l; j += 2)
                {
                    j1 = j + l;
                    x0r = a[j] - a[j1];
                    x0i = -a[j + 1] + a[j1 + 1];
                    a[j] += a[j1];
                    a[j + 1] = -a[j + 1] - a[j1 + 1];
                    a[j1] = x0r;
                    a[j1 + 1] = x0i;
                }
            }
        }

        static private void M_cft1st(int n, double[] a)
        {
            int j, kj, kr;
            double ew, wn4r, wk1r, wk1i, wk2r, wk2i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;

            x0r = a[0] + a[2];
            x0i = a[1] + a[3];
            x1r = a[0] - a[2];
            x1i = a[1] - a[3];
            x2r = a[4] + a[6];
            x2i = a[5] + a[7];
            x3r = a[4] - a[6];
            x3i = a[5] - a[7];
            a[0] = x0r + x2r;
            a[1] = x0i + x2i;
            a[4] = x0r - x2r;
            a[5] = x0i - x2i;
            a[2] = x1r - x3i;
            a[3] = x1i + x3r;
            a[6] = x1r + x3i;
            a[7] = x1i - x3r;
            wn4r = WR5000;
            x0r = a[8] + a[10];
            x0i = a[9] + a[11];
            x1r = a[8] - a[10];
            x1i = a[9] - a[11];
            x2r = a[12] + a[14];
            x2i = a[13] + a[15];
            x3r = a[12] - a[14];
            x3i = a[13] - a[15];
            a[8] = x0r + x2r;
            a[9] = x0i + x2i;
            a[12] = x2i - x0i;
            a[13] = x0r - x2r;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[10] = wn4r * (x0r - x0i);
            a[11] = wn4r * (x0r + x0i);
            x0r = x3i + x1r;
            x0i = x3r - x1i;
            a[14] = wn4r * (x0i - x0r);
            a[15] = wn4r * (x0i + x0r);
            ew = PiHalf / n;
            kr = 0;
            for (j = 16; j < n; j += 16)
            {
                for (kj = n >> 2; kj > (kr ^= kj); kj >>= 1) ;
                wk1r = Math.Cos(ew * kr);
                wk1i = Math.Sin(ew * kr);
                wk2r = 1 - 2 * wk1i * wk1i;
                wk2i = 2 * wk1i * wk1r;
                wk3r = wk1r - 2 * wk2i * wk1i;
                wk3i = 2 * wk2i * wk1r - wk1i;
                x0r = a[j] + a[j + 2];
                x0i = a[j + 1] + a[j + 3];
                x1r = a[j] - a[j + 2];
                x1i = a[j + 1] - a[j + 3];
                x2r = a[j + 4] + a[j + 6];
                x2i = a[j + 5] + a[j + 7];
                x3r = a[j + 4] - a[j + 6];
                x3i = a[j + 5] - a[j + 7];
                a[j] = x0r + x2r;
                a[j + 1] = x0i + x2i;
                x0r -= x2r;
                x0i -= x2i;
                a[j + 4] = wk2r * x0r - wk2i * x0i;
                a[j + 5] = wk2r * x0i + wk2i * x0r;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[j + 2] = wk1r * x0r - wk1i * x0i;
                a[j + 3] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[j + 6] = wk3r * x0r - wk3i * x0i;
                a[j + 7] = wk3r * x0i + wk3i * x0r;
                x0r = wn4r * (wk1r - wk1i);
                wk1i = wn4r * (wk1r + wk1i);
                wk1r = x0r;
                wk3r = wk1r - 2 * wk2r * wk1i;
                wk3i = 2 * wk2r * wk1r - wk1i;
                x0r = a[j + 8] + a[j + 10];
                x0i = a[j + 9] + a[j + 11];
                x1r = a[j + 8] - a[j + 10];
                x1i = a[j + 9] - a[j + 11];
                x2r = a[j + 12] + a[j + 14];
                x2i = a[j + 13] + a[j + 15];
                x3r = a[j + 12] - a[j + 14];
                x3i = a[j + 13] - a[j + 15];
                a[j + 8] = x0r + x2r;
                a[j + 9] = x0i + x2i;
                x0r -= x2r;
                x0i -= x2i;
                a[j + 12] = -wk2i * x0r - wk2r * x0i;
                a[j + 13] = -wk2i * x0i + wk2r * x0r;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[j + 10] = wk1r * x0r - wk1i * x0i;
                a[j + 11] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[j + 14] = wk3r * x0r - wk3i * x0i;
                a[j + 15] = wk3r * x0i + wk3i * x0r;
            }
        }

        static private void M_cftmdl(int n, int l, double[] a)
        {
            int j, j1, j2, j3, k, kj, kr, m, m2;
            double ew, wn4r, wk1r, wk1i, wk2r, wk2i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;

            m = l << 2;
            for (j = 0; j < l; j += 2)
            {
                j1 = j + l;
                j2 = j1 + l;
                j3 = j2 + l;
                x0r = a[j] + a[j1];
                x0i = a[j + 1] + a[j1 + 1];
                x1r = a[j] - a[j1];
                x1i = a[j + 1] - a[j1 + 1];
                x2r = a[j2] + a[j3];
                x2i = a[j2 + 1] + a[j3 + 1];
                x3r = a[j2] - a[j3];
                x3i = a[j2 + 1] - a[j3 + 1];
                a[j] = x0r + x2r;
                a[j + 1] = x0i + x2i;
                a[j2] = x0r - x2r;
                a[j2 + 1] = x0i - x2i;
                a[j1] = x1r - x3i;
                a[j1 + 1] = x1i + x3r;
                a[j3] = x1r + x3i;
                a[j3 + 1] = x1i - x3r;
            }
            wn4r = WR5000;
            for (j = m; j < l + m; j += 2)
            {
                j1 = j + l;
                j2 = j1 + l;
                j3 = j2 + l;
                x0r = a[j] + a[j1];
                x0i = a[j + 1] + a[j1 + 1];
                x1r = a[j] - a[j1];
                x1i = a[j + 1] - a[j1 + 1];
                x2r = a[j2] + a[j3];
                x2i = a[j2 + 1] + a[j3 + 1];
                x3r = a[j2] - a[j3];
                x3i = a[j2 + 1] - a[j3 + 1];
                a[j] = x0r + x2r;
                a[j + 1] = x0i + x2i;
                a[j2] = x2i - x0i;
                a[j2 + 1] = x0r - x2r;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[j1] = wn4r * (x0r - x0i);
                a[j1 + 1] = wn4r * (x0r + x0i);
                x0r = x3i + x1r;
                x0i = x3r - x1i;
                a[j3] = wn4r * (x0i - x0r);
                a[j3 + 1] = wn4r * (x0i + x0r);
            }
            ew = PiHalf / n;
            kr = 0;
            m2 = 2 * m;
            for (k = m2; k < n; k += m2)
            {
                for (kj = n >> 2; kj > (kr ^= kj); kj >>= 1) ;
                wk1r = Math.Cos(ew * kr);
                wk1i = Math.Sin(ew * kr);
                wk2r = 1 - 2 * wk1i * wk1i;
                wk2i = 2 * wk1i * wk1r;
                wk3r = wk1r - 2 * wk2i * wk1i;
                wk3i = 2 * wk2i * wk1r - wk1i;
                for (j = k; j < l + k; j += 2)
                {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = a[j + 1] + a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = a[j + 1] - a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i + x2i;
                    x0r -= x2r;
                    x0i -= x2i;
                    a[j2] = wk2r * x0r - wk2i * x0i;
                    a[j2 + 1] = wk2r * x0i + wk2i * x0r;
                    x0r = x1r - x3i;
                    x0i = x1i + x3r;
                    a[j1] = wk1r * x0r - wk1i * x0i;
                    a[j1 + 1] = wk1r * x0i + wk1i * x0r;
                    x0r = x1r + x3i;
                    x0i = x1i - x3r;
                    a[j3] = wk3r * x0r - wk3i * x0i;
                    a[j3 + 1] = wk3r * x0i + wk3i * x0r;
                }
                x0r = wn4r * (wk1r - wk1i);
                wk1i = wn4r * (wk1r + wk1i);
                wk1r = x0r;
                wk3r = wk1r - 2 * wk2r * wk1i;
                wk3i = 2 * wk2r * wk1r - wk1i;
                for (j = k + m; j < l + (k + m); j += 2)
                {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = a[j + 1] + a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = a[j + 1] - a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i + x2i;
                    x0r -= x2r;
                    x0i -= x2i;
                    a[j2] = -wk2i * x0r - wk2r * x0i;
                    a[j2 + 1] = -wk2i * x0i + wk2r * x0r;
                    x0r = x1r - x3i;
                    x0i = x1i + x3r;
                    a[j1] = wk1r * x0r - wk1i * x0i;
                    a[j1 + 1] = wk1r * x0i + wk1i * x0r;
                    x0r = x1r + x3i;
                    x0i = x1i - x3r;
                    a[j3] = wk3r * x0r - wk3i * x0i;
                    a[j3 + 1] = wk3r * x0i + wk3i * x0r;
                }
            }
        }

        static private void M_rftfsub(int n, double[] a)
        {
            int i, i0, j, k;
            double ec, w1r, w1i, wkr, wki, wdr, wdi, ss, xr, xi, yr, yi;

            ec = 2 * PiHalf / n;
            wkr = 0;
            wki = 0;
            wdi = Math.Cos(ec);
            wdr = Math.Sin(ec);
            wdi *= wdr;
            wdr *= wdr;
            w1r = 1 - 2 * wdr;
            w1i = 2 * wdi;
            ss = 2 * w1i;
            i = n >> 1;
            while (true)
            {
                i0 = i - 4 * RDFT_LOOP_DIV;
                if (i0 < 4)
                {
                    i0 = 4;
                }
                for (j = i - 4; j >= i0; j -= 4)
                {
                    k = n - j;
                    xr = a[j + 2] - a[k - 2];
                    xi = a[j + 3] + a[k - 1];
                    yr = wdr * xr - wdi * xi;
                    yi = wdr * xi + wdi * xr;
                    a[j + 2] -= yr;
                    a[j + 3] -= yi;
                    a[k - 2] += yr;
                    a[k - 1] -= yi;
                    wkr += ss * wdi;
                    wki += ss * (0.5 - wdr);
                    xr = a[j] - a[k];
                    xi = a[j + 1] + a[k + 1];
                    yr = wkr * xr - wki * xi;
                    yi = wkr * xi + wki * xr;
                    a[j] -= yr;
                    a[j + 1] -= yi;
                    a[k] += yr;
                    a[k + 1] -= yi;
                    wdr += ss * wki;
                    wdi += ss * (0.5 - wkr);
                }
                if (i0 == 4)
                {
                    break;
                }
                wkr = 0.5 * Math.Sin(ec * i0);
                wki = 0.5 * Math.Cos(ec * i0);
                wdr = 0.5 - (wkr * w1r - wki * w1i);
                wdi = wkr * w1i + wki * w1r;
                wkr = 0.5 - wkr;
                i = i0;
            }
            xr = a[2] - a[n - 2];
            xi = a[3] + a[n - 1];
            yr = wdr * xr - wdi * xi;
            yi = wdr * xi + wdi * xr;
            a[2] -= yr;
            a[3] -= yi;
            a[n - 2] += yr;
            a[n - 1] -= yi;
        }

        static private void M_rftbsub(int n, double[] a)
        {
            int i, i0, j, k;
            double ec, w1r, w1i, wkr, wki, wdr, wdi, ss, xr, xi, yr, yi;

            ec = 2 * PiHalf / n;
            wkr = 0;
            wki = 0;
            wdi = Math.Cos(ec);
            wdr = Math.Sin(ec);
            wdi *= wdr;
            wdr *= wdr;
            w1r = 1 - 2 * wdr;
            w1i = 2 * wdi;
            ss = 2 * w1i;
            i = n >> 1;
            a[i + 1] = -a[i + 1];
            while (true)
            {
                i0 = i - 4 * RDFT_LOOP_DIV;
                if (i0 < 4)
                {
                    i0 = 4;
                }
                for (j = i - 4; j >= i0; j -= 4)
                {
                    k = n - j;
                    xr = a[j + 2] - a[k - 2];
                    xi = a[j + 3] + a[k - 1];
                    yr = wdr * xr + wdi * xi;
                    yi = wdr * xi - wdi * xr;
                    a[j + 2] -= yr;
                    a[j + 3] = yi - a[j + 3];
                    a[k - 2] += yr;
                    a[k - 1] = yi - a[k - 1];
                    wkr += ss * wdi;
                    wki += ss * (0.5 - wdr);
                    xr = a[j] - a[k];
                    xi = a[j + 1] + a[k + 1];
                    yr = wkr * xr + wki * xi;
                    yi = wkr * xi - wki * xr;
                    a[j] -= yr;
                    a[j + 1] = yi - a[j + 1];
                    a[k] += yr;
                    a[k + 1] = yi - a[k + 1];
                    wdr += ss * wki;
                    wdi += ss * (0.5 - wkr);
                }
                if (i0 == 4)
                {
                    break;
                }
                wkr = 0.5 * Math.Sin(ec * i0);
                wki = 0.5 * Math.Cos(ec * i0);
                wdr = 0.5 - (wkr * w1r - wki * w1i);
                wdi = wkr * w1i + wki * w1r;
                wkr = 0.5 - wkr;
                i = i0;
            }
            xr = a[2] - a[n - 2];
            xi = a[3] + a[n - 1];
            yr = wdr * xr + wdi * xi;
            yi = wdr * xi - wdi * xr;
            a[2] -= yr;
            a[3] = yi - a[3];
            a[n - 2] += yr;
            a[n - 1] = yi - a[n - 1];
            a[1] = -a[1];
        }

        static private void M_rdft(int n, int isgn, double[] a)
        {
            double xi;

            if (isgn >= 0)
            {
                if (n > 4)
                {
                    M_bitrv2(n, a);
                    M_cftfsub(n, a);
                    M_rftfsub(n, a);
                }
                else if (n == 4)
                {
                    M_cftfsub(n, a);
                }
                xi = a[0] - a[1];
                a[0] += a[1];
                a[1] = xi;
            }
            else
            {
                a[1] = 0.5 * (a[0] - a[1]);
                a[0] -= a[1];
                if (n > 4)
                {
                    M_rftbsub(n, a);
                    M_bitrv2(n, a);
                    M_cftbsub(n, a);
                }
                else if (n == 4)
                {
                    M_cftfsub(n, a);
                }
            }
        }
      
        static private void FastMulFFT(byte [] ww, byte [] uu, byte [] vv, int nbytes)
        {
            int    i, j, nn2, nn , w0;
            ulong  ul;
            double carry, nnr, dtemp;
            double [] a=null;
            double [] b=null;
             
            nn  = nbytes;
            nn2 = nbytes >> 1;

            if (nn > 8200)
            {                
                a = new double[nn + 8];  
                b = new double[nn + 8];                  
            }
            else
            {
                a = new double[8200]; 
                b = new double[8200]; 
            }

            i = 0;
            for (j=0; j < nn2; j++)
            {
                a[j] = (double)((int)uu[i] * 100 + uu[i+1]);
                b[j] = (double)((int)vv[i] * 100 + vv[i+1]);
                i += 2;
            }

            /* zero fill the second half of the arrays */
            for (j=nn2; j < nn; j++)
            {
                a[j] = 0.0;
                b[j] = 0.0;
            }

            /* perform the forward Fourier transforms for both numbers */
            M_rdft(nn, 1, a);
            M_rdft(nn, 1, b);

            /* perform the convolution ... */
            b[0] *= a[0];
            b[1] *= a[1];

            for (j=3; j <= nn; j += 2)
            {
                dtemp  = b[j-1];
                b[j-1] = dtemp * a[j-1] - b[j] * a[j];
                b[j]   = dtemp * a[j] + b[j] * a[j-1];
            }

            /* perform the inverse transform on the result */
            M_rdft(nn, -1, b);

            /* perform a final pass to release all the carries */
            /* we are still in base 10000 at this point        */

            carry = 0.0;
            j     = nn;
            nnr   = 2.0 / (double)nn;

            while (true)
            {
                dtemp = b[--j] * nnr + carry + 0.5;
                ul    = (ulong)(dtemp * 1.0E-4);
                carry = (double)ul;
                b[j]  = dtemp - carry * 10000.0;

                if (j == 0)
                {
                    break;
                }
            }

            /* copy result to our destination after converting back to base 100 */

            w0 = 0;
            byte div = 0, rem = 0;

            UnpackMult((int)ul, ref div, ref rem);

            ww[w0+0] = div;
            ww[w0+1] = rem;
             
            for (j=0; j <= (nn - 2); j++)
            {
                w0 += 2;

                UnpackMult((int)b[j], ref div, ref rem);

                ww[w0 + 0] = div;
                ww[w0 + 1] = rem;                 
            }
        }

        static private void FastMul(BigNumber aa, BigNumber bb, BigNumber rr)
        {             
            int	ii, k, nexp, sign;
            BigNumber M_ain = new BigNumber();
            BigNumber M_bin = new BigNumber();

            BigNumber.Copy(aa, M_ain);
            BigNumber.Copy(bb, M_bin);

            int size_flag = GetSizeofInt();
            int bit_limit = 8 * size_flag + 1;
              
         
            sign = M_ain.signum * M_bin.signum;
            nexp = M_ain.exponent + M_bin.exponent;

            if (M_ain.dataLength >= M_bin.dataLength)
              ii = M_ain.dataLength;
            else
              ii = M_bin.dataLength;

            ii = (ii + 1) >> 1;
            ii = NextPowerOfTwo(ii);
             
            k = 2 * ii;                   /* required size of result, in bytes  */

            BigNumber.Pad(M_ain, k);          /* fill out the data so the number of */
            BigNumber.Pad(M_bin, k);          /* bytes is an exact power of 2       */

            if (k > rr.mantissa.Length)
            {
                BigNumber.Expand(rr, (k + 32));                 
            }

            BigNumber.FastMulFFT(rr.mantissa, M_ain.mantissa, M_bin.mantissa, ii);
             
            rr.signum     = (sbyte)sign;
            rr.exponent   = nexp;
            rr.dataLength = 4 * ii;

            BigNumber.Normalize(rr);
        }

        static private void Mul(BigNumber aa, BigNumber bb, BigNumber rr)
        {
            int ai, itmp,nexp, ii, jj, indexa, indexb, index0, numdigits;
            sbyte sign;
            
            sign = (sbyte)(aa.signum * bb.signum);
            nexp = aa.exponent + bb.exponent;

            if (sign == 0)       
            {
                BigNumber.SetZero(rr);
                return;
            }

            numdigits = aa.dataLength + bb.dataLength;
            indexa = (aa.dataLength + 1) >> 1;
            indexb = (bb.dataLength + 1) >> 1;
             
            if (indexa >= 48 && indexb >= 48)
            {
                BigNumber.FastMul(  aa, bb,rr);
                return;
            }  

            ii = (numdigits + 1) >> 1;    

            if (ii >= rr.mantissa.Length)
            {
                BigNumber.Expand(rr, ii + 32);
            }

            index0 = indexa + indexb;
            for (int i = 0; i < index0; i++)
            {
                rr.mantissa[i] = 0;
            }
            
            ii = indexa;

            while (true)
            {
                index0--;
                int crp = index0;
                jj = indexb;
                ai = (int)aa.mantissa[--ii];

                while (true)
                {
                    itmp = ai * bb.mantissa[--jj];

                    rr.mantissa[crp - 1] += s_MsbLookupMult[itmp];
                    rr.mantissa[crp]     += s_LsbLookupMult[itmp];

                    if (rr.mantissa[crp] >= 100)
                    {
                        rr.mantissa[crp]   -= 100;
                        rr.mantissa[crp-1] += 1;
                    }

                    crp--;

                    if (rr.mantissa[crp] >= 100)
                    {
                        rr.mantissa[crp]   -= 100;
                        rr.mantissa[crp-1] += 1;
                    }

                    if (jj == 0)
                    {
                        break;
                    }
                }

                if (ii == 0)
                {
                    break;
                }
            }

            rr.signum  = sign;
            rr.exponent = nexp;
            rr.dataLength = numdigits;

            BigNumber.Normalize(rr);
             
        }

        static private void Div(BigNumber aa, BigNumber bb, BigNumber rr, int places)
        {
            int j, k, m, b0, nexp, indexr, icompare, iterations;
            sbyte sign;
            long trial_numer;

            BigNumber M_div_worka = new BigNumber();
            BigNumber M_div_workb = new BigNumber();
            BigNumber M_div_tmp7 = new BigNumber();
            BigNumber M_div_tmp8 = new BigNumber();
            BigNumber M_div_tmp9 = new BigNumber();

            sign = (sbyte)(aa.signum * bb.signum);

            if (sign == 0)      /* one number is zero, result is zero */
            {
                if (bb.signum == 0)
                {
                    throw new BigNumberException("Division by Zero");
                }
                BigNumber.SetZero(rr);
                return;
            }

            if (bb.mantissa[0] >= 50)
            {
                BigNumber.Abs(M_div_worka, aa);
                BigNumber.Abs(M_div_workb, bb);
            }
            else       /* 'normal' step D1 */
            {
                k = 100 / (bb.mantissa[0] + 1);
                BigNumber.SetFromLong(M_div_tmp9, (long)k);

                BigNumber.Mul(M_div_tmp9, aa, M_div_worka);
                BigNumber.Mul(M_div_tmp9, bb, M_div_workb);

                M_div_worka.signum = 1;
                M_div_workb.signum = 1;
            }

            b0 = 100 * (int)M_div_workb.mantissa[0];

            if (M_div_workb.dataLength >= 3)
            {
                b0 += M_div_workb.mantissa[1];
            }

            nexp = M_div_worka.exponent - M_div_workb.exponent;

            if (nexp > 0)
            {
                iterations = nexp + places + 1;
            }
            else
            {
                iterations = places + 1;
            }

            k = (iterations + 1) >> 1;     /* required size of result, in bytes */

            if (k > rr.mantissa.Length)
            {
                BigNumber.Expand(rr, k + 32);
            }

            /* clear the exponent in the working copies */

            M_div_worka.exponent = 0;
            M_div_workb.exponent = 0;

            /* if numbers are equal, ratio == 1.00000... */

            if ((icompare = BigNumber.Compare(M_div_worka, M_div_workb)) == 0)
            {
                iterations = 1;
                rr.mantissa[0] = 10;
                nexp++;
            }
            else			           /* ratio not 1, do the real division */
            {
                if (icompare == 1)                        /* numerator > denominator */
                {
                    nexp++;                           /* to adjust the final exponent */
                    M_div_worka.exponent += 1;     /* multiply numerator by 10 */
                }
                else                                      /* numerator < denominator */
                {
                    M_div_worka.exponent += 2;    /* multiply numerator by 100 */
                }

                indexr = 0;
                m = 0;

                while (true)
                {
                    /*
                     *  Knuth step D3. Only use the 3rd -> 6th digits if the number
                     *  actually has that many digits.
                     */

                    trial_numer = 10000L * (long)M_div_worka.mantissa[0];

                    if (M_div_worka.dataLength >= 5)
                    {
                        trial_numer += 100 * M_div_worka.mantissa[1] + M_div_worka.mantissa[2];
                    }
                    else
                    {
                        if (M_div_worka.dataLength >= 3)
                        {
                            trial_numer += 100 * M_div_worka.mantissa[1];
                        }
                    }

                    j = (int)(trial_numer / b0);

                    /* 
                     *    Since the library 'normalizes' all the results, we need
                     *    to look at the exponent of the number to decide if we 
                     *    have a lead in 0n or 00.
                     */

                    if ((k = 2 - M_div_worka.exponent) > 0)
                    {
                        while (true)
                        {
                            j /= 10;
                            if (--k == 0)
                                break;
                        }
                    }

                    if (j == 100)       /* qhat == base ??      */
                        j = 99;         /* if so, decrease by 1 */

                    BigNumber.SetFromLong(M_div_tmp8, (long)j);
                    BigNumber.Mul(M_div_tmp8, M_div_workb, M_div_tmp7);

                    /*
                     *    Compare our q-hat (j) against the desired number.
                     *    j is either correct, 1 too large, or 2 too large
                     *    per Theorem B on pg 272 of Art of Compter Programming,
                     *    Volume 2, 3rd Edition.
                     *    
                     *    The above statement is only true if using the 2 leading
                     *    digits of the numerator and the leading digit of the 
                     *    denominator. Since we are using the (3) leading digits
                     *    of the numerator and the (2) leading digits of the 
                     *    denominator, we eliminate the case where our q-hat is 
                     *    2 too large, (and q-hat being 1 too large is quite remote).
                     */

                    if (BigNumber.Compare(M_div_tmp7, M_div_worka) == 1)
                    {
                        j--;
                        BigNumber.Sub(M_div_tmp7, M_div_workb, M_div_tmp8);
                        BigNumber.Copy(M_div_tmp8, M_div_tmp7);
                    }

                    /* 
                     *  Since we know q-hat is correct, step D6 is unnecessary.
                     *
                     *  Store q-hat, step D5. Since D6 is unnecessary, we can 
                     *  do D5 before D4 and decide if we are done.
                     */

                    rr.mantissa[indexr++] = (byte)j;    /* j == 'qhat' */
                    m += 2;

                    if (m >= iterations)
                        break;

                    /* step D4 */

                    BigNumber.Sub(M_div_worka, M_div_tmp7, M_div_tmp9);

                    /*
                     *  if the subtraction yields zero, the division is exact
                     *  and we are done early.
                     */

                    if (M_div_tmp9.signum == 0)
                    {
                        iterations = m;
                        break;
                    }

                    /* multiply by 100 and re-save */
                    M_div_tmp9.exponent += 2;
                    BigNumber.Copy(M_div_tmp9, M_div_worka);
                }
            }

            rr.signum = sign;
            rr.exponent = nexp;
            rr.dataLength = iterations;

            BigNumber.Normalize(rr);
        }

        static public void Div(BigNumber aa, BigNumber bb, BigNumber rr)
        {
            int places = BigNumber.MaxDigits(aa, bb);
            BigNumber.Div(aa, bb, rr, places);             
        }

    } // class
} // namespace

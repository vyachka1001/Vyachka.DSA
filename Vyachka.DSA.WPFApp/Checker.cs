using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Vyachka.DSA.WPFApp
{
    public static class Checker
    {
        public static bool CheckP(string qStr, string pStr)
        {
            BigInteger p = BigInteger.Parse(pStr);
            if (!MillerRabinTest(p, 10))
            {
                return false;
            }

            BigInteger q = BigInteger.Parse(qStr);
            return (p - 1) % q == 0;
        }

        public static bool CheckH(string qStr, string pStr, string hStr)
        {
            BigInteger h = BigInteger.Parse(hStr);
            BigInteger p = BigInteger.Parse(pStr);
            if (h < 1 || h > (p - 1))
            {
                return false;
            }

            BigInteger q = BigInteger.Parse(qStr);
            BigInteger g = FastExp(h, (p - 1) / q, p);
            if (g < 1)
            {
                return false;
            }

            return true;
        }

        public static bool CheckIsInInterval(string leftPart, string rightPart, string valueStr)
        {
            BigInteger value = BigInteger.Parse(valueStr);
            BigInteger right = BigInteger.Parse(rightPart);
            BigInteger left = BigInteger.Parse(leftPart);
           
            if (value < left || value > right)
            {
                return false;
            }

            return true;
        }

        public static bool MillerRabinTest(BigInteger n, int k)
        {
            if (n == 2 || n == 3)
            {
                return true;
            }

            if (n % 2 == 0)
            {
                return false;
            }

            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] arr = new byte[n.ToByteArray().LongLength];
                BigInteger a;

                do
                {
                    rng.GetBytes(arr);
                    a = new BigInteger(arr);
                }
                while (a < 2 || a >= n - 2);

                BigInteger x = BigInteger.ModPow(a, t, n);
                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;

                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                {
                    return false;
                }
            }

            return true;
        }

        private static BigInteger FastExp(BigInteger number, BigInteger power, BigInteger mod)
        {
            BigInteger x = 1;
            while (power != 0)
            {
                while (power % 2 == 0)
                {
                    power /= 2;
                    number = (number * number) % mod;
                }

                power--;
                x = (x * number) % mod;
            }

            return x;
        }
    }
}

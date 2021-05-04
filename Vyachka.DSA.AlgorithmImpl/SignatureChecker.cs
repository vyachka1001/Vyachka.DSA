using System;
using System.Numerics;

namespace Vyachka.DSA.AlgorithmImpl
{
    public class SignatureChecker
    {
        public bool CheckSign(byte[] msg, BigInteger r, BigInteger s, BigInteger q,  BigInteger p, 
                              BigInteger h, out BigInteger v)
        {
            //s−1mod q  = sq-2 mod q
            BigInteger w = Helper.FastExp(s, q - 2, q);
            BigInteger hashImage = Helper.CountHashImage(msg);
            BigInteger u1 = hashImage * w % q;
            BigInteger u2 = r * w % q;
            BigInteger g = Helper.FastExp(h, (p - 1) / q, p);
            v = Helper.FastExp(g, u1, p) * Helper.FastExp(g, u2, p) % q;
            Console.WriteLine($"w:{w} hash:{hashImage} u1:{u1} u2:{u2} g:{g}")
            return v == r;
        }
    }
}

using System.Numerics;

namespace Vyachka.DSA.AlgorithmImpl
{
    public static class Signer
    {
        public static bool SignInitialMsg(byte[] initialMsg, BigInteger q, BigInteger p, BigInteger k, BigInteger x, BigInteger h, out BigInteger r, out BigInteger s)
        {
            BigInteger hashImage = Helper.CountHashImage(initialMsg);
            BigInteger g = Helper.FastExp(h, (p - 1) / q, p);
            r = Helper.FastExp(g, k, p) % q;
            s = Helper.FastExp(k, q - 2, q) * (hashImage + x * r) % q;
            return (r == 0 || s == 0);
        }

        public static bool CheckSign(byte[] msg, BigInteger r, BigInteger s, BigInteger q, BigInteger p,
                              BigInteger h, BigInteger x, out BigInteger v)
        {
            BigInteger w = Helper.FastExp(s, q - 2, q);
            BigInteger hashImage = Helper.CountHashImage(msg);
            BigInteger u1 = hashImage * w % q;
            BigInteger u2 = r * w % q;
            BigInteger g = Helper.FastExp(h, (p - 1) / q, p);
            BigInteger y = Helper.FastExp(g, x, p);
            v = (Helper.FastExp(g, u1, p) * Helper.FastExp(y, u2, p) % p) % q;
            //Console.WriteLine($"w:{w} hash:{hashImage} u1:{u1} u2:{u2} g:{g} v:{v} y:{y}");
            return v == r;
        }
    }
}

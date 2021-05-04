using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Vyachka.DSA.AlgorithmImpl
{
    public class Signer
    {
        private BigInteger Q { get; set; }
        private BigInteger P { get; set; }
        private BigInteger K { get; set; }
        private BigInteger X { get; set; }
        private BigInteger H { get; set; } 

        public Signer(BigInteger q, BigInteger p, BigInteger k, BigInteger x, BigInteger h)
        {
            Q = q;
            P = p;
            K = k;
            X = x;
            H = h;
        }

        public bool SignInitialMsg(byte[] initialMsg, out BigInteger r, out BigInteger s)
        {
            BigInteger hashImage = Helper.CountHashImage(initialMsg);
            BigInteger g = Helper.FastExp(H, (P - 1) / Q, P);
            r = CountR(g);
            s = CountS(hashImage, r);
            return (r == 0 || s == 0)
           
        }

        private BigInteger CountR(BigInteger g)
        {
            //r = (g^k mod p) mod q
            return Helper.FastExp(g, K, P) % Q;
        }

        private BigInteger CountS(BigInteger hashImage, BigInteger r)
        {
            //s = k^(−1)(h(M) + x * r) mod q
            return ((hashImage + X * r) / K) % Q;
        }
    }
}

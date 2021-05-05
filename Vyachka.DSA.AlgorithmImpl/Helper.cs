using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Vyachka.DSA.AlgorithmImpl
{
    public static class Helper
    {
        public static BigInteger FastExp(BigInteger number, BigInteger power, BigInteger mod)
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

        public static BigInteger CountHashImage(byte[] initialMsg)
        {
            for (int i = 0; i < initialMsg.Length; i++)
            {
                Console.Write(initialMsg[i] + " ");
            }

            Console.WriteLine();
            HashAlgorithm sha = SHA1.Create();
            byte[] result = sha.ComputeHash(initialMsg);
            Array.Reverse(result);
            byte[] uResult = new byte[result.Length + 1];
            result.CopyTo(uResult, 0);
            uResult[^1] = 0;
            BigInteger hash = new BigInteger(uResult);
            Console.WriteLine($"hash: {hash}");
            //Hi = (Hi−1 + Mi)2 mod n
            /*int hash = 100;
            for (int i = 0; i < initialMsg.Length; i++)
            {
                hash = (hash + initialMsg[i]) * (hash + initialMsg[i]) % 323;
            }*/
            return hash;
        }

    }
}

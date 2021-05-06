using System;
using System.Collections;
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
            int amountOfBlocks = initialMsg.Length / 64;
            int amountOfRemainingBits = initialMsg.Length % 64 * 8;

            int startIndex = amountOfBlocks * 64;
            int amountOfBytes = initialMsg.Length - startIndex;
            byte[] remainingBytes = new byte[amountOfBytes];
            for (int i = startIndex; i < initialMsg.Length; i++)
            {
                remainingBytes[i - startIndex] = initialMsg[i];
            }

            BitArray remainingBits = new BitArray(remainingBytes);
            if (amountOfRemainingBits > 447)
            {
                //тут пока хз
            }
            else
            {
                string bits = remainingBits.ToString();
                bits += '1';
                for(int i = 0; i < 447 - amountOfRemainingBits - 1; i++)
                {
                    bits += '0';
                }

                //как-то пихать длину
            }

            /*for (int i = 0; i < initialMsg.Length; i++)
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
            Console.WriteLine($"hash: {hash}");*/

            /*int hash = 100;
            for (int i = 0; i < initialMsg.Length; i++)
            {
                hash = (hash + initialMsg[i]) * (hash + initialMsg[i]) % 323;
            }*/

            return hash;
        }

        /*
        public static void processBlock(uint[] block, uint[] hash, uint[] bigarray)
        {
            uint temp = 0;
            const uint k0 = 0x5a827999;
            const uint k1 = 0x6ed9eba1;
            const uint k2 = 0x8f1bbcdc;
            const uint k3 = 0xca62c1d6;
            int t = 0;
            for (t = 0; t < 16; t++)
            {
                bigarray[t] = block[t];
            }

            for (t = 16; t < 80; t++)
            {
                bigarray[t] = circularShift(1, (bigarray[t - 3] ^ bigarray[t - 8] ^ bigarray[t - 14] ^ bigarray[t - 16]));
            }

            uint A = hash[0];
            uint B = hash[1];
            uint C = hash[2];
            uint D = hash[3];
            uint E = hash[4];

            for (t = 0; t < 20; t++)
            {
                temp = circularShift(5, A) + ((B & C) | ((~B) & D)) + E + bigarray[t] + k0;
                E = D;
                D = C;
                C = circularShift(30, B);
                B = A;
                A = temp;
            }

            for (t = 20; t < 40; t++)
            {
                temp = circularShift(5, A) + (B ^ C ^ D) + E + bigarray[t] + k1;
                E = D;
                D = C;
                C = circularShift(30, B);
                B = A;
                A = temp;
            }

            for (t = 40; t < 60; t++)
            {
                temp = circularShift(5, A) + ((B & C) | (B & D) | (C & D)) + E + bigarray[t] + k2;
                E = D;
                D = C;
                C = circularShift(30, B);
                B = A;
                A = temp;
            }

            for (t = 60; t < 80; t++)
            {
                temp = circularShift(5, A) + (B ^ C ^ D) + E + bigarray[t] + k3;
                E = D;
                D = C;
                C = circularShift(30, B);
                B = A;
                A = temp;
            }

            hash[0] += A;
            hash[1] += B;
            hash[2] += C;
            hash[3] += D;
            hash[4] += E;   
        }
        */

    }
}

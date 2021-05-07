using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            byte[] result = Sha1Algorithm(initialMsg);
            Array.Reverse(result);
            byte[] uResult = new byte[result.Length + 1];
            result.CopyTo(uResult, 0);
            uResult[^1] = 0;
            BigInteger hash = new BigInteger(uResult);

            return hash;
        }

        static byte[] Sha1Algorithm(byte[] plaintext)
        {
            Block512[] blocks = ConvertPaddedTextToBlockArray(PadPlainText(plaintext));
            uint[] H = { 0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0 };
            uint[] K = DefineK();

            for (int i = 0; i < blocks.Length; i++)
            {
                uint[] W = CreateMessageScheduleSha1(blocks[i]);
                
                uint a = H[0];
                uint b = H[1];
                uint c = H[2];
                uint d = H[3];
                uint e = H[4];

                for (int t = 0; t < 80; t++)
                {
                    uint T = RotL(5, a) + F(t, b, c, d) + e + K[t] + W[t];
                    e = d;
                    d = c;
                    c = RotL(30, b);
                    b = a;
                    a = T;
                }

                H[0] += a;
                H[1] += b;
                H[2] += c;
                H[3] += d;
                H[4] += e;
            }

            return UIntArrayToByteArray(H);
        }

        private static uint[] DefineK()
        {
            uint[] k = new uint[80];

            for (int i = 0; i < 80; i++)
            {
                if (i <= 19)
                {
                    k[i] = 0x5a827999;
                }
                else if (i <= 39)
                {
                    k[i] = 0x6ed9eba1;
                }
                else if (i <= 59)
                {
                    k[i] = 0x8f1bbcdc;
                }
                else
                {
                    k[i] = 0xca62c1d6;
                }
            }

            return k;
        }

        static uint F(int t, uint x, uint y, uint z)
        {
            if (t >= 0 && t <= 19)
            {
                return F1(x, y, z);
            }
            else if (t >= 20 && t <= 39)
            {
                return F3(x, y, z);
            }
            else if (t >= 40 && t <= 59)
            {
                return F2(x, y, z);
            }
            else if (t >= 60 && t <= 79)
            {
                return F3(x, y, z);
            }
            else
            {
                return 0;
            }
        }

        static uint F1(uint x, uint y, uint z)
        {
            return (x & y) ^ (~x & z);
        }

        static uint F2(uint x, uint y, uint z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }

        static uint F3(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }

        private static byte[] PadPlainText(byte[] plaintext)
        {
            int numberBits = plaintext.Length * 8;
            int t = (numberBits + 8 + 64) / 512;
            int k = 512 * (t + 1) - (numberBits + 8 + 64);
            int n = k / 8;

            List<byte> paddedtext = plaintext.ToList();
            paddedtext.Add(0x80);
            for (int i = 0; i < n; i++)
            {
                paddedtext.Add(0);
            }

            byte[] b = BitConverter.GetBytes((ulong)numberBits);
            Array.Reverse(b);

            for (int i = 0; i < b.Length; i++)
            {
                paddedtext.Add(b[i]);
            }

            return paddedtext.ToArray();
        }

        static Block512[] ConvertPaddedTextToBlockArray(byte[] paddedtext)
        {
            int numberBlocks = (paddedtext.Length * 8) / 512;
            Block512[] blocks = new Block512[numberBlocks];

            for (int i = 0; i < numberBlocks; i++)
            {
                byte[] b = new byte[64];
                for (int j = 0; j < 64; j++)
                {
                    b[j] = paddedtext[i * 64 + j];
                }

                uint[] words = ByteArrayToUIntArray(b);
                blocks[i] = new Block512(words);
            }

            return blocks;
        }

        public static uint[] ByteArrayToUIntArray(byte[] b)
        {
            int numberBytes = b.Length;
            int n = numberBytes / 4;
            uint[] uintArray = new uint[n];

            for (int i = 0; i < n; i++)
            {
                uintArray[i] = ByteArrayToUInt(b, 4 * i);
            }

            return uintArray;
        }

        public static uint ByteArrayToUInt(byte[] b, int startIndex)
        {
            uint c = 256;
            uint output = 0;

            for (int i = startIndex; i < startIndex + 4; i++)
            {
                output = output * c + b[i];
            }

            return output;
        }

        static uint[] CreateMessageScheduleSha1(Block512 block)
        {
            uint[] W = new uint[80];
            for (int t = 0; t < 80; t++)
            {
                if (t < 16)
                {
                    W[t] = block.words[t];
                }
                else
                {
                    W[t] = RotL(1, W[t - 3] ^ W[t - 8] ^ W[t - 14] ^ W[t - 16]);
                }
            }

            return W;
        }

        static uint RotL(int n, uint x)
        {
            return (x << n) | (x >> 32 - n);
        }

        public static byte[] UIntArrayToByteArray(uint[] words)
        {
            List<byte> b = new List<byte>();

            for (int i = 0; i < words.Length; i++)
            {
                b.AddRange(UIntToByteArray(words[i]));
            }

            return b.ToArray();
        }

        public static byte[] UIntToByteArray(uint x)
        {
            byte[] b = BitConverter.GetBytes(x);
            Array.Reverse(b);
            return b;
        }

        /*Console.WriteLine();
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
    }

    public class Block512
    {
        public uint[] words;

        public Block512(uint[] words)
        {
            if (words.Length == 16)
            {
                this.words = words;
            }
            else
            {
                Console.WriteLine("ERROR: A block must be 16 words");
                this.words = null;
            }
        }
    }
}

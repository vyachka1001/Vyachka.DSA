using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Vyachka.DSA.AlgorithmImpl;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string pStr = "898846567431157967424297114057633644601771516927834298008846524493109792637522" +
                          "535293491954598238817151457964980464592383454281215613866269456797539564000773" +
                          "528820716639254597505008070182540287714904340213156913571237346370468948761234" +
                          "96168716251735252662742462099334802433058472377674408598573487858308054417";
            BigInteger p = BigInteger.Parse(pStr);

            string qStr = "1193447034984784682329306571139467195163334221569";
            BigInteger q = BigInteger.Parse(qStr);

            string input = "BSUIR";
            /*Signer.SignInitialMsg(Encoding.ASCII.GetBytes(input), q: q, p: p, k: 6274, x: 11934, h: 3, out BigInteger r, out BigInteger s);
            Console.WriteLine(r + "," + s);*/
            Console.WriteLine($"Hash myself = {Helper.CountHashImage(Encoding.ASCII.GetBytes(input))}");
            HashAlgorithm sha = SHA1.Create();
            byte[] result = sha.ComputeHash(Encoding.ASCII.GetBytes(input));
            Array.Reverse(result);
            byte[] uResult = new byte[result.Length + 1];
            result.CopyTo(uResult, 0);
            uResult[^1] = 0;
            BigInteger hash = new BigInteger(uResult);
            Console.WriteLine($"hash        : {hash}");
            /*Console.WriteLine(Signer.CheckSign(Encoding.ASCII.GetBytes(input), r, s, q: q, p: p, h: 3, x: 11934, out BigInteger v));*/
            Console.ReadKey();
        }
    }
}

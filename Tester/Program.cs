using System;
using System.Numerics;
using System.Text;
using Vyachka.DSA.AlgorithmImpl;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            /* string pStr = "898846567431157967424297114057633644601771516927834298008846524493109792637522" +
                           "535293491954598238817151457964980464592383454281215613866269456797539564000773" +
                           "528820716639254597505008070182540287714904340213156913571237346370468948761234" +
                           "96168716251735252662742462099334802433058472377674408598573487858308054417";
             BigInteger p = BigInteger.Parse(pStr);

             string qStr = "1193447034984784682329306571139467195163334221569";
             BigInteger q = BigInteger.Parse(qStr);*/


            Signer signer = new Signer(q: 107, p: 643, k: 31, x: 45, h: 0) ;
            string input = "BSUIR";
            signer.SignInitialMsg(Encoding.ASCII.GetBytes(input), out BigInteger r, out BigInteger s);
            Console.WriteLine(r + "," + s);
            SignatureChecker checker = new SignatureChecker();
            Console.WriteLine(checker.CheckSign(Encoding.ASCII.GetBytes(input), r, s,107 , , 3, out BigInteger v));
            Console.ReadKey();
        }
    }
}

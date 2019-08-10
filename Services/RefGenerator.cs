using HashidsNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RefGenerator : IRefGen
    {
        readonly Hashids _hashAppRef = new Hashids(
            "'Fear is the path to the dark side. Fear leads to anger; anger leads to hate; hate leads to suffering. I sense much fear in you.' — Yoda",
            8,
            alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

        readonly Hashids _hashTxnGroupRef = new Hashids(
            "'The dark side of the Force is a pathway to many abilities some consider to be unnatural.' — Chancellor Palpatine",
            8,
            alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

        readonly Hashids _hashTxnRef = new Hashids(
            "'You can’t stop the change, any more than you can stop the suns from setting.' — Shmi Skywalker",
            8,
            alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

        public string GenAppRef(long num)
        {
            return _hashAppRef.EncodeLong(num);
        }

        public string GenTxnGroupRef(long num)
        {
            return _hashTxnGroupRef.EncodeLong(num);
        }

        public string GenTxnRef(long num)
        {
            return _hashTxnRef.EncodeLong(num);
        }
    }
}

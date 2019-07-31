using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Embily.Gateways.CoinPayInTh.Models;
using Newtonsoft.Json;

namespace Embily.Gateways.CoinPayInTh
{
    public class Signature
    {
        protected string _secret;

        public Signature(string secret = "965f85b24c04") //965f85b24c04
        {
            _secret = secret;
        }

        public string Generate(Dictionary<string, object> data)
        {
            data.Add("secret", _secret);


            foreach (var var in data)
            {
                if (var.Value.GetType().IsArray)
                {
                    Dictionary<string, string> ar = (Dictionary<string, string>)var.Value;
                    Dictionary<string, string> arRes = new Dictionary<string, string>();
                    var list = ar.Keys.ToList();
                    list.Sort();
                    foreach (var key in list)
                    {
                        arRes.Add(key, ar[key]);
                    }
                    data[var.Key] = arRes;
                }
            }


            var str = JsonConvert.SerializeObject(data);

            HashAlgorithm algorithm = SHA1.Create();  // SHA1.Create()
            var h = algorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
            return HexStringFromBytes(h);

        }


        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public bool Check(ResponseCallbacks callbacks)
        {
            bool ch = false;

            if(string.IsNullOrEmpty(callbacks.Signature))
            {
                return false;
            }
            //string signature = "";
            //try
            //{
            //    signature = callbacks.Signature;
            //}
            //catch (NullReferenceException e)
            //{
            //    return false;
            //    // Stuff to do if the code inside the try-block produces an error.
            //}
            //data.Remove("signature");
            var data = new Dictionary<string, object>()
            {
                {"order_id", callbacks.OrderId},
                {"message", callbacks.Message},
                {"confirmed_in_full", callbacks.ConfirmedInFull}
            };

            //{ "order_id":"8","message":"paid in full","confirmed_in_full":true,"signature":"786a9b1dde3deed19c94c1989c2c240659c48723"}
            //{ "order_id":"8","message":"paid in full","confirmed_in_full":true,"secret":"5f58c877653f"}
            //data.Add("signature", "");

            string correctSignature = Generate(data);

            if (callbacks.Signature == correctSignature)
            {
                ch = true;
            }
            return ch;
        }

    } 
}




//class Signature
//{
//    protected $secret;

//    public function __construct($secret)
//    {
//      $this->secter = $secret;
//    }

//    /**
//     * Generate signature
//     * @param array $data
//     * @return string
//     */
//    public function generate($data)
//    {
//        $data['secret'] = $this->secret; // your API Key

//        // Sort to have always the same order in signature.
//        $data = array_walk_recursive($data, function($item) {
//            if (is_array($item))
//            {
//                return ksort($item);
//            }
//            return $item;
//        });

//        $signable_string = json_encode($data); // Convert into json
//        return sha1($signable_string); // Get SHA-1 hash
//    }

//    public function check($data)
//    {
//        if (!isset($data['signature']))
//        {
//            return false;
//        }

//       $signature = $data['signature'];
//       $correctSignature = $this->generate($this->clean($data));

//        return $signature === $correctSignature;
//    }

//    protected function clean($data)
//    {
//        unset($data['signature']);
//        return $data;
//    }
//}
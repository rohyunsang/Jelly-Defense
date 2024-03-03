// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("nhp9n0YXrxWTx2j2rhS0OLFdzLe0dFBcb69PBpuPzrA657Hoe4URrmqtabeRwREH1LuGL2HsIS7CBG8J6Ea6H1MSEIQnKWF6g8Q4wfRA3OgWVazrW8zu5AIVe5LoeCwDhzBMcedANacdH/Hos6LBaT+h6tpKlfm7CBZp2/6/07Pf6crIBJSaxBQUxobr+VpNvqwQvUunRofE5As7PcslIT14WqGlmlqT3dynX7aPrPKI0rqtCTAQtN12eTtwrcpvMeaFzIGdFxQdrywPHSArJAerZavaICwsLCgtLjp4O4rZkgZFnL3Ogd5QyVirATB3v2TxZIt+NKCH92V0pK1XEfFMbZevLCItHa8sJy+vLCwt6fQPTbiW2oQb4B9Xlx/qEi8uLC0s");
        private static int[] order = new int[] { 8,9,7,5,5,13,13,10,10,13,10,12,12,13,14 };
        private static int key = 45;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

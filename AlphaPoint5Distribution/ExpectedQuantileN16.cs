using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class ExpectedQuantileN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results/quantile_precision142.csv")) {
                sw.WriteLine("quantile,x");

                for (int k = 5000; k > 1; k--) {
                    MultiPrecision<Pow2.N16> p = MultiPrecision<Pow2.N16>.Div(k, 10000);
                    MultiPrecision<Pow2.N16> x = QuantilePadeN16.Value(p);

                    Console.WriteLine($"{1 - p},{x:e142}");
                    sw.WriteLine($"{1 - p},{x:e142}");
                }

                for (long k = 10000; k < 1000000000000000000L; k *= 10) {
                    MultiPrecision<Pow2.N16> p = MultiPrecision<Pow2.N16>.Div(1, k);
                    MultiPrecision<Pow2.N16> x = QuantilePadeN16.Value(p);

                    Console.WriteLine($"{1 - p},{x:e142}");
                    sw.WriteLine($"{1 - p},{x:e142}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

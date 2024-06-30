using MultiPrecision;
using SaSPoint5Expected;
using SaSPoint5PadeApprox;

namespace SaSPoint5EvalPadeApprox {
    internal class EvalQuantilePadeN16 {
        static void Main_() {
            MultiPrecision<Pow2.N16> max_err = "1e-142";

            using (StreamWriter sw = new("../../../../results_disused/quantile_pade_eval.csv")) {
                sw.WriteLine("u:=-log2(p),q:=cquantile(p),ccdf(q),error");

                for (MultiPrecision<Pow2.N16> u0 = 1; u0 < 2048; u0 *= 2) {
                    for (MultiPrecision<Pow2.N16> u = u0; u < u0 * 2; u += u0 / (u0 < 4 ? 65536 : 4096)) {
                        MultiPrecision<Pow2.N16> p = MultiPrecision<Pow2.N16>.Pow2(-u);

                        MultiPrecision<Pow2.N16> x = QuantilePadeN16.Value(p);
                        MultiPrecision<Pow2.N16> y = CDFN16.Value(x, complementary: true);

                        MultiPrecision<Pow2.N16> ccdf_error = MultiPrecision<Pow2.N16>.Abs(p - y) / p;

                        Console.WriteLine($"{u}\n{x}\n{y}\n{ccdf_error:e10}");

                        sw.WriteLine($"{u},{x},{y},{ccdf_error:e20}");

                        if (ccdf_error >= max_err) {
                            Console.ReadLine();
                        }
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

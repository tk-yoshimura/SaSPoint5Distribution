using MultiPrecision;
using MultiPrecisionRootFinding;
using SaSPoint5Expected;

namespace SaSPoint5EvalExpected {
    internal class ExpectedQuantileLogLogN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/quantile_precision150_loglog_2.csv")) {
                sw.WriteLine("u:=-log2(p),v:=log2(quantile(p)+1)");

                MultiPrecision<Pow2.N16> x = 0;

                for (MultiPrecision<Pow2.N16> u0 = 1; u0 < 2048; u0 *= 2) {
                    for (MultiPrecision<Pow2.N16> u = u0; u < u0 * 2; u += u0 / (u0 < 4 ? 16384 : 4096)) {
                        MultiPrecision<Pow2.N16> p = MultiPrecision<Pow2.N16>.Pow2(-u);

                        x = NewtonRaphsonFinder<Pow2.N16>.RootFind(
                            x => (CDFN16.Value(x, complementary: true) - p, -PDFN16.Value(x.Convert<Pow2.N16>()).Convert<Pow2.N16>()),
                            x0: x, accurate_bits: 508, overshoot_decay: true
                        );

                        MultiPrecision<Pow2.N16> v = MultiPrecision<Pow2.N32>.Log2(x.Convert<Pow2.N32>() + 1).Convert<Pow2.N16>();

                        MultiPrecision<Pow2.N16> z = MultiPrecision<Pow2.N16>.Log2(CDFN16.Value(x, complementary: true));

                        Console.WriteLine($"{u}\n{v}\n");

                        sw.WriteLine($"{u},{v}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

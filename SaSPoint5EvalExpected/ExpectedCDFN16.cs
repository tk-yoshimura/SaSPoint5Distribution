using MultiPrecision;
using SaSPoint5Expected;

namespace SaSPoint5EvalExpected {
    internal class ExpectedCDFPadeN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results/cdf_precision150.csv")) {
                sw.WriteLine("x,cdf(x)-1/2,ccdf(x)");

                for (double x = 0; x < 1; x += 1d / 1024) {
                    MultiPrecision<Pow2.N16> y = CDFN16.Value(x, complementary: false);
                    MultiPrecision<Pow2.N16> yc = CDFN16.Value(x, complementary: true);

                    Console.WriteLine($"{x}\n{y}\n{yc}");
                    sw.WriteLine($"{x},{y},{yc}");
                }

                for (double x0 = 1; x0 < 4096; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 512) {
                        MultiPrecision<Pow2.N16> y = CDFN16.Value(x, complementary: false);
                        MultiPrecision<Pow2.N16> yc = CDFN16.Value(x, complementary: true);

                        Console.WriteLine($"{x}\n{y}\n{yc}");
                        sw.WriteLine($"{x},{y},{yc}");
                    }
                }

                for (MultiPrecision<Pow2.N16> x = MultiPrecision<Pow2.N16>.Ldexp(1, 12); x.Exponent <= 1024; x *= 2) {
                    MultiPrecision<Pow2.N16> y = CDFN16.Value(x, complementary: false);
                    MultiPrecision<Pow2.N16> yc = CDFN16.Value(x, complementary: true);

                    Console.WriteLine($"{x}\n{y}\n{yc}");
                    sw.WriteLine($"2^{x.Exponent},{y},{yc}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

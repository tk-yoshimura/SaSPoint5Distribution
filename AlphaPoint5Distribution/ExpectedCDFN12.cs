using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class ExpectedCDFPadeN12 {
        static void Main() {
            using (StreamWriter sw = new("../../../../results/cdf_precision100.csv")) {
                sw.WriteLine("x,cdf(x)-1/2,ccdf(x)");

                for (double x = 0; x < 1; x += 1d / 1024) {
                    MultiPrecision<N12> y = CDFPadeN12.Value(x, complementary: false);
                    MultiPrecision<N12> yc = CDFPadeN12.Value(x, complementary: true);

                    Console.WriteLine($"{x}\n{y}\n{yc}");
                    sw.WriteLine($"{x},{y:e102},{yc:e102}");
                }

                for (double x0 = 1; x0 < 4096; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 512) {
                        MultiPrecision<N12> y = CDFPadeN12.Value(x, complementary: false);
                        MultiPrecision<N12> yc = CDFPadeN12.Value(x, complementary: true);

                        Console.WriteLine($"{x}\n{y}\n{yc}");
                        sw.WriteLine($"{x},{y:e102},{yc:e102}");
                    }
                }

                for (MultiPrecision<N12> x = MultiPrecision<N12>.Ldexp(1, 12); x.Exponent <= 1024; x *= 2) {
                    MultiPrecision<N12> y = CDFPadeN12.Value(x, complementary: false);
                    MultiPrecision<N12> yc = CDFPadeN12.Value(x, complementary: true);

                    Console.WriteLine($"{x}\n{y}\n{yc}");
                    sw.WriteLine($"2^{x.Exponent},{y:e102},{yc:e102}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

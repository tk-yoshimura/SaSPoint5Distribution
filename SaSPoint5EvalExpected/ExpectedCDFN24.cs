using SaSPoint5Expected;
using MultiPrecision;

namespace SaSPoint5kEvalExpected {
    internal class ExpectedCDFN24 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results/cdf_precision230.csv")) {
                sw.WriteLine("x,cdf(x)-1/2,ccdf(x)");

                for (double x = 0; x < 1; x += 1d / 1024) {
                    MultiPrecision<N24> y = CDFN24.Value(x, complementary: false);
                    MultiPrecision<N24> yc = CDFN24.Value(x, complementary: true);

                    Console.WriteLine($"{x}\n{y}\n{yc}");
                    sw.WriteLine($"{x},{y},{yc}");
                }

                for (double x0 = 1; x0 < 4096; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 512) {
                        MultiPrecision<N24> y = CDFN24.Value(x, complementary: false);
                        MultiPrecision<N24> yc = CDFN24.Value(x, complementary: true);

                        Console.WriteLine($"{x}\n{y}\n{yc}");
                        sw.WriteLine($"{x},{y},{yc}");
                    }
                }

                for (MultiPrecision<N24> x = MultiPrecision<N24>.Ldexp(1, 12); x.Exponent <= 1024; x *= 2) {
                    MultiPrecision<N24> y = CDFN24.Value(x, complementary: false);
                    MultiPrecision<N24> yc = CDFN24.Value(x, complementary: true);

                    Console.WriteLine($"{x}\n{y}\n{yc}");
                    sw.WriteLine($"2^{x.Exponent},{y},{yc}");
                }
            }

            using (BinaryWriter sw = new(File.Open("../../../../results_disused/cdf_precision230.bin", FileMode.Create))) {
                for (MultiPrecision<N24> x = 0, h = 1 / 32768d; x < 1; x += h) {
                    MultiPrecision<N24> y = CDFN24.Value(x, complementary: false);

                    Console.WriteLine($"{x}\n{y}\n{0.5 - y}");
                    sw.Write(x);
                    sw.Write(y);
                    sw.Write(0.5 - y);
                }

                for (MultiPrecision<N24> x0 = 1; x0.Exponent < 32; x0 *= 2) {
                    for (MultiPrecision<N24> x = x0, h = x0 / 16384; x < x0 * 2; x += h) {
                        MultiPrecision<N24> yc = CDFN24.Value(x, complementary: true);

                        Console.WriteLine($"{x}\n{0.5 - yc}\n{yc}");
                        sw.Write(x);
                        sw.Write(0.5 - yc);
                        sw.Write(yc);
                    }
                }

                for (int xexp = 32; xexp < 1024; xexp *= 2) {
                    for (MultiPrecision<N24> x0 = MultiPrecision<N24>.Ldexp(1, xexp); x0.Exponent < xexp * 2; x0 *= 2) {
                        for (MultiPrecision<N24> x = x0, h = x0 / (262144 / xexp); x < x0 * 2; x += h) {
                            MultiPrecision<N24> yc = CDFN24.Value(x, complementary: true);

                            Console.WriteLine($"{x}\n{0.5 - yc}\n{yc}");
                            sw.Write(x);
                            sw.Write(0.5 - yc);
                            sw.Write(yc);
                        }
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

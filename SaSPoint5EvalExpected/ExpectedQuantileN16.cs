using SaSPoint5PadeApprox;
using MultiPrecision;
using MultiPrecisionRootFinding;

namespace SaSPoint5EvalExpected {
    internal class ExpectedQuantileN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/quantile_precision150.csv")) {
                sw.WriteLine("complementary,p,quantile(p),cdf(quantile(p))");

                MultiPrecision<Pow2.N16> x = 0;

                for (MultiPrecision<Pow2.N16> p = 0; p <= 0.25; p += 1d / 65536) {
                    x = NewtonRaphsonFinder<Pow2.N16>.RootFind(
                        x => (CDFPadeN16.Value(x, complementary: false) - p, PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<Pow2.N16>()),
                        x0: x, accurate_bits: 508, overshoot_decay: true
                    );

                    MultiPrecision<Pow2.N16> y = CDFPadeN16.Value(x, complementary: false);

                    Console.WriteLine($"{p}\n{y}\n{x}\n");
                    sw.WriteLine($"0,{p},{x},{y}");
                }

                for (MultiPrecision<Pow2.N16> p = 0.25; p > 1d / 2048; p -= 1d / 65536) {
                    x = NewtonRaphsonFinder<Pow2.N16>.RootFind(
                        x => (CDFPadeN16.Value(x, complementary: true) - p, -PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<Pow2.N16>()),
                        x0: x, accurate_bits: 508, overshoot_decay: true
                    );

                    MultiPrecision<Pow2.N16> y = CDFPadeN16.Value(x, complementary: true);

                    Console.WriteLine($"{p}\n{y}\n{x}\n");
                    sw.WriteLine($"1,{p},{x},{y}");
                }

                for (MultiPrecision<Pow2.N16> p0 = 1d / 2048; p0.Exponent > -2048; p0 /= 2) {
                    for (MultiPrecision<Pow2.N16> p = p0; p > p0 / 2; p -= p0 / 64) {
                        x = NewtonRaphsonFinder<Pow2.N16>.RootFind(
                            x => (CDFPadeN16.Value(x, complementary: true) - p, -PDFPadeN16.Value(x.Convert<Pow2.N16>()).Convert<Pow2.N16>()),
                            x0: x, accurate_bits: 508, overshoot_decay: true
                        );

                        MultiPrecision<Pow2.N16> y = CDFPadeN16.Value(x, complementary: true);

                        Console.WriteLine(
                            $"{MultiPrecision<Pow2.N16>.Ldexp(p, -p.Exponent)}*2^{p.Exponent}\n" +
                            $"{MultiPrecision<Pow2.N16>.Ldexp(y, -y.Exponent)}*2^{y.Exponent}\n" +
                            $"{x}\n");
                        sw.WriteLine($"1," +
                            $"{MultiPrecision<Pow2.N16>.Ldexp(p, -p.Exponent)}*2^{p.Exponent}," +
                            $"{x}," +
                            $"{MultiPrecision<Pow2.N16>.Ldexp(y, -y.Exponent)}*2^{y.Exponent}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

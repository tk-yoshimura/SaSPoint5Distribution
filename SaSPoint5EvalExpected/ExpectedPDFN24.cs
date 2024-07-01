using SaSPoint5Expected;
using MultiPrecision;

namespace SaSPoint5kEvalExpected {
    internal class ExpectedPDFN24 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results/pdf_precision230.csv")) {
                sw.WriteLine("x,pdf(x)");

                for (double x = 0; x < 1; x += 1d / 1024) {
                    MultiPrecision<N24> y = PDFN24.Value(x);

                    Console.WriteLine($"{x}\n{y}");
                    sw.WriteLine($"{x},{y}");
                }

                for (double x0 = 1; x0 < 4096; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 512) {
                        MultiPrecision<N24> y = PDFN24.Value(x);

                        Console.WriteLine($"{x}\n{y}");
                        sw.WriteLine($"{x},{y}");
                    }
                }

                for (MultiPrecision<N24> x = MultiPrecision<N24>.Ldexp(1, 12); x.Exponent <= 1024; x *= 2) {
                    MultiPrecision<N24> y = PDFN24.Value(x);

                    Console.WriteLine($"{x}\n{y}");
                    sw.WriteLine($"2^{x.Exponent},{y}");
                }
            }

            using (BinaryWriter sw = new(File.Open("../../../../results_disused/pdf_precision230.bin", FileMode.Create))) {
                for (MultiPrecision<N24> x = 0, h = 1 / 32768d; x < 1; x += h) {
                    MultiPrecision<N24> y = PDFN24.Value(x);

                    Console.WriteLine($"{x}\n{y}");
                    sw.Write(x);
                    sw.Write(y);
                }

                for (MultiPrecision<N24> x0 = 1; x0.Exponent < 32; x0 *= 2) {
                    for (MultiPrecision<N24> x = x0, h = x0 / 16384; x < x0 * 2; x += h) {
                        MultiPrecision<N24> y = PDFN24.Value(x);

                        Console.WriteLine($"{x}\n{y}");
                        sw.Write(x);
                        sw.Write(y);
                    }
                }

                for (int xexp = 32; xexp < 1024; xexp *= 2) {
                    for (MultiPrecision<N24> x0 = MultiPrecision<N24>.Ldexp(1, xexp); x0.Exponent < xexp * 2; x0 *= 2) {
                        for (MultiPrecision<N24> x = x0, h = x0 / (262144 / xexp); x < x0 * 2; x += h) {
                            MultiPrecision<N24> y = PDFN24.Value(x);

                            Console.WriteLine($"{x}\n{y}");
                            sw.Write(x);
                            sw.Write(y);
                        }
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

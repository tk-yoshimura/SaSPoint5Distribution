using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class ExpectedN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results/pdf_precision150_2.csv")) {
                sw.WriteLine("x,pdf(x)");

                for (double x = 0; x < 1; x += 1d / 1024) {
                    MultiPrecision<Pow2.N16> y = PDFN16.Value(x) * MultiPrecision<Pow2.N16>.Pow(MultiPrecision<Pow2.N16>.Sqrt(x), 3);

                    Console.WriteLine($"{x}\n{y}");
                    sw.WriteLine($"{x},{y}");
                }

                for (double x0 = 1; x0 < 4096; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 512) {
                        MultiPrecision<Pow2.N16> y = PDFN16.Value(x) * MultiPrecision<Pow2.N16>.Pow(MultiPrecision<Pow2.N16>.Sqrt(x), 3);

                        Console.WriteLine($"{x}\n{y}");
                        sw.WriteLine($"{x},{y}");
                    }
                }

                for (MultiPrecision<Pow2.N16> x = MultiPrecision<Pow2.N16>.Ldexp(1, 12); x.Exponent <= 1024; x *= 2) {
                    MultiPrecision<Pow2.N16> y = PDFN16.Value(x) * MultiPrecision<Pow2.N16>.Pow(MultiPrecision<Pow2.N16>.Sqrt(x), 3);

                    Console.WriteLine($"{x}\n{y}");
                    sw.WriteLine($"2^{x.Exponent},{y}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class ExtremelySmallX {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/pdf_smallx.csv")) {
                sw.WriteLine("x,pdf(x)");

                for (double x = 1; x > 0; x /= 2) {
                    MultiPrecision<Pow2.N16> y = PDFN16.Value(x);
                    MultiPrecision<Pow2.N16> y_dec = PDFN16.Value(MultiPrecision<Pow2.N16>.BitDecrement(x));
                    MultiPrecision<Pow2.N16> err = MultiPrecision<Pow2.N16>.Abs(y - y_dec);

                    Console.WriteLine($"{x}\n{y}\n{y_dec}\n{err}");
                    sw.WriteLine($"{x},{y},{y_dec}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

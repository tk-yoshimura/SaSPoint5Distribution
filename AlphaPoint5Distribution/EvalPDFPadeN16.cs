using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class EvalPDFPadeN16 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/pdf_pade_eval.csv")) {
                sw.WriteLine("x,pdf(x),pdf_pade(x),err");

                for (double x = 0; x < 1; x += 1d / 4096) {
                    MultiPrecision<Pow2.N16> y = PDFN16.Value(x);
                    MultiPrecision<Pow2.N16> y_approx = PDFPadeN16.Value(x);
                    MultiPrecision<Pow2.N16> err = MultiPrecision<Pow2.N16>.Abs(y - y_approx) / y;

                    Console.WriteLine($"{x}\n{y}\n{y_approx}\n{err:e10}");
                    sw.WriteLine($"{x},{y},{y_approx},{err:e10}");
                }

                for (double x0 = 1; x0 < 4294967296; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 2048) {
                        MultiPrecision<Pow2.N16> y = PDFN16.Value(x);
                        MultiPrecision<Pow2.N16> y_approx = PDFPadeN16.Value(x);
                        MultiPrecision<Pow2.N16> err = MultiPrecision<Pow2.N16>.Abs(y - y_approx) / y;

                        Console.WriteLine($"{x}\n{y}\n{y_approx}\n{err:e10}");
                        sw.WriteLine($"{x},{y},{y_approx},{err:e10}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

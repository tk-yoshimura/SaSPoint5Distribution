using MultiPrecision;
using MultiPrecisionIntegrate;

namespace AlphaPoint5Distribution {
    internal class NumericIntegration {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/integrate_pdf_precision105_2.csv")) {
                sw.WriteLine("x0,x1,int_x0^x1 pdf(x) dx,err");

                //for (MultiPrecision<Pow2.N16> x = 0, h = 1d / 2097152; x < 1d / 512; x += h) {
                //    MultiPrecision<Pow2.N16> eps = (PDFPadeN16.Value(x) + PDFPadeN16.Value(x + h)) / 2 * h * "1e-105";

                //    (MultiPrecision<Pow2.N16> y, MultiPrecision<Pow2.N16> err) =
                //        GaussKronrodIntegral<Pow2.N16>.AdaptiveIntegrate(PDFPadeN16.Value, x, x + h, eps, depth: 16);

                //    Console.WriteLine($"[{x},{x + h}]\n{y}\n{err:e5}");
                //    sw.WriteLine($"{x},{x + h},{y},{err:e10}");
                //    sw.Flush();
                //}

                for (MultiPrecision<Pow2.N16> x0 = 256; x0 < 4294967296; x0 *= 2) {
                    for (MultiPrecision<Pow2.N16> x = x0, h = x0 / 4096; x < x0 * 2; x += h) {
                        MultiPrecision<Pow2.N16> eps = (PDFPadeN16.Value(x) + PDFPadeN16.Value(x + h)) / 2 * h * "1e-105";

                        (MultiPrecision<Pow2.N16> y, MultiPrecision<Pow2.N16> err) =
                            GaussKronrodIntegral<Pow2.N16>.AdaptiveIntegrate(PDFPadeN16.Value, x, x + h, eps, depth: 16);

                        Console.WriteLine($"[{x},{x + h}]\n{y}\n{err:e5}");
                        sw.WriteLine($"{x},{x + h},{y},{err:e10}");
                        sw.Flush();
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

using MultiPrecision;
using MultiPrecisionIntegrate;
using SaSPoint5PadeApprox;

namespace SaSPoint5EvalExpected {
    internal class EvalEntropy {
        static void Main_() {
            using StreamWriter sw = new("../../../../results/entropy_precision64.csv");

            static MultiPrecision<Pow2.N16> info(MultiPrecision<Pow2.N16> x) {
                MultiPrecision<Pow2.N16> px = PDFPadeN16.Value(x);

                if (px == 0) {
                    return 0;
                }

                return -px * MultiPrecision<Pow2.N16>.Log(px);
            };

            (MultiPrecision<Pow2.N16> value, MultiPrecision<Pow2.N16> error, _) =
                GaussKronrodIntegral<Pow2.N16>.AdaptiveIntegrate(info, 0, MultiPrecision<Pow2.N16>.PositiveInfinity,
                1e-60, GaussKronrodOrder.G64K129, discontinue_eval_points: 131072
            );

            value *= 2;
            error *= 2;

            Console.WriteLine($"{value}\n{error:e20}");

            sw.WriteLine($"entropy,error");
            sw.WriteLine($"{value},{error:e20}");

            sw.Close();

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

using MultiPrecision;
using MultiPresicionDifferentiate;

namespace AlphaPoint5Distribution {
    internal class Diff {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results/pdf_diff_nz.csv")) {
                for (int derivative = 0; derivative < 16; derivative++) {
                    MultiPrecision<Pow2.N16> diff =
                        ForwardIntwayDifferential<Pow2.N16>.Differentiate(
                            PDFN16.Value, 0, derivative, MultiPrecision<Pow2.N16>.Ldexp(1, -24)
                        ) * MultiPrecision<Pow2.N16>.TaylorSequence[derivative];

                    sw.WriteLine($"{derivative},{diff}");
                    Console.WriteLine($"{derivative}\n{diff}");
                }
            }

            using (StreamWriter sw = new("../../../../results/pdf_diff_limit.csv")) {
                for (int derivative = 0; derivative < 16; derivative++) {
                    MultiPrecision<Pow2.N16> diff =
                        ForwardIntwayDifferential<Pow2.N16>.Differentiate(
                            x => PDFN16.Value(1 / (x * x)), 0, derivative, MultiPrecision<Pow2.N16>.Ldexp(1, -24)
                        ) * MultiPrecision<Pow2.N16>.TaylorSequence[derivative];

                    sw.WriteLine($"{derivative},{diff}");
                    Console.WriteLine($"{derivative}\n{diff}");

                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

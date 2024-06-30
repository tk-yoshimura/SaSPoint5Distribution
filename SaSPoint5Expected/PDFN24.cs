using MultiPrecision;

namespace SaSPoint5Expected {
    public static class PDFN24 {
        public static MultiPrecision<N24> Value(MultiPrecision<N24> x) {
            if (x.Exponent <= -13) {
                return PDFNearZero<N24, Pow2.N32>.Value(x);
            }
            if (MultiPrecision<N24>.IsPositiveInfinity(x)) {
                return 0;
            }
            else {
                MultiPrecision<N24> y;

                if (x.Exponent <= -12) {
                    y = PDFLimit<N24, N96>.Value(x);
                }
                else if (x.Exponent <= -11) {
                    y = PDFLimit<N24, Pow2.N64>.Value(x);
                }
                else {
                    y = PDFLimit<N24, N48>.Value(x);
                }

                return y;
            }
        }
    }
}

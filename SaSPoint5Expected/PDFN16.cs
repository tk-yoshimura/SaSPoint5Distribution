using MultiPrecision;

namespace SaSPoint5Expected {
    public static class PDFN16 {
        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> x) {
            if (x.Exponent <= -12) {
                return PDFNearZero<Pow2.N16, N24>.Value(x);
            }
            if (MultiPrecision<Pow2.N16>.IsPositiveInfinity(x)) {
                return 0;
            }
            else {
                MultiPrecision<Pow2.N16> y;

                if (x.Exponent <= -11) {
                    y = PDFLimit<Pow2.N16, N48>.Value(x);
                }
                else if (x.Exponent <= -10) {
                    y = PDFLimit<Pow2.N16, Pow2.N32>.Value(x);
                }
                else {
                    y = PDFLimit<Pow2.N16, N24>.Value(x);
                }

                return y;
            }
        }
    }
}

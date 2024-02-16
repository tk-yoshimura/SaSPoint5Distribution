using MultiPrecision;

namespace AlphaPoint5Expected {
    public static class PDFN16 {
        public static readonly List<MultiPrecision<Pow2.N32>> nearzero_coefs = [];

        static PDFN16() {
            for (int k = 1; k <= 54; k++) {
                MultiPrecision<Pow2.N32> c = 2 * (4 * k - 2) * (4 * k - 1) * (4 * k + 1) / (2 * k - 1);

                nearzero_coefs.Add(c);
            }
        }

        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> x) {
            x = MultiPrecision<Pow2.N16>.Abs(x);

            if (x.Exponent < -12) {
                MultiPrecision<Pow2.N32> x2 = MultiPrecision<Pow2.N32>.Square(x.Convert<Pow2.N32>());

                MultiPrecision<Pow2.N32> s = 1 - x2 * nearzero_coefs[^1];

                for (int k = nearzero_coefs.Count - 2; k >= 0; k--) {
                    s = 1 - x2 * nearzero_coefs[k] * s;
                }

                MultiPrecision<Pow2.N16> y = 2 * MultiPrecision<Pow2.N16>.RcpPI * s.Convert<Pow2.N16>();

                return y;
            }
            if (MultiPrecision<Pow2.N16>.IsPositiveInfinity(x)) {
                return 0;
            }
            else {
                MultiPrecision<Pow2.N16> y;

                if (x.Exponent >= -2) {
                    y = PDFLimit<Pow2.N16, N20>.Value(x);
                }
                else if (x.Exponent >= -10) {
                    y = PDFLimit<Pow2.N16, Pow2.N32>.Value(x);
                }
                else if (x.Exponent >= -11) {
                    y = PDFLimit<Pow2.N16, Pow2.N64>.Value(x);
                }
                else {
                    y = PDFLimit<Pow2.N16, Pow2.N128>.Value(x);
                }

                return y;
            }
        }
    }
}

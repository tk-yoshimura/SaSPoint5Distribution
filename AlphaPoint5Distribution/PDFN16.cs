using MultiPrecision;

namespace AlphaPoint5Distribution {
    public static class PDFN16 {
        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> x) {
            x = MultiPrecision<Pow2.N16>.Abs(x);

            if (x == 0) {
                return 2 * MultiPrecision<Pow2.N16>.RcpPI;
            }

            if (x.Exponent < -24) {
                throw new ArgumentException("extremely small x", nameof(x));
            }

            if (MultiPrecision<Pow2.N16>.IsPositiveInfinity(x)) {
                return 0;
            }

            (MultiPrecision<Pow2.N16> fc, MultiPrecision<Pow2.N16> fs)
                = FresnelN16.Value(1 / MultiPrecision<Pow2.N16>.Sqrt(2 * MultiPrecision<Pow2.N16>.PI * x));
            (MultiPrecision<Pow2.N16> c, MultiPrecision<Pow2.N16> s)
                = (MultiPrecision<Pow2.N16>.Cos(1 / (4 * x)), MultiPrecision<Pow2.N16>.Sin(1 / (4 * x)));

            MultiPrecision<Pow2.N16> r = 1 / MultiPrecision<Pow2.N16>.Sqrt(2 * MultiPrecision<Pow2.N16>.PI * x * x * x);
            MultiPrecision<Pow2.N16> n = s * fs + c * fc;

            MultiPrecision<Pow2.N16> y = -r * n;

            return y;
        }
    }
}

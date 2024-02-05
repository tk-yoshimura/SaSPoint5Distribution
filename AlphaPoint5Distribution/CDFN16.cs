using MultiPrecision;

namespace AlphaPoint5Distribution {
    public static class CDFN16 {
        public static readonly List<MultiPrecision<Pow2.N32>> nearzero_coefs = [];

        static CDFN16() {
            MultiPrecision<Pow2.N32> prev_c = 1, c = 1;
            for (int k = 1; k <= 54; k++) {
                c = c * (4 * k - 2) * (4 * k - 1) * (4 * k) * (4 * k + 1) / (2 * k) / (2 * k + 1);

                MultiPrecision<Pow2.N32> r = c / prev_c;
                prev_c = c;
                
                nearzero_coefs.Add(r);
            }
        }

        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> x, bool complementary = false) {
            x = MultiPrecision<Pow2.N16>.Abs(x);

            if (MultiPrecision<Pow2.N16>.IsZero(x)) {
                return complementary ? 0.5 : 0;
            }

            if (x.Exponent < -12) {
                MultiPrecision<Pow2.N32> x2 = MultiPrecision<Pow2.N32>.Square(x.Convert<Pow2.N32>());

                MultiPrecision<Pow2.N32> s = 1 - x2 * nearzero_coefs[^1];

                for (int k = nearzero_coefs.Count - 2; k >= 0; k--) {
                    s = 1 - x2 * nearzero_coefs[k] * s;
                }

                MultiPrecision<Pow2.N16> y = 2 * MultiPrecision<Pow2.N16>.RcpPI * x * s.Convert<Pow2.N16>();

                return complementary ? 0.5 - y : y;
            }
            if (MultiPrecision<Pow2.N16>.IsPositiveInfinity(x)) {
                return complementary ? 0 : 0.5;
            }
            else {
                MultiPrecision<N20> y;

                if (x.Exponent >= -2) {
                    y = CDFLimit<N20, N20>.Value(x.Convert<N20>());
                }
                else if (x.Exponent >= -10) {
                    y = CDFLimit<N20, Pow2.N32>.Value(x.Convert<N20>());
                }
                else if (x.Exponent >= -11) {
                    y = CDFLimit<N20, Pow2.N64>.Value(x.Convert<N20>());
                }
                else if (x.Exponent >= -12) {
                    y = CDFLimit<N20, Pow2.N128>.Value(x.Convert<N20>());
                }
                else {
                    return MultiPrecision<Pow2.N16>.NaN;
                }

                return complementary ? y.Convert<Pow2.N16>() : (0.5 - y).Convert<Pow2.N16>();
            }
        }
    }
}

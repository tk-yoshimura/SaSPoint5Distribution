using MultiPrecision;

namespace AlphaPoint5Distribution {
    public static class PDFN16 {
        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> x) {
            x = MultiPrecision<Pow2.N16>.Abs(x);

            if (x.Exponent < -21) {
                MultiPrecision<Pow2.N32> x2 = MultiPrecision<Pow2.N32>.Square(x.Convert<Pow2.N32>());

                return 2 * MultiPrecision<Pow2.N16>.RcpPI * (
                    1 - x2 * 60 * (1 - x2 * 252 * (1 - x2 * 572 * (1 - x2 * 1020 * (1 - x2 * 1596 * (
                    1 - x2 * 2300 * (1 - x2 * 3132 * (1 - x2 * 4092 * (1 - x2 * 5180 * (1 - x2 * 6396 * (
                    1 - x2 * 7740 * (1 - x2 * 9212 * (1 - x2 * 10812 * (1 - x2 * 12540 * (1 - x2 * 14396))))))))))))))
                ).Convert<Pow2.N16>();
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

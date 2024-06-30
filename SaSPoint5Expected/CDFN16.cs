using MultiPrecision;

namespace SaSPoint5Expected {
    public static class CDFN16 {
        public static MultiPrecision<Pow2.N16> Value(MultiPrecision<Pow2.N16> x, bool complementary = false) {
            x = MultiPrecision<Pow2.N16>.Abs(x);

            if (MultiPrecision<Pow2.N16>.IsZero(x)) {
                return complementary ? 0.5 : 0;
            }

            if (x.Exponent <= -12) {
                MultiPrecision<Pow2.N16> y = CDFNearZero<Pow2.N16, N24>.Value(x);

                return complementary ? 0.5 - y : y;
            }
            if (MultiPrecision<Pow2.N16>.IsPositiveInfinity(x)) {
                return complementary ? 0 : 0.5;
            }
            else {
                MultiPrecision<Plus1<Pow2.N16>> y;

                if (x.Exponent <= -11) {
                    y = CDFLimit<Plus1<Pow2.N16>, N48>.Value(x.Convert<Plus1<Pow2.N16>>());
                }
                else if (x.Exponent <= -10) {
                    y = CDFLimit<Plus1<Pow2.N16>, Pow2.N32>.Value(x.Convert<Plus1<Pow2.N16>>());
                }
                else{
                    y = CDFLimit<Plus1<Pow2.N16>, N24>.Value(x.Convert<Plus1<Pow2.N16>>());
                }

                return complementary ? y.Convert<Pow2.N16>() : (0.5 - y).Convert<Pow2.N16>();
            }
        }
    }
}

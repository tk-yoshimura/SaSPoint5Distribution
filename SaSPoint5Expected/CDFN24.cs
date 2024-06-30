using MultiPrecision;

namespace SaSPoint5Expected {
    public static class CDFN24 {
        public static MultiPrecision<N24> Value(MultiPrecision<N24> x, bool complementary = false) {
            x = MultiPrecision<N24>.Abs(x);

            if (MultiPrecision<N24>.IsZero(x)) {
                return complementary ? 0.5 : 0;
            }

            if (x.Exponent <= -13) {
                MultiPrecision<N24> y = CDFNearZero<N24, Pow2.N32>.Value(x);

                return complementary ? 0.5 - y : y;
            }
            if (MultiPrecision<N24>.IsPositiveInfinity(x)) {
                return complementary ? 0 : 0.5;
            }
            else {
                MultiPrecision<Plus1<N24>> y;

                if (x.Exponent <= -12) {
                    y = CDFLimit<Plus1<N24>, N96>.Value(x.Convert<Plus1<N24>>());
                }
                else if (x.Exponent <= -11) {
                    y = CDFLimit<Plus1<N24>, Pow2.N64>.Value(x.Convert<Plus1<N24>>());
                }
                else{
                    y = CDFLimit<Plus1<N24>, N48>.Value(x.Convert<Plus1<N24>>());
                }

                return complementary ? y.Convert<N24>() : (0.5 - y).Convert<N24>();
            }
        }
    }
}

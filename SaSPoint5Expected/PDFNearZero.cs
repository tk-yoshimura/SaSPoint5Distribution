using MultiPrecision;

namespace SaSPoint5Expected {
    public static class PDFNearZero<N, M> where N : struct, IConstant where M : struct, IConstant {
        public static readonly List<MultiPrecision<M>> coef_table = [1];

        public static MultiPrecision<N> Value(MultiPrecision<N> x, int max_terms = 8192) {
            x = MultiPrecision<N>.Abs(x);

            MultiPrecision<M> x2 = MultiPrecision<M>.Square(x.Convert<M>()), x4 = x2 * x2;

            MultiPrecision<M> s = 0, u = 1;

            for (int k = 0, conv_times = 0; k <= max_terms; k += 2) {
                MultiPrecision<M> c0 = CoefTable(k), c1 = CoefTable(k + 1);

                MultiPrecision<M> ds = u * (c0 + x2 * c1);

                if (s.Exponent - ds.Exponent > MultiPrecision<N>.Bits) {
                    conv_times++;

                    if (conv_times >= 4) {
                        return 2 * MultiPrecision<N>.RcpPI * s.Convert<N>();
                    }
                }
                else {
                    conv_times = 0;
                }

                if (s.Exponent > MultiPrecision<M>.Bits - MultiPrecision<N>.Bits) {
                    break;
                }

                s += ds;
                u *= x4;
            }

            return MultiPrecision<N>.NaN;
        }

        public static MultiPrecision<M> CoefTable(int n) {
            for (int k = coef_table.Count; k <= n; k++) { 
                MultiPrecision<M> c = -coef_table[^1] * 4 * (4 * k - 1) * (4 * k + 1);

                coef_table.Add(c);
            }

            return coef_table[n];
        }
    }
}

using MultiPrecision;

namespace SaSPoint5Expected {
    public class CDFLimit<N, M> where N : struct, IConstant where M : struct, IConstant {
        private static readonly List<MultiPrecision<M>> coef_table = [];

        public static MultiPrecision<N> Value(MultiPrecision<N> x, int max_terms = 8192) {
            MultiPrecision<M> w = 1 / x.Convert<M>(), v = MultiPrecision<M>.Sqrt(w);

            MultiPrecision<M> s = 0, u = v;

            for (int k = 0, conv_times = 0; k <= max_terms; k += 2) {
                MultiPrecision<M> c0 = CoefTable(k), c1 = CoefTable(k + 1);

                MultiPrecision<M> ds = u * (c0 + v * c1);

                if (s.Exponent - ds.Exponent > MultiPrecision<N>.Bits) {
                    conv_times++;

                    if (conv_times >= 4) {
                        return s.Convert<N>();
                    }
                }
                else {
                    conv_times = 0;
                }

                if (s.Exponent > MultiPrecision<M>.Bits - MultiPrecision<N>.Bits) {
                    break;
                }

                s += ds;
                u *= w;
            }

            return MultiPrecision<N>.NaN;
        }

        public static MultiPrecision<M> CoefTable(int n) {
            for (int k = coef_table.Count; k <= n; k++) {
                MultiPrecision<M> c = PDFLimit<N, M>.CoefTable(k) / (k + 1) * 2;

                coef_table.Add(c);
            }

            return coef_table[n];
        }
    }
}

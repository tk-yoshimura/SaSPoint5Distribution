using MultiPrecision;

namespace SaSPoint5Expected {
    public class PDFLimit<N, M> where N : struct, IConstant where M : struct, IConstant {
        private static readonly MultiPrecision<M> norm = 1 / MultiPrecision<M>.Sqrt(2 * MultiPrecision<M>.PI);
        private static readonly List<MultiPrecision<M>> prod_table = [1], frac_table = [2], coef_table = [];

        public static MultiPrecision<N> Value(MultiPrecision<N> x, int max_terms = 8192) {
            MultiPrecision<M> w = 1 / x.Convert<M>(), v = MultiPrecision<M>.Sqrt(w);

            MultiPrecision<M> s = 0, u = v * v * v;

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
                MultiPrecision<M> c = 0;

                if (k % 4 == 0 || k % 4 == 2) {
                    int m = k / 2;

                    c = ((m / 2) % 2 == 0 ? 1 : -1) * norm / (MultiPrecision<M>.Ldexp(2, m) * ProdTable(m));
                }
                else if (k % 4 == 1) {
                    int m = k / 4;

                    c = (m % 2 == 0 ? -1 : 1) * MultiPrecision<M>.RcpPI / FracTable(m);
                }

                coef_table.Add(c);
            }

            return coef_table[n];
        }

        private static MultiPrecision<M> ProdTable(int n) {
            for (int k = prod_table.Count; k <= n; k++) {
                MultiPrecision<M> c = 2 * k * prod_table[^1];

                prod_table.Add(c);
            }

            return prod_table[n];
        }

        private static MultiPrecision<M> FracTable(int n) {
            for (int k = frac_table.Count; k <= n; k++) {
                MultiPrecision<M> c = frac_table[^1] * (4 * k - 1) * (4 * k) * (4 * k + 1) * (4 * k + 2) / (2 * k) / (2 * k + 1);

                frac_table.Add(c);
            }

            return frac_table[n];
        }
    }
}

using MultiPrecision;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AlphaPoint5Distribution {
    internal static class ApproxUtil {

        public static MultiPrecision<Pow2.N16> Pade(MultiPrecision<Pow2.N16> x, ReadOnlyCollection<(MultiPrecision<Pow2.N16> c, MultiPrecision<Pow2.N16> d)> table) {
            (MultiPrecision<Pow2.N16> sc, MultiPrecision<Pow2.N16> sd) = table[^1];

#if DEBUG
            Trace.Assert(x >= 0, $"must be positive! {x}");
#endif

            for (int i = table.Count - 2; i >= 0; i--) {
                sc = sc * x + table[i].c;
                sd = sd * x + table[i].d;
            }

#if DEBUG
            Trace.Assert(sd >= 0.5, $"pade denom digits loss! {x}");
#endif

            return sc / sd;
        }

        public static MultiPrecision<Pow2.N16> Poly(MultiPrecision<Pow2.N16> x, ReadOnlyCollection<MultiPrecision<Pow2.N16>> table) {
            MultiPrecision<Pow2.N16> s = table[^1];

            for (int i = table.Count - 2; i >= 0; i--) {
                s = s * x + table[i];
            }

            return s;
        }
    }
}
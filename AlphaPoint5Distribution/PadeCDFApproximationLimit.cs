using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace AlphaPoint5Distribution {
    internal class PadeCDFApproximationLimit {
        static void Main_() {
            List<(double xmin, double xmax)> ranges = [
                (0, 1d / 4)
            ];

            using (StreamWriter sw = new("../../../../results_disused/pade_limitcdf_precision105.csv")) {
                foreach ((double xmin, double xmax) in ranges) {
                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range = [];

                    for (MultiPrecision<Pow2.N16> x = xmin, h = (xmax - xmin) / 4096; x <= xmax; x += h) {
                        if (x != 0) {
                            MultiPrecision<Pow2.N32> y = CDFLimit<Pow2.N32, Pow2.N64>.Value(1 / (x * x).Convert<Pow2.N32>()) / x.Convert<Pow2.N32>();

                            expecteds_range.Add((x.Convert<Pow2.N64>(), y.Convert<Pow2.N64>()));
                        }
                        else {
                            expecteds_range.Add((x.Convert<Pow2.N64>(), 1 / MultiPrecision<Pow2.N64>.Sqrt(2 * MultiPrecision<Pow2.N64>.PI)));
                        }
                    }

                    Console.WriteLine("expecteds computed");

                    MultiPrecision<Pow2.N64> y0 = expecteds_range.Where(item => item.x == xmin).First().y;
                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.x - xmin).ToArray(), ys = expecteds_range.Select(item => item.y).ToArray();

                    bool finished = false;
                    for (int n = 16; n <= 64 && !finished; n++) {
                        for (int m = n - 2; m <= n + 2; m++) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n, intercept: y0);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();
                            Vector<Pow2.N64> errs = pade.Error(param);

                            MultiPrecision<Pow2.N64> max_rateerr = 0;
                            for (int i = 0; i < errs.Dim; i++) {
                                if (ys[i] == 0) {
                                    continue;
                                }

                                max_rateerr = MultiPrecision<Pow2.N64>.Max(MultiPrecision<Pow2.N64>.Abs(errs[i] / ys[i]), max_rateerr);
                            }

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            Console.WriteLine($"mcount: {param.Count(item => item.val.Sign != Sign.Plus)}");

                            if (max_rateerr > "1e-80") {
                                break;
                            }

                            if (max_rateerr < "1e-102" && param.Count(item => item.val.Sign != Sign.Plus) <= 0) {
                                sw.WriteLine($"x=[{xmin},{xmax}]");
                                sw.WriteLine($"m={m},n={n}");

                                sw.WriteLine("numer");
                                foreach (var (_, val) in param[..m]) {
                                    sw.WriteLine($"{val:e105}");
                                }
                                sw.WriteLine("denom");
                                foreach (var (_, val) in param[m..]) {
                                    sw.WriteLine($"{val:e105}");
                                }

                                sw.WriteLine("coef");

                                if (m < n) {
                                    for (int i = 0; i < m; i++) {
                                        sw.WriteLine($"(\"{param[..m][i]:e105}\", \"{param[m..][i]:e105}\"),");
                                    }
                                    for (int i = 0; i < n - m; i++) {
                                        sw.WriteLine($"(0, \"{param[m..][m + i]:e105}\"),");
                                    }
                                }
                                else {
                                    for (int i = 0; i < n; i++) {
                                        sw.WriteLine($"(\"{param[..m][i]:e105}\", \"{param[m..][i]:e105}\"),");
                                    }
                                    for (int i = 0; i < m - n; i++) {
                                        sw.WriteLine($"(\"{param[..m][n + i]:e105}\", 0),");
                                    }
                                }

                                sw.WriteLine("relative err");
                                sw.WriteLine($"{max_rateerr:e20}");
                                sw.Flush();

                                finished = true;
                                break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

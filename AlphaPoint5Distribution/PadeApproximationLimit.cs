using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace AlphaPoint5Distribution {
    internal class PadeApproximationLimit {
        static void Main_() {
            List<(double xmin, double xmax)> ranges = [
                (0, 1d / 4)
            ];

            using (StreamWriter sw = new("../../../../results_disused/pade_limitpdf_precision135.csv")) {
                foreach ((double xmin, double xmax) in ranges) {
                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range = [];

                    for (MultiPrecision<Pow2.N16> x = xmin, h = (xmax - xmin) / 4096; x <= xmax; x += h) {
                        if (x != 0) {
                            MultiPrecision<Pow2.N64> y = PDFN16.Value(1 / (x * x)).Convert<Pow2.N64>() / MultiPrecision<Pow2.N64>.Pow(x.Convert<Pow2.N64>(), 3);

                            expecteds_range.Add((x.Convert<Pow2.N64>(), y));
                        }
                        else { 
                            expecteds_range.Add((x.Convert<Pow2.N64>(), 1 / MultiPrecision<Pow2.N64>.Sqrt(8 * MultiPrecision<Pow2.N64>.PI)));
                        }
                    }

                    Console.WriteLine("expecteds computed");

                    MultiPrecision<Pow2.N64> y0 = expecteds_range.Where(item => item.x == xmin).First().y;
                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.x - xmin).ToArray(), ys = expecteds_range.Select(item => item.y).ToArray();

                    for (int m = 16; m <= 64; m++) {
                        PadeFitter<Pow2.N64> pade = new(xs, ys, m - 2, m, intercept: y0);

                        Vector<Pow2.N64> param = pade.ExecuteFitting();
                        Vector<Pow2.N64> errs = pade.Error(param);

                        MultiPrecision<Pow2.N64> max_rateerr = 0;
                        for (int i = 0; i < errs.Dim; i++) {
                            if (ys[i] == 0) {
                                continue;
                            }

                            max_rateerr = MultiPrecision<Pow2.N64>.Max(MultiPrecision<Pow2.N64>.Abs(errs[i] / ys[i]), max_rateerr);
                        }

                        Console.WriteLine($"m={m - 2},n={m}");
                        Console.WriteLine($"{max_rateerr:e20}");

                        if (max_rateerr < "1e-135" && param.All(item => item.val.Sign == Sign.Plus)) {
                            sw.WriteLine($"x=[{xmin},{xmax}]");
                            sw.WriteLine($"m={m - 2},n={m}");
                            sw.WriteLine("numer");
                            foreach (var (_, val) in param[..(m - 2)]) {
                                sw.WriteLine($"{val:e150}");
                            }
                            sw.WriteLine("denom");
                            foreach (var (_, val) in param[(m - 2)..]) {
                                sw.WriteLine($"{val:e150}");
                            }

                            sw.WriteLine("relative err");
                            sw.WriteLine($"{max_rateerr:e20}");
                            sw.Flush();

                            break;
                        }
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

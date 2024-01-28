using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace AlphaPoint5Distribution {
    internal class PadeApproximation {
        static void Main_() {
            List<(double xmin, double xmax)> ranges = [
                (0, 1d / 512)
            ];

            for (double xmin = 1d / 512; xmin < 16; xmin *= 2) {
                ranges.Add((xmin, xmin * 2));
            }

            using (StreamWriter sw = new("../../../../results_disused/pade_pdf_precision135_2.csv")) {
                foreach ((double xmin, double xmax) in ranges) {
                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range = [];

                    for (double x = xmin, h = (xmax - xmin) / 4096; x <= xmax; x += h) {
                        MultiPrecision<Pow2.N16> y = PDFN16.Value(x);

                        expecteds_range.Add((x, y.Convert<Pow2.N64>()));
                    }

                    Console.WriteLine("expecteds computed");

                    MultiPrecision<Pow2.N64> y0 = expecteds_range.Where(item => item.x == xmin).First().y;
                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.x - xmin).ToArray(), ys = expecteds_range.Select(item => item.y).ToArray();

                    for (int m = 40; m <= 64; m++) {
                        PadeFitter<Pow2.N64> pade = new(xs, ys, m - 1, m, intercept: y0);

                        Vector<Pow2.N64> param = pade.ExecuteFitting();
                        Vector<Pow2.N64> errs = pade.Error(param);

                        MultiPrecision<Pow2.N64> max_rateerr = 0;
                        for (int i = 0; i < errs.Dim; i++) {
                            if (ys[i] == 0) {
                                continue;
                            }

                            max_rateerr = MultiPrecision<Pow2.N64>.Max(MultiPrecision<Pow2.N64>.Abs(errs[i] / ys[i]), max_rateerr);
                        }

                        Console.WriteLine($"m={m - 1},n={m}");
                        Console.WriteLine($"{max_rateerr:e20}");

                        if (max_rateerr < "1e-135" && param.All(item => item.val.Sign == Sign.Plus)) {
                            sw.WriteLine($"x=[{xmin},{xmax}]");
                            sw.WriteLine($"m={m - 1},n={m}");
                            sw.WriteLine("numer");
                            foreach (var (_, val) in param[..(m - 1)]) {
                                sw.WriteLine($"{val:e150}");
                            }
                            sw.WriteLine("denom");
                            foreach (var (_, val) in param[(m - 1)..]) {
                                sw.WriteLine($"{val:e150}");
                            }

                            sw.WriteLine("coef");
                            for (int i = 0; i < m - 1; i++) {
                                sw.WriteLine($"(\"{param[..(m - 1)][i]:e150}\", \"{param[(m - 1)..][i]:e150}\"),");
                            }
                            for (int i = 0; i < 1; i++) {
                                sw.WriteLine($"(0, \"{param[(m - 1)..][m - 1 + i]:e150}\"),");
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

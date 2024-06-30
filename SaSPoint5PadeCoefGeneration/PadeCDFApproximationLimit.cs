using SaSPoint5Expected;
using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace SaSPoint5PadeCoefGeneration {
    internal class CDFPadeApproximationLimit {
        static void Main_() {
            List<(MultiPrecision<Pow2.N64> xmin, MultiPrecision<Pow2.N64> xmax, MultiPrecision<Pow2.N64> limit_range)> ranges = [
                (0, 1 / 16d, 1 / 16d),
                (0, 1 / 8d, 1 / 8d),
                (0, 1 / 4d, 1 / 4d)
            ];

            using (StreamWriter sw = new("../../../../results_disused/pade_limitcdf_precision150.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> xmin, MultiPrecision<Pow2.N64> xmax) {
                    Console.WriteLine($"[{xmin}, {xmax}]");

                    List<(MultiPrecision<Pow2.N64> x, MultiPrecision<Pow2.N64> y)> expecteds_range = [];

                    for (MultiPrecision<Pow2.N64> x = xmin, h = (xmax - xmin) / 4096; x <= xmax; x += h) {

                        if (x != 0) {
                            MultiPrecision<Pow2.N64> y = CDFN16.Value(1 / MultiPrecision<Pow2.N16>.Square(x.Convert<Pow2.N16>()), complementary: true).Convert<Pow2.N64>()
                                / x;

                            expecteds_range.Add((x.Convert<Pow2.N64>(), y.Convert<Pow2.N64>()));
                        }
                        else {
                            expecteds_range.Add((x.Convert<Pow2.N64>(), 1 / MultiPrecision<Pow2.N64>.Sqrt(2 * MultiPrecision<Pow2.N64>.PI)));
                        }
                    }

                    Console.WriteLine("expecteds computed");

                    MultiPrecision<Pow2.N64> x0 = expecteds_range.First().x;
                    MultiPrecision<Pow2.N64> y0 = expecteds_range.First().y;
                    MultiPrecision<Pow2.N64> xrange = expecteds_range.Last().x - x0;

                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.x - x0).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    for (int coefs = 5; coefs <= 128; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n, intercept: y0);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();
                            Vector<Pow2.N64> errs = pade.Error(param);

                            MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            if (coefs > 32 && max_rateerr > "1e-50") {
                                return false;
                            }

                            if (max_rateerr > "1e-145") {
                                break;
                            }

                            if (max_rateerr < "1e-150" &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, xrange) &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, xrange)) {
                                sw.WriteLine($"x=[{xmin},{xmax}]");
                                sw.WriteLine($"m={m},n={n}");
                                sw.WriteLine("numer");
                                foreach (var (_, val) in param[..m]) {
                                    sw.WriteLine($"{val:e155}");
                                }
                                sw.WriteLine("denom");
                                foreach (var (_, val) in param[m..]) {
                                    sw.WriteLine($"{val:e155}");
                                }

                                sw.WriteLine("coef");
                                foreach ((var numer, var denom) in CurveFittingUtils.EnumeratePadeCoef(param, m, n)) {
                                    sw.WriteLine($"(\"{numer:e155}\", \"{denom:e155}\"),");
                                }

                                sw.WriteLine("relative err");
                                sw.WriteLine($"{max_rateerr:e20}");
                                sw.Flush();

                                return true;
                            }
                        }
                    }

                    return false;
                }

                Segmenter<Pow2.N64> segmenter = new(ranges, approximate);
                segmenter.Execute();

                foreach ((var xmin, var xmax, bool is_successs) in segmenter.ApproximatedRanges) {
                    sw.WriteLine($"[{xmin},{xmax}],{(is_successs ? "OK" : "NG")}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

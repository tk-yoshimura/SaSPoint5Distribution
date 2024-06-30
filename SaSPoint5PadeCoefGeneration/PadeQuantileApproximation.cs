using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace SaSPoint5PadeCoefGeneration {
    internal class PadeQuantileApproximation {
        static void Main() {
            List<(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax, MultiPrecision<Pow2.N64> limit_range)> ranges = [];

            for (MultiPrecision<Pow2.N64> pmin = 1; pmin < 4; pmin *= 2) {
                ranges.Add((pmin, pmin * 2, pmin / 512));
            }
            for (MultiPrecision<Pow2.N64> pmin = 4; pmin < 1024; pmin *= 2) {
                ranges.Add((pmin, pmin * 2, pmin / 16));
            }

            List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds = [];

            using (StreamReader sr = new("../../../../results_disused/quantile_precision150_loglog.csv")) {
                sr.ReadLine();

                while (!sr.EndOfStream) {
                    string? line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line)) {
                        break;
                    }

                    string[] line_split = line.Split(",");

                    MultiPrecision<Pow2.N64> p = line_split[0];
                    MultiPrecision<Pow2.N64> x = line_split[1];

                    if (p > ranges[^1].pmax) {
                        break;
                    }

                    expecteds.Add((p, x));
                }
            }

            using (StreamWriter sw = new("../../../../results_disused/pade_quantile_precision145_2.csv")) {
                bool approximate(MultiPrecision<Pow2.N64> pmin, MultiPrecision<Pow2.N64> pmax) {
                    Console.WriteLine($"[{pmin}, {pmax}]");

                    List<(MultiPrecision<Pow2.N64> p, MultiPrecision<Pow2.N64> y)> expecteds_range
                        = expecteds.Where(item => item.p >= pmin && item.p <= pmax).ToList();

                    Console.WriteLine($"expecteds {expecteds_range.Count} samples");

                    if (expecteds_range.Count < 128) {
                        return false;
                    }

                    Vector<Pow2.N64> xs = expecteds_range.Select(item => item.p - pmin).ToArray();
                    Vector<Pow2.N64> ys = expecteds_range.Select(item => item.y).ToArray();

                    MultiPrecision<Pow2.N64> y0 = ys[0];

                    for (int coefs = 5; coefs <= 128; coefs++) {
                        foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coefs, 2)) {
                            PadeFitter<Pow2.N64> pade = new(xs, ys, m, n, intercept: y0);

                            Vector<Pow2.N64> param = pade.ExecuteFitting();

                            MultiPrecision<Pow2.N64> max_rateerr = CurveFittingUtils.MaxRelativeError(ys, pade.FittingValue(xs, param));

                            Console.WriteLine($"m={m},n={n}");
                            Console.WriteLine($"{max_rateerr:e20}");

                            Console.WriteLine($"mcount: {param.Count(item => item.val.Sign != Sign.Plus)}");

                            if (coefs > 8 && max_rateerr > "1e-8") {
                                return false;
                            }

                            if (coefs > 32 && max_rateerr > "1e-45") {
                                return false;
                            }

                            if (max_rateerr > "1e-140") {
                                break;
                            }

                            if (max_rateerr < "1e-145" &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[..m], 0, pmax - pmin) &&
                                !CurveFittingUtils.HasLossDigitsPolynomialCoef(param[m..], 0, pmax - pmin)) {

                                sw.WriteLine($"p=[{pmin},{pmax}]");
                                sw.WriteLine($"m={m},n={n}");

                                sw.WriteLine("numer");
                                foreach (var (_, val) in param[..m]) {
                                    sw.WriteLine($"{val:e150}");
                                }
                                sw.WriteLine("denom");
                                foreach (var (_, val) in param[m..]) {
                                    sw.WriteLine($"{val:e150}");
                                }

                                sw.WriteLine("coef");
                                foreach ((var numer, var denom) in CurveFittingUtils.EnumeratePadeCoef(param, m, n)) {
                                    sw.WriteLine($"(\"{numer:e150}\", \"{denom:e150}\"),");
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

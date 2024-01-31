using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class EvalCDFPadeN12 {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/cdf_pade_eval.csv")) {
                sw.WriteLine("x,cdferror,ccdferror");

                using (StreamReader sr = new("../../../../results_disused/cdf_precision105_all.csv")) {
                    sr.ReadLine();

                    while (!sr.EndOfStream) {
                        string? line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line)) {
                            break;
                        }

                        string[] line_split = line.Split(",");

                        MultiPrecision<N12> x = line_split[0];
                        MultiPrecision<N12> cdf_expected = line_split[1];
                        MultiPrecision<N12> ccdf_expected = line_split[2];

                        MultiPrecision<N12> cdf_actual = CDFPadeN12.Value(x);
                        MultiPrecision<N12> ccdf_actual = CDFPadeN12.Value(x, complementary: true);

                        MultiPrecision<N12> cdf_error = MultiPrecision<N12>.Abs(cdf_expected - cdf_actual) / cdf_expected;
                        MultiPrecision<N12> ccdf_error = MultiPrecision<N12>.Abs(ccdf_expected - ccdf_actual) / ccdf_expected;

                        Console.WriteLine($"{x}\n{cdf_error:e10}\n{ccdf_error:e10}");
                        sw.WriteLine($"{x},{cdf_error:e20},{ccdf_error:e20}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

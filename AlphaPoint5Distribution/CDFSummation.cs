using MultiPrecision;
using System;

namespace AlphaPoint5Distribution {
    internal class CDFSummation {
        static void Main_() {
            using (StreamWriter sw = new("../../../../results_disused/cdf_precision105_all.csv")) {
                sw.WriteLine("x,cdf(x)-1/2,ccdf(x)");
                sw.WriteLine("0,0,0.5");

                using (StreamReader sr = new("../../../../results_disused/integrate_pdf_precision105.csv")) {
                    sr.ReadLine();

                    MultiPrecision<Pow2.N16> cdf = 0;

                    while (!sr.EndOfStream) {
                        string? line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line)) {
                            break;
                        }

                        string[] line_split = line.Split(",");

                        MultiPrecision<Pow2.N16> x0 = line_split[0];
                        MultiPrecision<Pow2.N16> x1 = line_split[1];
                        MultiPrecision<Pow2.N16> integration = line_split[2];

                        cdf += integration;
                        MultiPrecision<Pow2.N16> ccdf = 0.5 - cdf;

                        Console.WriteLine($"{x1}\n{cdf:e108}\n{ccdf:e108}");
                        sw.WriteLine($"{x1},{cdf:e108},{ccdf:e108}");
                        
                        if (x1 >= 4) {
                            break;
                        }
                    }
                }

                for (double x0 = 4; x0 < 4294967296; x0 *= 2) {
                    for (double x = x0; x < x0 * 2; x += x0 / 2048) {
                        MultiPrecision<Pow2.N16> ccdf = CDFLimit<Pow2.N16, Pow2.N32>.Value(x);
                        MultiPrecision<Pow2.N16> cdf = 0.5 - ccdf;

                        Console.WriteLine($"{x}\n{cdf:e108}\n{ccdf:e108}");
                        sw.WriteLine($"{x},{cdf:e108},{ccdf:e108}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

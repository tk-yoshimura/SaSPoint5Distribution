using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class PDFCoef {
        static void Main_() {
            //using (StreamWriter sw = new("../../../../results_disused/cdf_asympcoef.csv")) {
            //    for (int k = 0; k < 24; k++) {
            //        MultiPrecision<Pow2.N16> y = CDFLimit<Pow2.N16, Pow2.N16>.CoefTable(k);

            //        sw.WriteLine($"{k},{y}");
            //    }
            //}

            MultiPrecision<Pow2.N16> y = CDFLimit<Pow2.N16, Pow2.N32>.Value(6.0009765625) - CDFLimit<Pow2.N16, Pow2.N32>.Value(6);

            Console.WriteLine(y);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

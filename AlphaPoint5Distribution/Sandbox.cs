using MultiPrecision;

namespace AlphaPoint5Distribution {
    internal class Sandbox {
        static void Main_() {

            for (int exponent = 5; exponent > -24; exponent--) {
                Console.WriteLine(exponent);

                MultiPrecision<Pow2.N16> x = MultiPrecision<Pow2.N16>.Ldexp(1, exponent);
                bool complementary = true;

                MultiPrecision<Pow2.N16> y_dec = CDFN16.Value(MultiPrecision<Pow2.N16>.BitDecrement(x), complementary);
                MultiPrecision<Pow2.N16> y_0 = CDFN16.Value(x, complementary);
                MultiPrecision<Pow2.N16> y_inc = CDFN16.Value(MultiPrecision<Pow2.N16>.BitIncrement(x), complementary);

                Console.WriteLine(y_dec);
                Console.WriteLine(y_0);
                Console.WriteLine(y_inc);

                Console.WriteLine(y_0 == y_dec);
                Console.WriteLine(y_0 == y_inc);
            }

            //for (int exponent = 5; exponent > -24; exponent--) {
            //    Console.WriteLine(exponent);

            //    MultiPrecision<Pow2.N16> x = MultiPrecision<Pow2.N16>.Ldexp(1, exponent);

            //    MultiPrecision<Pow2.N16> y_dec = PDFN16.Value(MultiPrecision<Pow2.N16>.BitDecrement(x));
            //    MultiPrecision<Pow2.N16> y_0 = PDFN16.Value(x);
            //    MultiPrecision<Pow2.N16> y_inc = PDFN16.Value(MultiPrecision<Pow2.N16>.BitIncrement(x));

            //    Console.WriteLine(y_dec);
            //    Console.WriteLine(y_0);
            //    Console.WriteLine(y_inc);

            //    Console.WriteLine(y_0 == y_dec);
            //    Console.WriteLine(y_0 == y_inc);
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}

using MultiPrecision;
using SaSPoint5Expected;

namespace SaSPoint5ExpectedTest {
    [TestClass]
    public class PDFN16Test {
        [TestMethod]
        public void NearZeroTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N24  \t{x}\t {PDFNearZero<Pow2.N16, N24>.Value(x)}");
                Console.WriteLine($"N32  \t{x}\t {PDFNearZero<Pow2.N16, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {PDFNearZero<Pow2.N16, N48>.Value(x)}");
                Console.WriteLine($"Limit\t{x}\t {PDFLimit<Pow2.N16, N48>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void LimitTest() {
            for (int exp = -16; exp <= 16; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N24  \t{x}\t {PDFLimit<Pow2.N16, N24>.Value(x)}");
                Console.WriteLine($"N32  \t{x}\t {PDFLimit<Pow2.N16, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {PDFLimit<Pow2.N16, N48>.Value(x)}");
                Console.WriteLine("");
            }
        }

        

        [TestMethod]
        public void BitsTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                MultiPrecision<Pow2.N16> x = double.ScaleB(1, exp);

                MultiPrecision<Pow2.N16> y_dec = PDFN16.Value(MultiPrecision<Pow2.N16>.BitDecrement(x));
                MultiPrecision<Pow2.N16> y_cen = PDFN16.Value(x);
                MultiPrecision<Pow2.N16> y_inc = PDFN16.Value(MultiPrecision<Pow2.N16>.BitIncrement(x));

                Console.WriteLine(y_dec);
                Console.WriteLine(y_cen);
                Console.WriteLine(y_inc);

                Assert.IsTrue(MultiPrecision<Pow2.N16>.NearlyEqualBits(y_dec, y_cen, 1));
                Assert.IsTrue(MultiPrecision<Pow2.N16>.NearlyEqualBits(y_inc, y_cen, 1));
            }
        }
    }
}
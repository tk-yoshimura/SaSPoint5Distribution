using MultiPrecision;
using SaSPoint5Expected;

namespace SaSPoint5ExpectedTest {
    [TestClass]
    public class PDFN24Test {
        [TestMethod]
        public void NearZeroTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N32  \t{x}\t {PDFNearZero<N24, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {PDFNearZero<N24, N48>.Value(x)}");
                Console.WriteLine($"N64  \t{x}\t {PDFNearZero<N24, Pow2.N64>.Value(x)}");
                Console.WriteLine($"Limit\t{x}\t {PDFLimit<N24, N96>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void LimitTest() {
            for (int exp = -16; exp <= 16; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N32  \t{x}\t {PDFLimit<N24, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {PDFLimit<N24, N48>.Value(x)}");
                Console.WriteLine($"N64  \t{x}\t {PDFLimit<N24, Pow2.N64>.Value(x)}");
                Console.WriteLine($"N96  \t{x}\t {PDFLimit<N24, N96>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void BitsTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                for (double v = 1; v < 2; v += 1d / 32) {
                    MultiPrecision<N24> x = double.ScaleB(v, exp);

                    MultiPrecision<N24> y_dec = PDFN24.Value(MultiPrecision<N24>.BitDecrement(x));
                    MultiPrecision<N24> y_cen = PDFN24.Value(x);
                    MultiPrecision<N24> y_inc = PDFN24.Value(MultiPrecision<N24>.BitIncrement(x));

                    Console.WriteLine(y_dec);
                    Console.WriteLine(y_cen);
                    Console.WriteLine(y_inc);

                    Assert.IsTrue(MultiPrecision<N24>.NearlyEqualBits(y_dec, y_cen, 1));
                    Assert.IsTrue(MultiPrecision<N24>.NearlyEqualBits(y_inc, y_cen, 1));
                }
            }
        }
    }
}
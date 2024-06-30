using MultiPrecision;
using SaSPoint5Expected;

namespace SaSPoint5ExpectedTest {
    [TestClass]
    public class CDFN16Test {
        [TestMethod]
        public void NearZeroTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N24  \t{x}\t {0.5 - CDFNearZero<Pow2.N16, N24>.Value(x)}");
                Console.WriteLine($"N32  \t{x}\t {0.5 - CDFNearZero<Pow2.N16, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {0.5 - CDFNearZero<Pow2.N16, N48>.Value(x)}");
                Console.WriteLine($"Limit\t{x}\t {CDFLimit<Pow2.N16, N48>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void LimitTest() {
            for (int exp = -16; exp <= 16; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N24  \t{x}\t {CDFLimit<Pow2.N16, N24>.Value(x)}");
                Console.WriteLine($"N32  \t{x}\t {CDFLimit<Pow2.N16, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {CDFLimit<Pow2.N16, N48>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void BitsTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                for (double v = 1; v < 2; v += 1d / 32) {
                    MultiPrecision<Pow2.N16> x = double.ScaleB(v, exp);

                    MultiPrecision<Pow2.N16> y_dec = CDFN16.Value(MultiPrecision<Pow2.N16>.BitDecrement(x), complementary: true);
                    MultiPrecision<Pow2.N16> y_cen = CDFN16.Value(x, complementary: true);
                    MultiPrecision<Pow2.N16> y_inc = CDFN16.Value(MultiPrecision<Pow2.N16>.BitIncrement(x), complementary: true);

                    Console.WriteLine(y_dec);
                    Console.WriteLine(y_cen);
                    Console.WriteLine(y_inc);

                    Assert.IsTrue(MultiPrecision<Pow2.N16>.NearlyEqualBits(y_dec, y_cen, 1));
                    Assert.IsTrue(MultiPrecision<Pow2.N16>.NearlyEqualBits(y_inc, y_cen, 1));
                }
            }
        }
    }
}
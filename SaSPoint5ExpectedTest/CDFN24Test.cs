using MultiPrecision;
using SaSPoint5Expected;

namespace SaSPoint5ExpectedTest {
    [TestClass]
    public class CDF24Test {
        [TestMethod]
        public void NearZeroTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N32  \t{x}\t {0.5 - CDFNearZero<N24, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {0.5 - CDFNearZero<N24, N48>.Value(x)}");
                Console.WriteLine($"N64  \t{x}\t {0.5 - CDFNearZero<N24, Pow2.N64>.Value(x)}");
                Console.WriteLine($"Limit\t{x}\t {CDFLimit<N24, N96>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void LimitTest() {
            for (int exp = -16; exp <= 16; exp++) {
                Console.WriteLine($"2^{exp}");

                double x = double.ScaleB(1, exp);

                Console.WriteLine($"N32  \t{x}\t {CDFLimit<N24, Pow2.N32>.Value(x)}");
                Console.WriteLine($"N48  \t{x}\t {CDFLimit<N24, N48>.Value(x)}");
                Console.WriteLine($"N64  \t{x}\t {CDFLimit<N24, Pow2.N64>.Value(x)}");
                Console.WriteLine($"N96  \t{x}\t {CDFLimit<N24, N96>.Value(x)}");
                Console.WriteLine("");
            }
        }

        [TestMethod]
        public void BitsTest() {
            for (int exp = -32; exp <= -4; exp++) {
                Console.WriteLine($"2^{exp}");

                for (double v = 1; v < 2; v += 1d / 32) {
                    MultiPrecision<N24> x = double.ScaleB(v, exp);

                    MultiPrecision<N24> y_dec = CDFN24.Value(MultiPrecision<N24>.BitDecrement(x), complementary: true);
                    MultiPrecision<N24> y_cen = CDFN24.Value(x, complementary: true);
                    MultiPrecision<N24> y_inc = CDFN24.Value(MultiPrecision<N24>.BitIncrement(x), complementary: true);

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
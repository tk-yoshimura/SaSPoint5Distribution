using MultiPrecision;

namespace SaSPoint5Expected {
    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 1);
    }

    public struct N24 : IConstant {
        public readonly int Value => 24;
    }

    public struct N48 : IConstant {
        public readonly int Value => 48;
    }

    public struct N96 : IConstant {
        public readonly int Value => 96;
    }
}

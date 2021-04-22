using System;

namespace Test
{

    class Q
    {
        int _i;

        public Q(int i)
        {
            _i = i;
        }

    }

    class Test
    {
        public static void Main(params string[] args)
        {
            var i = new Q(1);

            Q j = new (1);

            A v1 = new A();
            v1.WriteOut1();

            A v2 = new B();
            v2.WriteOut1();

            B v3 = new B();
            v3.WriteOut1();

            ((B)v2).WriteOut1();

            C v4 = new C();
            v4.WriteOut1();

            B v5 = new C();
            v5.WriteOut1();
        }
    }

    class A
    {
        public string a;
        internal string b;
        protected string c;
        protected internal string d;
        private string e;
        private protected string f;

        static string g;
        public static string h;
        protected static string i;
        protected internal static string j = "A.j";

        public void WriteOut1()
        {
            Console.WriteLine($"{nameof(A)}.{nameof(WriteOut1)} {j}");
        }
    }

    class B : A 
    {
        protected internal static new string j = "B.j";
        public new virtual void WriteOut1()
        {
            Console.WriteLine($"{nameof(B)}.{nameof(WriteOut1)} {j}");
        }
    }

    class C : B
    {
        public override void WriteOut1()
        {
            Console.WriteLine($"{nameof(C)}.{nameof(WriteOut1)}");
        }
    }

    class D : A
    {
    }
}
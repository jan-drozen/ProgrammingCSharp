using System;

namespace Test01
{
    class Program
    {
        static void Main(string[] args)
        {
            //A objectA = new A();
            //objectA.WriteOutput();

            //A objectB = new B();
            //objectB.WriteOutput();

            A objectC = new C();
            objectC.WriteOutput();
        }
    }

    class A
    {
        public int intField;
        public void WriteOutput()
        {
            Console.WriteLine($"Result: {nameof(A)}.{nameof(WriteOutput)} {GetType()}");
        }
    }

    class B : A 
    {
        public int intField;
        public new virtual void WriteOutput()
        {
            Console.WriteLine($"Result: {nameof(B)}.{nameof(WriteOutput)} {GetType()}");
        }
    }

    class C : B
    {
        public static void Foo()
        {
            
        }

        public override void WriteOutput()
        {
            Type t = GetType();
            Type t2 = typeof(C);
            Console.WriteLine($"Result: {nameof(C)}.{nameof(WriteOutput)} {GetType()}");

            

        }
    }

}

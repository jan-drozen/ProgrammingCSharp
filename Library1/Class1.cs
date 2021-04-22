using System;

namespace Library1
{
    public class LibraryClass
    {
        public int libraryField;
        internal int internalLibraryField;
        protected internal int piLibraryField;
        private protected static int priLibraryField;

    }

    public class LibraryDescendant : LibraryClass
    {
        void Foo()
        {
            priLibraryField = 1;   
        }
    }
}

using static TechGen_Generics_Fundamentals.Program;

namespace TechGen_Generics_Fundamentals
{
    internal class Program
    {
        #region Exercise 1
        public static (T2, T1) SwapSides<T1, T2>((T1 item1, T2 item2) input)
        {
            return (input.item2, input.item1);
        }

        #endregion Exercise 1


        #region Exercise 2

        // helper function to print passed collections
        static void Print<T>(T[] collection)
        {
            foreach (T item in collection)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public static T[] Filter<T>(T[] collection, Predicate<T> condition)
        {
            List<T> filteredList = new List<T>();

            foreach (var item in collection)
            {
                if (condition(item))
                {
                    filteredList.Add(item);
                }
            }

            return filteredList.ToArray();
        }

        public static TOut[] Project<TIn, TOut>(TIn[] collection, Func<TIn, TOut> mutator)
        {
            List<TOut> mutatedList = new List<TOut>();

            for (int i = 0; i < collection.Length; i++)
            {
                mutatedList.Add(mutator(collection[i]));
            }

            return mutatedList.ToArray();
        }

        #endregion Exercise 2

        #region Exercise 3
        internal interface IInitializable
        {
            void Initialize();
        }

        class MyClass : IInitializable
        {
            public bool IsInitialized = false;

            public void Initialize()
            {
                IsInitialized = true;
            }   
        }

        static T CreateAndInitialize<T>() where T : IInitializable, new()
        {
            T obj = new T();
            obj.Initialize();
            return obj;
        }

        #endregion Exercise 3

        static void Main(string[] args)
        {
            // test cases for Ex1
            Console.WriteLine(SwapSides(("string", 1)));
            Console.WriteLine(SwapSides((45, 1)));
            Console.WriteLine(SwapSides((0, "text")));
            Console.WriteLine(SwapSides(('a', 1)));
            Console.WriteLine();

            // test case for Ex2
            int[] array = { 1, 2, 3, 4, 5, 6 };
            Console.WriteLine("Before filtering:");
            Print<int>(array);
            int[] array2 = Filter<int>(array, x => x % 2 == 0); //filter even values
            Console.WriteLine("After filtering:");
            Print<int>(array2);

            Console.WriteLine("After mutation:");
            string[] mutated = Project<int, string>(array2, (x) => $"N{x}");
            Print<string>(mutated);

        }
    }
}

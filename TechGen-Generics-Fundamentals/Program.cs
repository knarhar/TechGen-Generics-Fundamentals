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

        #region Exercise 4

        class Buffer<T>
        {
            int _capacity;
            List<T> buffer;
            Comparer<T> comparer;

            public Buffer(int capacity, Comparer<T> comparer)
            {
                _capacity = capacity;
                buffer = new List<T>(_capacity);
                this.comparer = comparer;
            }

            public void Add(T item)
            {
                buffer.Add(item);
                buffer.Sort((a, b) => comparer.Compare(b, a)); // descending sort
                if (buffer.Count > _capacity)
                {
                    buffer.RemoveAt(buffer.Count - 1);
                }
            }

            public T[] Snapshot()
            {
                return buffer.ToArray();
            }
        }

        #endregion Exercise 4

        #region Exercise 5
        public sealed class Result<T>
        {
            public bool Success { get; }
            public T? Value { get; }
            public Exception? Exception { get; }
            public string? Error { get; }
            public int Attempt { get; }

            private Result(bool success, T? value, Exception? exception, int attempt)
            {
                Success = success;
                Value = value;
                Exception = exception;
                Error = exception?.Message;
                Attempt = attempt;
            }

            public static Result<T> Ok(T value, int attempt = 1)
            {
                return new Result<T>(true, value, null, attempt);
            }

            public static Result<T> Fail(Exception exception, int attempt)
            {
                return new Result<T>(false, default, exception, attempt);
            }

            public override string ToString()
            {
                return $"Success: {Success}\nValue: {Value}\nError: {Error}\nAttempts: {Attempt}\n";
            }
        }

        public static Result<T> Execute<T>(
            Func<T> operation, 
            int maxAttempts, 
            Func<Exception, bool>? shouldRetry = null
        ) {
            Exception? lastError = null;
            int attempt = 0;

            for (attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    T value = operation();
                    return Result<T>.Ok(value, attempt);
                }
                catch (Exception e)
                {
                    lastError = e;

                    bool retry = shouldRetry?.Invoke(e) ?? true;
                    if (!retry)
                        break;
                }
            }

            int finalAttempts = attempt > maxAttempts ? maxAttempts : attempt;
            return Result<T>.Fail(lastError!, finalAttempts);
        }

        #endregion Exercise 5

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

            // test case for Ex3
            var instance = CreateAndInitialize<MyClass>();
            Console.WriteLine(instance.IsInitialized);

            // test case for Ex4
            Buffer<int> buffer1 = new Buffer<int>(2, Comparer<int>.Default);
            foreach (int n in new[] { 5, 1, 9, 3, 7, 2 })
                buffer1.Add(n);
            Console.WriteLine("N=2: " + string.Join(", ", buffer1.Snapshot()));

            Buffer<int> buffer2 = new Buffer<int>(3, Comparer<int>.Default);
            foreach (int n in new[] { 5, 1, 9, 3 })
                buffer2.Add(n);
            Console.WriteLine("N=3: " + string.Join(", ", buffer2.Snapshot()));
            Console.WriteLine();


            // test case for Ex5
            int counter1 = 0;
            var result1 = Execute<int>(() =>
            {
                counter1++;
                if (counter1 > 0) throw new Exception("fail");
                return 42;
            }, maxAttempts: 5, shouldRetry: ex => counter1 < 3);
            Console.WriteLine(result1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ExceptPerformance
{
    public class Program
    {
        private static Random _randomGenerator = new Random();
        private static Stopwatch _stopWatch = new Stopwatch();
        private static int _iterations = 1000000;

        static void Main(string[] args)
        {
            var (all, subset) = GenerateLists();

            LinqExceptMethod(all, subset);
            LeftJoinMethod(all, subset);
            HashSetMethod(all, subset);
            HashSetWithConversionMethod(all, subset);

            Console.ReadLine();
        }

        private static (IEnumerable<Example>, IEnumerable<Example>) GenerateLists()
        {
            var all = new List<Example>();
            var subset = new List<Example>();

            for (var i = 0; i < _iterations; i++)
            {
                var exampleObject = Example.Generate();

                if (_randomGenerator.NextDouble() >= 0.1)
                    subset.Add(exampleObject);

                all.Add(exampleObject);
            }
            return (all, subset);
        }

        private static void LinqExceptMethod(IEnumerable<Example> all, IEnumerable<Example> subset)
        {
            _stopWatch.Start();

            var setDifference = all.Except(subset);

            _stopWatch.Stop();

            Console.WriteLine($"Linq except method: {_stopWatch.ElapsedTicks}");

            _stopWatch.Reset();
        }


        private static void LeftJoinMethod(IEnumerable<Example> all, IEnumerable<Example> subset)
        {
            _stopWatch.Start();

            var setDifference2 = from record in all
                                 join s in subset on record.Id equals s.Id into q
                                 from p in q.DefaultIfEmpty()
                                 where p == null
                                 select p;

            _stopWatch.Stop();

            Console.WriteLine($"Left join method: {_stopWatch.ElapsedTicks}");

            _stopWatch.Reset();
        }

        private static void HashSetMethod(IEnumerable<Example> all, IEnumerable<Example> subset)
        {
            var subsetHashset = subset.ToHashSet(); // conversion time not included

            _stopWatch.Start();

            var setDifference3 = all.Where(x => !subsetHashset.Contains(x));

            _stopWatch.Stop();

            Console.WriteLine($"Hash set method: {_stopWatch.ElapsedTicks}");

            _stopWatch.Reset();
        }

        private static void HashSetWithConversionMethod(IEnumerable<Example> all, IEnumerable<Example> subset)
        {
            _stopWatch.Start();

            var subsetHashset = subset.ToHashSet(); // conversion time included
            var setDifference3 = all.Where(x => !subsetHashset.Contains(x));

            _stopWatch.Stop();

            Console.WriteLine($"Hash set (with conversion) method: {_stopWatch.ElapsedTicks}");

            _stopWatch.Reset();
        }
    }
}

using System;

namespace ExceptPerformance
{
    public class Example
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Hash { get; set; }

        private static int _index = 0;

        public static Example Generate()
        {
            _index += 1;

            return new Example
            {
                Id = _index,
                Name = Guid.NewGuid().ToString(),
                Hash = Guid.NewGuid().ToString(),
            };
        }
    }
}

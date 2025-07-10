using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievilArchipelago
{
    public static class Extensions
    {
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            return values.Any(value => source.Contains(value));
        }
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] values)
        {
            return values.Any(value => source.Contains(value));
        }
        public static bool ContainsAny(this string source, IEnumerable<string> values)
        {
            return values.Any(value => source.Contains(value));
        }
        public static bool ContainsAny(this string source, params string[] values)
        {
            return values.Any(value => source.Contains(value));
        }
        public static bool ContainsAny(this IEnumerable<string> source, IEnumerable<string> searchTerms)
        {
            return source.Any(item => searchTerms.Any(term => item.Contains(term)));
        }
        public static bool ContainsAny(this IEnumerable<string> source, params string[] searchTerms)
        {
            return source.Any(item => searchTerms.Any(term => item.Contains(term)));
        }
    }
}

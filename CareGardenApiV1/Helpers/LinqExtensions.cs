using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CareGardenApiV1.Helpers
{
    public static class LinqExtension
    {
        public static IQueryable<TSource> WhereIf<TSource>
        (this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>
        (this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<TSource> OrderByIf<TSource, TKey>
    (this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> keySelector)
        {
            return condition ? source.OrderBy(keySelector) : source.OrderBy(x => 0);
        }

        public static IEnumerable<TSource> OrderByDescendingIf<TSource, TKey>
            (this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> keySelector)
        {
            return condition ? source.OrderByDescending(keySelector) : source.OrderByDescending(x => 0);
        }
    }
}

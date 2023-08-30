using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsWebsite.Common
{
    public static class ObjectExtensions
    {
        public static void CheckArgumentIsNull(this object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }

        public static IEnumerable<IEnumerable<TValue>> Chunk<TValue>(this IEnumerable<TValue> values, Int32 chunkSize)
        {
            var valuesList = values.ToList();
            var count = valuesList.Count();
            for (var i = 0; i < (count / chunkSize) + (count % chunkSize == 0 ? 0 : 1); i++)
            {
                yield return valuesList.Skip(i * chunkSize).Take(chunkSize);
            }
        }

    }
}

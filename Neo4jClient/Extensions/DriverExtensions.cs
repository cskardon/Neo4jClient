using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Neo4jClient.Extensions
{
    using System.Linq;
    using Neo4j.Driver;

    public static class DriverExtensions
    {

        public static IAsyncSession AsyncSession(this IDriver driver, Version databaseVersion, string database, bool isWrite, IEnumerable<string> bookmarks)
        {
            IEnumerable<Bookmark> properBookmarks = null;
            if (bookmarks != null)
                properBookmarks = bookmarks.Select(b => Bookmark.From(b));

            return driver.AsyncSession(databaseVersion, database, isWrite, properBookmarks);
        }

        public static IAsyncSession AsyncSession(this IDriver driver, Version databaseVersion, string database, bool isWrite, IEnumerable<Bookmark> bookmarks)
        {
            return driver.AsyncSession(builder =>
            {
                if(databaseVersion.Major >= 4)
                    builder.WithDatabase(database);
                builder.WithDefaultAccessMode(isWrite ? AccessMode.Write : AccessMode.Read);
                if (bookmarks != null)
                    builder.WithBookmarks(bookmarks.ToArray());
            });
        }
    }

    public static class ZonedDateTimeExtensions
    {
        private static Regex ZtdRegex = 
            new Regex(".*ZonedDateTime{EpochSeconds:(?<epoch>[0-9]*)\\sNanos:(?<nanos>[0-9]*)\\sZone:(?<zone>.*)}", 
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public static DateTimeOffset FromZonedDateTimeString(this string ztd)
        {
            //Lazy-ZonedDateTime{EpochSeconds:1577876400 Nanos:0 Zone:Z}
            if(int.TryParse(ZtdRegex.Match(ztd).Groups["epoch"].ToString(), out int epochSeconds ))
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epochSeconds);
            }
            return DateTimeOffset.MinValue;
        }
    }
}

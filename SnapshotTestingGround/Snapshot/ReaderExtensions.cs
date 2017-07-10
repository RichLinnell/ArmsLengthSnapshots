using System;
using System.Collections.Generic;

namespace SnapshotTestingGround.Snapshot
{
    public static class ReaderExtensions
    {
        public static long ReadSnapshotValue(this long _, string value)
        {
            return long.Parse(value);
        }
        public static string ReadSnapshotValue(this string _, string value)
        {
            return value;
        }

        public static DateTime ReadSnapshotValue(this DateTime _, string value)
        {
            return DateTime.Parse(value);
        }
    }

    public static class WriterExtensions
    {
        public static string WriteSnapshotValue(this long value)
        {
            return value.ToString();
        }

        public static string WriteSnapshotValue(this string value)
        {
            return value;
        }
        public static string WriteSnapshotValue(this DateTime value)
        {
            return value.ToLongDateString();
        }

        

        public static string WriteSnapshotValue(this KeyValuePair<string, string> value)
        {
            return $"{value.Key}|{value.Value}";
        }
    }
}
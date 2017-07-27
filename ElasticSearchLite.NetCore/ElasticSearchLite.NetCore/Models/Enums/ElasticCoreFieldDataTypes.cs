using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models.Enums
{
    public sealed class ElasticCoreFieldDataTypes
    {
        private static List<ElasticCoreFieldDataTypes> _items = new List<ElasticCoreFieldDataTypes>();
        public string Name { get; }
        public static IEnumerable<ElasticCoreFieldDataTypes> Items => _items;
        private ElasticCoreFieldDataTypes(string name)
        {
            Name = name;
            _items.Add(this);
        }
        /// <summary>
        /// Indexed full-text value
        /// </summary>
        public static ElasticCoreFieldDataTypes Text { get; } = new ElasticCoreFieldDataTypes("text");
        /// <summary>
        /// Only searchable by their exact value
        /// </summary>
        public static ElasticCoreFieldDataTypes Keyword { get; } = new ElasticCoreFieldDataTypes("keyword");
        /// <summary>
        /// Signed 64-bit integer (-2^63 - 2^63-1)
        /// </summary>
        public static ElasticCoreFieldDataTypes Long { get; } = new ElasticCoreFieldDataTypes("long");
        /// <summary>
        /// Signed 32-bit integer (-2^31 - 2^31-1)
        /// </summary>
        public static ElasticCoreFieldDataTypes Integer { get; } = new ElasticCoreFieldDataTypes("integer");
        /// <summary>
        /// Signed 16-bit integer (-32.768 - 32.767)
        /// </summary>
        public static ElasticCoreFieldDataTypes Short { get; } = new ElasticCoreFieldDataTypes("short");
        /// <summary>
        /// Signed 8-bit integer (-128 - 127)
        /// </summary>
        public static ElasticCoreFieldDataTypes Byte { get; } = new ElasticCoreFieldDataTypes("byte");
        /// <summary>
        /// 64-bit double precision floating point.
        /// </summary>
        public static ElasticCoreFieldDataTypes Double { get; } = new ElasticCoreFieldDataTypes("double");
        /// <summary>
        /// 32-bit single precision floating point.
        /// </summary>
        public static ElasticCoreFieldDataTypes Float { get; } = new ElasticCoreFieldDataTypes("float");
        /// <summary>
        /// 16-bit half precision floating point.
        /// </summary>
        public static ElasticCoreFieldDataTypes HalfFloat { get; } = new ElasticCoreFieldDataTypes("half_float");
        /// <summary>
        /// A floating point that is backed by a long and a fixed scaling factor.
        /// </summary>
        public static ElasticCoreFieldDataTypes ScaledFloat { get; } = new ElasticCoreFieldDataTypes("scaled_float");
        /// <summary>
        /// Date type - strings containing formatted dates, e.g. "2015-01-01" or "2015/01/01 12:10:30".
        /// </summary>
        public static ElasticCoreFieldDataTypes Date { get; } = new ElasticCoreFieldDataTypes("date");
        /// <summary>
        /// Boolean type - accepts JSON true, false values.
        /// </summary>
        public static ElasticCoreFieldDataTypes Boolean { get; } = new ElasticCoreFieldDataTypes("boolean");
    }
}
using System;
using System.Text;

namespace Orzoo.AspNet.Test
{
    public static class TestDataGenerator
    {
        #region Static Fields and Constants

        public static string[] PersonNames = new string[] {"张三", "李四", "王五", "赵六", "孙德彪"};

        public static string[] IdCards = new string[]
        {"51043319821105783X", "210433197701057832", "611423196701057832", "251423199701057832", "751423194706257832"};

        public static string[] Regions = new string[] {"51", "61", "62", "63", "31"};

        #endregion

        #region Properties, Indexers

        public static Random TheRandom { get; set; }

        #endregion

        #region Methods

        #region Private Methods

        private static string GetGB2312String(int count)
        {
            var random = TheRandom;
            var bs = new byte[count*2];
            for (var i = 0; i < count; i++)
            {
                var c = GetGB2312Char(random);
                bs[i*2] = (byte) (c.X + 0xa0);
                bs[i*2 + 1] = (byte) (c.Y + 0xa0);
            }
            return Encoding.GetEncoding("GB2312").GetString(bs);
        }

        private static Point GetGB2312Char(Random random)
        {
            // 国标一级字(共3755个): 区:16-55, 位:01-94, 55区最后5位为空位
            var qu = random.Next(40) + 16;
            var wei = random.Next(qu == 55 ? 89 : 94) + 1;
            return new Point() {X = qu, Y = wei};
        }

        #endregion

        #region Public Methods

        public static string RandomData(string[] list)
        {
            var r = TheRandom.Next(list.Length);
            return list[r];
        }

        public static double GetNumber(double minimum = 0, double maximum = 1000)
        {
            return TheRandom.NextDouble()*(maximum - minimum) + minimum;
        }

        public static string GetPersonName()
        {
            return RandomData(PersonNames);
        }

        public static string GetIdCard()
        {
            return RandomData(IdCards);
        }

        public static string GetRegion()
        {
            return RandomData(Regions);
        }

        public static DateTime GetDate(DateTime? from = null, DateTime? to = null)
        {
            from = from ?? DateTime.Now;
            to = to ?? DateTime.Now.AddDays(10);

            var range = new TimeSpan(to.Value.Ticks - from.Value.Ticks);


            var randTimeSpan = new TimeSpan((long) (range.TotalSeconds - TheRandom.Next(0, (int) range.TotalSeconds)));

            return from.Value + randTimeSpan;
        }

        public static string GetSimpleChinese(int length)
        {
            return GetGB2312String(length);
            //// TODO: 这里出现了伪随机现象
            //var ranges = new List<Range>
            //{
            //    new Range(0x4e00, 0x9FBF)
            //};
            //var builder = new StringBuilder(length);
            //for (var i = 0; i < length; i++)
            //{
            //    var rangeIndex = TheRandom.Next(ranges.Count);
            //    var range = ranges[rangeIndex];
            //    builder.Append((char)TheRandom.Next(range.Begin, range.End));
            //}
            //return builder.ToString();
        }

        #endregion

        #endregion

        #region Nested type: Point

        private class Point
        {
            #region Properties, Indexers

            public int X { get; set; }
            public int Y { get; set; }

            #endregion
        }

        #endregion

        #region Nested type: Range

        private class Range
        {
            #region Constructors

            public Range(int begin, int end)
            {
                Begin = begin;
                End = end;
            }

            #endregion

            #region Properties, Indexers

            public int Begin { get; set; }

            public int End { get; set; }

            #endregion
        }

        #endregion
    }
}
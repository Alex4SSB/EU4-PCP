using EU4_PCP;
using EU4_PCP_Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using static EU4_PCP.PCP_Implementations;

namespace EU4_PCP_Test
{
    [TestClass]
    public class Tests
    {
        private readonly string TestFiles = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestFiles\"));

        [TestMethod()]
        public void AddTest()
        {
            var arr = new string[1];

            for (int i = 0; i < 10; i++)
            {
                Add(ref arr, $"{i}");
            }

            Assert.IsTrue(arr.Count(cell => !string.IsNullOrEmpty(cell)) == 10);
        }

        [TestMethod()]
        public void RangeTestPositive()
        {
            int x = 5, a = 0, b = 10;

            Assert.IsTrue(x.Range(a, b));
        }

        [TestMethod()]
        public void RangeTestNegative()
        {
            int x = 15, a = 0, b = 10;

            Assert.IsFalse(x.Range(a, b));
        }

        [TestMethod()]
        public void ToIntTest()
        {
            string s = "30";

            int n = s.ToInt();

            Assert.AreEqual(30, n);
        }

        [TestMethod()]
        public void ToByteTestPositive()
        {
            string[] s = { "10", "135", "240" };

            Assert.IsTrue(s.ToByte(out byte[] res, 0));

            Assert.AreEqual(3, res.Length);

            Assert.AreEqual(10, res[0]);
            Assert.AreEqual(135, res[1]);
            Assert.AreEqual(240, res[2]);
        }

        [TestMethod()]
        public void ToByteTestNegative()
        {
            string[] s = { "-1", "500", "a" };

            Assert.IsFalse(s.ToByte(out byte[] res, 0));

            Assert.AreEqual(3, res.Length);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(0, res[i]);
            }
        }

        [TestMethod()]
        public void GtTestPositive()
        {
            string a = "4", b = "6";
            string c = " 4 ", d = " 6 ";

            Assert.IsTrue(b.Gt(a));

            Assert.IsTrue(d.Gt(c));
        }

        [TestMethod]
        public void GtTestNegative()
        {
            string a = "4", b = "6";

            Assert.IsFalse(a.Gt(b));

            Assert.IsFalse(a.Gt(a));
        }

        [TestMethod]
        public void GeTestPositive()
        {
            string a = "4", b = "6";
            string c = " 4 ", d = " 6 ";

            Assert.IsTrue(b.Ge(a));

            Assert.IsTrue(a.Ge(a));

            Assert.IsTrue(d.Ge(c));
        }

        [TestMethod]
        public void GeTestNegative()
        {
            string a = "4", b = "6";

            Assert.IsFalse(a.Ge(b));
        }

        [TestMethod]
        public void IncTest()
        {
            string n = "30";

            Assert.AreEqual("35", Inc(n, 5));
        }

        [TestMethod()]
        public void NextLineTestPositive()
        {
            string[] lines =
            {
                "  owner = SWE",
                "owner = SWE#",
                "\towner = SWE",
                "\t \t owner = SWE"
            };
            string[] lines2 =
            {
                "no assignment",
                "}",
                "{"
            };

            for (int i = 0; i < lines.Length; i++)
            {
                Assert.IsFalse(NextLine(lines[i]));

                if (lines2.Length > i)
                    Assert.IsFalse(NextLine(lines2[i], true));
            }
        }

        [TestMethod]
        public void NextLineTestNegative()
        {
            string[] lines =
            {
                "#  owner = SWE",
                "\t",
                "\t\t",
                "  ",
                "  #owner = SWE",
                "\t#owner = SWE",
                "\t \t #owner = SWE",
                "no assignment"
            };

            for (int i = 0; i < lines.Length; i++)
            {
                Assert.IsTrue(NextLine(lines[i]));
            }
        }

        [TestMethod]
        public void DateParserTestPositive()
        {
            string[] dates =
            {
                "1444.11.11",
                "2.12.31",
                "790.4.20",
                "70.11.3"
            };
            DateTime[] original = {
                new DateTime(1444, 11, 11),
                new DateTime(2, 12, 31),
                new DateTime(790, 4, 20),
                new DateTime(70, 11, 3)
            };

            for (int i = 0; i < dates.Length; i++)
            {
                Assert.AreEqual(original[i], DateParser(dates[i], true));
            }

            Assert.AreEqual(original[0], DateParser(dates[0]));
        }

        [TestMethod]
        public void DateParserTestNegative()
        {
            string[] dates =
            {
                "2000.2.31",
                "",
                "4.5",
                "-1",
                "0",
                "not_a_date"
            };

            for (int i = 0; i < dates.Length; i++)
            {
                Assert.AreEqual(DateTime.MinValue, DateParser(dates[i], true));
                Assert.AreEqual(DateTime.MinValue, DateParser(dates[i]));
            }
        }

        [TestMethod]
        public void LastEventTest()
        {
            DateTime[] dates = {
                new DateTime(500, 1, 1),
                new DateTime(1500, 1, 1)
            };

            string[] owners = {
                "BYZ",
                "TUR"
            };

            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < dates.Length; i++)
            {
                Assert.AreEqual(owners[i], LastEvent(Resources.Prov_151, EventType.Province, dates[i]));
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed); // Trace.WriteLine() for main app
        }

        [TestMethod]
        public void LastCultureEventTest()
        {
            DateTime[] dates = {
                new DateTime(1000, 1, 1),
                new DateTime(1500, 1, 1)
            };

            Assert.AreEqual("lombard", LastEvent(Resources.ACH, EventType.Country, dates[0]));
            Assert.AreEqual("greek", LastEvent(Resources.ACH, EventType.Country, dates[1]));
        }

        [TestMethod]
        public void RandomProvColorTest()
        {
            var testProv = new Dictionary<int, Province>() {
                { 0, new Province(color: Color.FromRgb(130, 12, 56)) },
                { 1, new Province(color: Color.FromRgb(1, 40, 100)) },
                { 2, new Province(color: Color.FromRgb(78, 32, 47)) },
                { 3, new Province(color: Color.FromRgb(23, 190, 200)) },
                { 4, new Province(color: Color.FromRgb(90, 212, 231)) }
            };

            Color newColor = RandomProvColor(testProv);

            Assert.IsTrue(!testProv.Values.Any(p => p.Color == newColor));

            // Lock tests
            int val1 = 10, val2 = 20;

            newColor = RandomProvColor(testProv, val1); // lock red
            Assert.IsTrue(!testProv.Values.Any(p => p.Color == newColor));
            Assert.IsTrue(newColor.R == val1);

            newColor = RandomProvColor(testProv, val1, val2); // lock red & green
            Assert.IsTrue(!testProv.Values.Any(p => p.Color == newColor));
            Assert.IsTrue(newColor.R == val1);
            Assert.IsTrue(newColor.G == val2);

            newColor = RandomProvColor(testProv, -1, val1, val2); // lock green & blue
            Assert.IsTrue(!testProv.Values.Any(p => p.Color == newColor));
            Assert.IsTrue(newColor.G == val1);
            Assert.IsTrue(newColor.B == val2);
        }

        [TestMethod]
        public void ToCsvTest()
        {
            // Also tests IsRNW

            var colors = new List<Color>() {
                Color.FromRgb(130, 12, 56),
                Color.FromRgb(1, 40, 100),
                Color.FromRgb(78, 32, 47),
                Color.FromRgb(23, 190, 200),
                Color.FromRgb(90, 212, 231)
            };
            var testProv = new List<Province>() {
                new (index: 1, color: colors[0], name: "prov1"),
                new (index: 2000, color: colors[1], name: "prov2000"),
                new (index: 3, color: colors[2], name: "Unused1"),
                new (index: 4, color: colors[3], name: "UnusedLand1"),
                new (index: 5, color: colors[4], name: "RNW"),
                new (index: 773, color: new (238, 42, 192), name: "Banda Oriente"),
                new (index: 4500, color: new (-1, -1, 100), name: ""),
                new (index: 40, color: new(10, 20, 30), name: new("def", alt: "alt"))
            };

            Assert.IsTrue(testProv[0].ToCsv() == "1;130;12;56;prov1;x");
            Assert.IsTrue(testProv[1].ToCsv() == "2000;1;40;100;prov2000;x");
            Assert.IsTrue(testProv[2].ToCsv() == "3;78;32;47;Unused1;");
            Assert.IsTrue(testProv[3].ToCsv() == "4;23;190;200;UnusedLand1;");
            Assert.IsTrue(testProv[4].ToCsv() == "5;90;212;231;RNW;");
            Assert.IsTrue(testProv[5].ToCsv() == "773;238;42;192;Banda Oriente;x");
            Assert.IsTrue(testProv[6].ToCsv() == "4500;;;100;;");
            Assert.IsTrue(testProv[7].ToCsv() == "40;10;20;30;def;alt");
        }

        [TestMethod]
        public void DefinReadPositiveTest()
        {
            var dict = DefinRead($@"{TestFiles}\definition.csv");

            Assert.IsTrue(dict.Values.Count(prov => prov) == 8);
            Assert.IsTrue(dict[2905].Name.ToString() == "Rio Das Mortes");
            Assert.IsTrue(dict.Values.Count(prov => prov.IsRNW()) == 3);
            Assert.IsTrue(dict[2].Color == Color.FromRgb(0, 36, 128));
        }

        [TestMethod]
        public void DefinReadNegativeTest()
        {
            var list = DefinRead($@"{TestFiles}\definition_negative.csv");

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void DefinParsePositiveTest()
        {
            Assert.IsTrue(DefinParse("4941;110;47;45;East Utah;", true) is not null);
            Assert.IsTrue(DefinParse("4942;10;77;55;;", true) is not null);

            Assert.IsTrue(DefinParse("4952;110;167;;;", false) is not null);
            Assert.IsTrue(DefinParse("4954;;;;;", false) is not null);
        }

        [TestMethod]
        public void DefinParseNegativeTest()
        {
            Assert.IsTrue(DefinParse("4952;110;167;;;", true) is null);
            Assert.IsTrue(DefinParse("4954;;;;;", true) is null);

            Assert.IsTrue(DefinParse(";3;251;221;;", false) is null);
        }

        [TestMethod]
        public void ColorExistTest()
        {
            var colors = new List<Color>() {
                Color.FromRgb(130, 12, 56),
                Color.FromRgb(1, 40, 100),
                Color.FromRgb(78, 32, 47),
                Color.FromRgb(23, 190, 200),
                Color.FromRgb(90, 212, 231)
            };
            var testProv = new Dictionary<int, Province>
            {
                { 0, new Province(index: 0, color: colors[0]) },
                { 1, new Province(index: 1, color: colors[1]) },
                { 2, new Province(index: 2, color: colors[2]) },
                { 3, new Province(index: 3, color: colors[3]) },
                { 4, new Province(index: 4, color: colors[4]) },
                { 5, new Province(index: 5, color: new P_Color(238, 42, 192)) },
                { 6, new Province(index: 6, color: new P_Color(-1, -1, 100)) }
            };

            Assert.IsTrue(ColorExist(new P_Color(0, 1, 2), testProv) == false);
            Assert.IsTrue(ColorExist(new P_Color(1, 40, 100), testProv) == true);
            Assert.IsTrue(ColorExist(new P_Color(1, 40, 100), testProv, testProv[1]) == false);
        }

        [TestMethod]
        public void DigitStrTest()
        {
            Assert.IsTrue(DigitStr("135") == "135");
            Assert.IsTrue(DigitStr("140u") == "140");

            Assert.IsFalse(DigitStr("q45") == "45");
        }

        [TestMethod]
        public void CultureSetupTest()
        {
            List<Culture> cultures = new();

            CultureSetup($@"{TestFiles}\00_cultures.txt", new object(), cultures);

            Assert.IsTrue(cultures.Count == 10);
            Assert.IsTrue(cultures.Count(c => c.Group && c.Group.Name == "germanic") == 3);
            Assert.IsTrue(cultures.Find(c => c.Name == "swedish").Group.Name == "scandinavian");
        }
    }
}

﻿using EU4_PCP;
using EU4_PCP_Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;
using static EU4_PCP.PCP_Const;
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
        public void ProvinceOwnerTest()
        {
            // Also tests LastEvent

            DateTime[] dates = {
                new DateTime(200, 1, 1),
                new DateTime(500, 1, 1),
                new DateTime (1250, 1, 1),
                new DateTime(1300, 1, 1),
                new DateTime(1500, 1, 1),
                new DateTime(1950, 1, 1),
            };

            string[] owners = {
                "ROM",
                "BYZ",
                "LAT",
                "BYZ",
                "TUR",
                "TKY",
            };

            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < dates.Length; i++)
            {
                Assert.AreEqual(owners[i], OwnerOrCulture(Resources.Prov_151, EventType.Province, dates[i]));
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed); // Trace.WriteLine() for main app
        }

        [TestMethod]
        public void CountryCultureTest()
        {
            // Also tests LastEvent

            DateTime[] dates = {
                new DateTime(800, 1, 1),
                new DateTime(910, 1, 1),
                new DateTime(935, 1, 1),
                new DateTime(945, 1, 1),
                new DateTime(955, 1, 1),
                new DateTime(1500, 1, 1)
            };

            string[] cultures = {
                "albanian",
                "lombard",
                "lombard",
                "lombard",
                "lombard",
                "greek",
            };

            for (int i = 1; i < 5; i++)
            {
                Assert.AreEqual(cultures[i], OwnerOrCulture(Resources.ACH, EventType.Country, dates[i]));
            }
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

        [TestMethod]
        public void MarkerPrepTest()
        {
            // Also tests DupliPrep

            var testProv = new List<Province>
            {
                { new Province(index: 1, color: new P_Color(10, 20, 30), name: "a") },      // dupli with #4
                { new Province(index: 2, color: new P_Color(-1, -1, 100), name: "b") },     // illegal color
                { new Province(index: 3, color: new P_Color(1, 41, 5)) },                   // illegal name
                { new Province(index: 4, color: new P_Color(10, 20, 30), name: "c") },      // dupli with #1
                { new Province(index: 5, color: new P_Color(100, 200, 100), name: "d") },   // dupli with #6
                { new Province(index: 6, color: new P_Color(100, 200, 100), name: "e") },   // dupli with #5
                { new Province(index: 7, color: new P_Color(200, 10, 1), name: "RNW") },    // RNW, dupli with #8
                { new Province(index: 8, color: new P_Color(200, 10, 1), name: "RNW") },    // RNW, dupli with #7
            };

            DupliPrep(testProv);
            var markers = MarkerPrep(testProv, true, true);

            Assert.IsFalse(markers.Any(m => (new[] { 1, 4, 5, 6, 7, 8 }).Contains(m.Item1.Index) && m.Item2.Color != RedBackground));
            Assert.IsFalse(markers.Any(m => (new[] { 2, 3 }).Contains(m.Item1.Index) && m.Item2.Color != PurpleBackground));
            Assert.IsFalse(markers.Any(m => m.Item3 < 0));
            Assert.IsTrue(markers.Find(m => m.Item1.Index == 8).Item1.NextDupli.Index == 7);

            var illegalMarkers = MarkerPrep(testProv, false, true);
            Assert.IsFalse(illegalMarkers.Any(m => m.Item2.Color == RedBackground));
            Assert.IsFalse(illegalMarkers.Any(m => (new[] { 2, 3 }).Contains(m.Item1.Index) && m.Item2.Color != PurpleBackground));
            Assert.IsFalse(illegalMarkers.Any(m => m.Item3 < 0));

            var dupliMarkers = MarkerPrep(testProv, true, false);
            Assert.IsFalse(dupliMarkers.Any(m => m.Item2.Color == PurpleBackground));
            Assert.IsFalse(dupliMarkers.Any(m => (new[] { 1, 4, 5, 6 }).Contains(m.Item1.Index) && m.Item2.Color != RedBackground));
            Assert.IsFalse(dupliMarkers.Any(m => m.Item3 < 0));

            var noMarkers = MarkerPrep(testProv, false, false);
            Assert.IsFalse(noMarkers.Any());
        }

        [TestMethod]
        public void BookmarkCtorTest()
        {
            DateTime date = new(2012, 9, 11); // it's the best
            const string name = "Original bookmark", code = "ORIGIN_BOOK";

            Bookmark origin = new()
            {
                Code = code,
                Date = date,
                Name = name,
                IsDefault = true
            };
            Bookmark other = new(origin);

            Assert.AreEqual(code, other.Code);
            Assert.AreEqual(name, other.Name);
            Assert.AreEqual(date, other.Date);
            Assert.IsTrue(other.IsDefault);

            ListBookmark book = new(origin);

            Assert.AreEqual(name, book.Name);
            Assert.AreEqual("2012-Sep-11", book.DateString);
        }

        [TestMethod]
        public void ProvNameTest()
        {
            var provNames = ProvNamePrep(new($@"{TestFiles}\cornish.txt"));

            Assert.AreEqual("cornish", provNames.Name);
            Assert.AreEqual("Kernow", provNames.ProvNames[233]);
            Assert.IsTrue(provNames.ProvNames.Count == 11);
        }

        [TestMethod]
        public void BookPrepTest()
        {
            var ottoBook = BookPrep($@"{TestFiles}\rise_of_the_ottomans.txt");

            Assert.IsTrue(ottoBook.Code == "RISE_OF_THE_OTTOMANS");
            Assert.IsTrue(ottoBook.Date == new DateTime(1444, 11, 11));
            Assert.IsTrue(ottoBook.IsDefault);

            var civilBook = BookPrep($@"{TestFiles}\american_civil_war.txt");

            Assert.IsTrue(civilBook.Code == "AMERICAN_CIVIL_WAR_NAME");
            Assert.IsTrue(civilBook.Date == new DateTime(1861, 7, 1));
        }

        [TestMethod]
        public void ModPrepTest()
        {
            // Also tests ModPathPrep and ReplacePrep

            var eb = ModPrep($@"{TestFiles}\mods\EnhancedBritain.mod", TestFiles);

            Assert.IsTrue(eb.Name == "Enhanced Britain");
            Assert.IsTrue(!string.IsNullOrEmpty(eb.Path));
            Assert.IsTrue(eb.GameVer == "1.21");

            var tot = ModPrep($@"{TestFiles}\mods\typus.mod", TestFiles);

            Assert.IsTrue(!string.IsNullOrEmpty(tot.Path));

            var et = ModPrep($@"{TestFiles}\mods\ugc_217416366.mod", TestFiles);

            Assert.IsTrue(et.GameVer == "1.31");
            Assert.IsTrue(!string.IsNullOrEmpty(et.Path));
            Assert.IsTrue(et.Replace.Countries && et.Replace.Provinces && !et.Replace.Bookmarks);


            // Directory for Bellum Orbis Terrarum 3 doesn't exist in TestFiles, so null should be returned
            var bot3 = ModPrep($@"{TestFiles}\mods\lostmc.mod", TestFiles);

            Assert.IsTrue(bot3 is null);
        }

        [TestMethod]
        public void SortBooksOrderTest()
        {
            List<Bookmark> books = new()
            {
                new() { Date = new DateTime(2000, 1, 1), Name = "2000" },
                new() { Date = new DateTime(1900, 1, 1), Name = "1900" }
            };

            var sorted = SortBooks(books);

            Assert.IsTrue(sorted.Count == 2);
            Assert.IsTrue(sorted[1].Name == "2000");
        }

        [TestMethod]
        public void SortBooksDefaultTest()
        {
            List<Bookmark> books = new()
            {
                new() { Date = new DateTime(2000, 1, 1), IsDefault = true, Name = "DefBook" },
                new() { Date = new DateTime(2000, 1, 1), Name = "NotDefBook" }
            };

            var sorted = SortBooks(books);

            Assert.IsTrue(sorted.Count == 1);
            Assert.IsTrue(sorted[0].Name == "DefBook");
        }

        [TestMethod]
        public void SortBooksSameDateTest()
        {
            List<Bookmark> books = new()
            {
                new() { Date = new DateTime(2000, 1, 1), Code = "BOOK_B" },
                new() { Date = new DateTime(2000, 1, 1), Code = "BOOK_A" }
            };

            var sorted = SortBooks(books);

            Assert.IsTrue(sorted.Count == 1);
            Assert.IsTrue(sorted[0].Code == "BOOK_A");
        }

        [TestMethod]
        public void NextProvTest()
        {
            var testProv = new List<Province>
            {
                { new Province(index: 1, color: new P_Color(10, 20, 30), name: "a") },      // dupli with #4
                { new Province(index: 2, color: new P_Color(-1, -1, 100), name: "b") },     // illegal color
                { new Province(index: 3, color: new P_Color(1, 41, 5)) },                   // illegal name
                { new Province(index: 4, color: new P_Color(10, 20, 30), name: "c") },      // dupli with #1
                { new Province(index: 5, color: new P_Color(100, 200, 100), name: "d") },   // dupli with #6
                { new Province(index: 6, color: new P_Color(100, 200, 100), name: "e") },   // dupli with #5
                { new Province(index: 7, color: new P_Color(200, 10, 1), name: "RNW") },    // RNW, dupli with #8
                { new Province(index: 8, color: new P_Color(200, 10, 1), name: "RNW") },    // RNW, dupli with #7
            };
            DupliPrep(testProv);

            var illegalA = SelectNextProv(testProv, 6, false);
            var illegalB = SelectNextProv(testProv, 2, false);

            Assert.IsTrue(illegalA.Index == 2);
            Assert.IsTrue(illegalB.Index == 3);

            var dupliA = SelectNextProv(testProv, 2, true);
            var dupliB = SelectNextProv(testProv, 9, true);

            Assert.IsTrue(dupliA.Index == 4);
            Assert.IsTrue(dupliB.Index == 1);
        }

        [TestMethod]
        public void WriteProvincesTest()
        {
            var desired =
@"province;red;green;blue;x;x
1;130;12;56;Östergötland;x
3;78;32;47;Unused1;
4;23;190;200;UnusedLand1;
5;90;212;231;RNW;
10;1;2;3;Üçkuduk;x
40;10;20;30;def;alt
773;238;42;192;Banda Oriente;x
2000;1;40;100;prov2000;x
4500;;;100;;
";

            var colors = new List<Color>() {
                Color.FromRgb(130, 12, 56),
                Color.FromRgb(1, 40, 100),
                Color.FromRgb(78, 32, 47),
                Color.FromRgb(23, 190, 200),
                Color.FromRgb(90, 212, 231)
            };
            var testProv = new List<Province>() {
                new (index: 1, color: colors[0], name: "Östergötland"),
                new (index: 2000, color: colors[1], name: "prov2000"),
                new (index: 3, color: colors[2], name: "Unused1"),
                new (index: 4, color: colors[3], name: "UnusedLand1"),
                new (index: 5, color: colors[4], name: "RNW"),
                new (index: 773, color: new (238, 42, 192), name: "Banda Oriente"),
                new (index: 4500, color: new (-1, -1, 100), name: ""),
                new (index: 40, color: new(10, 20, 30), name: new("def", alt: "alt")),
                new (index: 10, color: new(1, 2, 3), name: "Üçkuduk")
            };

            var success = WriteProvinces(testProv.OrderBy(p => p.Index), TestFiles + "w_definition.csv");

            Assert.IsTrue(success);

            var text = File.ReadAllText(TestFiles + "w_definition.csv", UTF7);

            Assert.AreEqual(desired, text);
        }

        [TestMethod]
        public void GameVerTest()
        {
            string[] logs =
            {
@"[main.cpp:511]: 
Game Version: EU4 v1.32.0.0 Songhai
Time:2021-11-12 13:21",
                "Game Version: EU4 v1.30.6.0 Austria.rf43rw4t43",
                "Game Version: EU4 v1.31.0 Majapahit."
            };

            Assert.AreEqual("Game - v1.32.0 Songhai", GameVer(logs[0]));
            Assert.AreEqual("Game - v1.30.6 Austria", GameVer(logs[1]));
            Assert.AreEqual("Game - v1.31.0 Majapahit", GameVer(logs[2]));
        }

        [TestMethod]
        public void ProvFileIndexTest()
        {
            Assert.AreEqual(4, ProvFileIndex("4-Bergslagen.txt"));
            Assert.AreEqual(1996, ProvFileIndex("1996  Palau.txt"));
            Assert.IsTrue(ProvFileIndex("") is null);
        }

        [TestMethod()]
        public void ParseMaxProvincesTest()
        {
            string[] maxProvStrings =
            {
                "max_provinces	= 5518",
                " max_provinces=5",
                "# max_provinces=345",
            };

            string[] maxProvValues =
            {
                "5518",
                "5",
                null,
            };

            for (int i = 0; i < maxProvStrings.Length; i++)
            {
                Assert.AreEqual(maxProvValues[i], ParseMaxProvinces(maxProvStrings[i]));
            }
            
        }
    }
}

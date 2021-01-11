using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EU4_PCP_WPF.Tests.MSTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static EU4_PCP_WPF.PCP_Implementations;

namespace EU4_PCP_WPF.Tests.MSTest
{
    [TestClass]
    public class Tests
    {
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

            for (int i = 0; i < dates.Length; i++)
            {
                Assert.AreEqual(owners[i], LastEvent(Resources.Prov_151, EventType.Province, dates[i]));
            }
        }

        [TestMethod]
        public void RandomProvColorTest()
        {
            var testProv = new List<Province>() {
                new Province(color: Color.FromArgb(130, 12, 56)),
                new Province(color: Color.FromArgb(1, 40, 100)),
                new Province(color: Color.FromArgb(78, 32, 47)),
                new Province(color: Color.FromArgb(23, 190, 200)),
                new Province(color: Color.FromArgb(90, 212, 231))
            };

            Color newColor = RandomProvColor(testProv);

            Assert.IsTrue(!testProv.Any(p => p.Color == newColor));

            // Lock tests
            int val1 = 10, val2 = 20;

            newColor = RandomProvColor(testProv, val1); // lock red
            Assert.IsTrue(!testProv.Any(p => p.Color == newColor));
            Assert.IsTrue(newColor.R == val1);

            newColor = RandomProvColor(testProv, val1, val2); // lock red & green
            Assert.IsTrue(!testProv.Any(p => p.Color == newColor));
            Assert.IsTrue(newColor.R == val1);
            Assert.IsTrue(newColor.G == val2);

            newColor = RandomProvColor(testProv, -1, val1, val2); // lock green & blue
            Assert.IsTrue(!testProv.Any(p => p.Color == newColor));
            Assert.IsTrue(newColor.G == val1);
            Assert.IsTrue(newColor.B == val2);
        }

        [TestMethod]
        public void MemberScopeTest()
        {
            // Also tests both C-tors

            string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestFiles\"));

            var gameMembers = new MembersCount[] {
                new MembersCount($@"{path}\gamePath\localisation\aow_l_english.yml", "3", "1"),
                new MembersCount($@"{path}\gamePath\localisation\manchu_l_english.yml", "43", "0"),
                new MembersCount($@"{path}\gamePath\localisation\EU4_l_english.yml", "1", "1"),
                new MembersCount($@"{path}\gamePath\localisation\emperor_map_l_english.yml", "91", "0")
            };

            var modMembers = new MembersCount[] {
                new MembersCount() { Path = $@"modPath\localisation\mod_aow_l_english.yml"},
                new MembersCount() { Path = $@"modPath\localisation\mod_manchu_l_english.yml"}
            };

            foreach (var item in gameMembers)
            {
                item.MemberScope(path);
                Assert.IsTrue(item.Scope == Scope.Game);
            }

            foreach (var item in modMembers)
            {
                item.MemberScope(path);
                Assert.IsTrue(item.Scope == Scope.Mod);
            }
        }
    }
}

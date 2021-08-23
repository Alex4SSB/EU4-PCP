using EU4_PCP.Converters;
using EU4_PCP.Models;
using EU4_PCP.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static EU4_PCP.PCP_Const;
using static EU4_PCP.PCP_Data;
using static EU4_PCP.PCP_Paths;
using static EU4_PCP.PCP_RegEx;

namespace EU4_PCP
{
    public static class PCP_Implementations
    {
        private static string _gamePath = null;

        private static string GameFolder
        {
            get
            {
                if (_gamePath is null)
                {
                    _gamePath = Directory.GetParent(GamePath + LocPath).FullName;
                }
                return _gamePath;
            }
        }

        #region Overrides and Helper Functions

        /// <summary>
        /// Appends an array of strings with a new <see cref="string"/>, growing the array if the last cell isn't empty. <br />
        /// [Emulates the List.Add() method for arrays.]
        /// </summary>
        /// <param name="arr">The array to be modified.</param>
        /// <param name="item">The item to be added.</param>
        public static void Add(ref string[] arr, string item)
        {
            if (!string.IsNullOrEmpty(arr[^1]))
                Array.Resize(ref arr, arr.Length + 1);
            arr[^1] = item;
        }

        /// <summary>
        /// Checks if an integer is in a range.
        /// </summary>
        /// <param name="eval">The number to evaluate.</param>
        /// <param name="lLimit">Lower limit of the range.</param>
        /// <param name="uLimit">Upper limit of the range.</param>
        /// <returns><see langword="true"/> if the number is in range.</returns>
        public static bool Range(this int eval, int lLimit, int uLimit)
        {
            return eval >= lLimit && eval <= uLimit;
        }

        /// <summary>
        /// Checks if a 16-bit integer is in a range.
        /// </summary>
        /// <param name="eval">The number to evaluate.</param>
        /// <param name="lLimit">Lower limit of the range.</param>
        /// <param name="uLimit">Upper limit of the range.</param>
        /// <returns><see langword="true"/> if the number is in range.</returns>
        public static bool Range(this short eval, short lLimit, short uLimit)
        {
            return eval >= lLimit && eval <= uLimit;
        }

        /// <summary>
        /// Converts the <see cref="string"/> representation of a number to its 32-bit signed integer equivalent. 
        /// <br />
        /// [An alias to int.Parse()]
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>A 32-bit signed integer equivalent to the number contained in s.</returns>
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        /// <summary>
        /// Converts a <see cref="string"/> array to <see cref="byte"/> array. <br />
        /// [Backwards compatible with C# 7.3]
        /// </summary>
        /// <param name="s">The <see cref="string"/> array to convert.</param>
        /// <param name="res">The output <see cref="byte"/> array that will receive the result of the conversion.</param>
        /// <param name="startIndex">Starting index in the array</param>
        /// <returns><see langword="true"/> if the conversion was successful.</returns>
        public static bool ToByte(this string[] s, out byte[] res, int startIndex = 0, int length = 3)
        {
            res = new byte[s.Length];
            for (int i = startIndex; i < length + startIndex; i++)
            {
                if (!byte.TryParse(DigitStr(s[i]), out res[i - startIndex]))
                    return false;
            }
            return true;
        }

        public static string DigitStr(string str)
        {
            while (str.Length > 0)
            {
                if (!char.IsDigit(str[^1]))
                    str = str[..^1];
                else
                    break;
            }

            return str;
        }

        /// <summary>
        /// Trims and then compares two numeric strings using > (greater than) operator
        /// </summary>
        /// <param name="s">The left hand string.</param>
        /// <param name="other">The right hand string.</param>
        /// <returns><see langword="true"/> if the right hand string is greater than the left hand string.</returns>
        public static bool Gt(this string s, string other)
        {
            return s.Trim().ToInt() > other.Trim().ToInt();
        }

        /// <summary>
        /// Trims and then compares two numeric strings using >= (greater than or equal to) operator
        /// </summary>
        /// <param name="s">The left hand string.</param>
        /// <param name="other">The right hand string.</param>
        /// <returns></returns>
        public static bool Ge(this string s, string other)
        {
            return s.Trim().ToInt() >= other.Trim().ToInt();
        }

        /// <summary>
        /// Increments the numeric value of a <see cref="string"/> by a given value.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to increment.</param>
        /// <param name="val">The value by which to increment.</param>
        /// <returns>The incremented <see cref="string"/>.</returns>
        public static string Inc(string s, int val)
        {
            int temp = s.ToInt();
            temp += val;
            return temp.ToString();
        }

        public static Visibility Visible(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

        public static void Visible(this FrameworkElement control, bool value) => control.Visibility = Visible(value);

        public static bool Visible(this FrameworkElement control) => control.Visibility == Visibility.Visible;

        public static void ToggleVisibility(this FrameworkElement control) => control.Visible(!control.Visible());

        #endregion

        /// <summary>
        /// Updates the Provinces list with provinces from the selected definition file.
        /// </summary>
        /// <param name="path">Root folder to work on.</param>
        /// <returns><see langword="false"/> if an exception occurs while trying to read from the definition file.</returns>
        public static bool DefinSetup(string path)
        {
            var provList = DefinRead(path + DefinPath, true, !Storage.RetrieveBool(General.ShowIllegalProv));
            if (provList is null)
                return false;
            
            Provinces.Clear();
            Provinces = provList;
            
            return true;
        }

        /// <summary>
        /// Parses one line of a definition file to a <see cref="Province"/>.
        /// </summary>
        /// <param name="definLine">A line from definition.csv</param>
        /// <returns><see cref="Province"/> object if the parsing was successful, <see langword="null"/> otherwise.</returns>
        public static Province DefinParse(string definLine, bool validateColor = true)
        {
            P_Color provColor;
            var list = definLine.Split(';');

            if (int.TryParse(list[0], out int i))
            {
                if (validateColor)
                {
                    if (list[1..].ToByte(out byte[] byteArr))
                        provColor = new(byteArr);
                    else
                        return null;
                }
                else
                {
                    provColor = new(list[1..]);
                }
            }
            else
                return null;

            string provName = null, altName = null;
            if (list.Length >= 5)
            {
                provName = list[4].Trim();
                altName = list.Length > 5 && list[5].Length > 1 ? list[5].Trim() : null;
            }
            if (string.IsNullOrEmpty(provName)) provName = null;

            return new Province(i, new(provName, alt: altName), provColor);
        }

        /// <summary>
        /// Converts the given definition.csv file to a <see cref="Province"/> list.
        /// </summary>
        /// <param name="path">Full path to definition.csv</param>
        /// <param name="parallel"><see langword="false"/> to disable parallelism</param>
        /// <returns><see cref="Province"/> list containing the provinces from the file.</returns>
        public static Dictionary<int, Province> DefinRead(string path, bool parallel = true, bool validateColor = true)
        {
            string[] dFile;
            try
            {
                dFile = File.ReadAllText(path, UTF7).Split(
                    SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception)
            { return null; }

            Dictionary<int, Province> provList = new();
            if (parallel)
            {
                var definLock = new object();
                Parallel.ForEach(dFile, line =>
                {
                    var prov = DefinParse(line, validateColor);
                    if (!prov || provList.ContainsKey(prov.Index)) return;

                    lock (definLock)
                    {
                        provList.Add(prov.Index, prov);
                    }
                });
            }
            else
            {
                foreach (var line in dFile)
                {
                    if (DefinParse(line) is Province prov && !provList.ContainsKey(prov.Index))
                        provList.Add(prov.Index, prov);
                }
            }

            return provList;
        }

        private static bool IsGameDirectory(string path)
        {
            return path.Contains(GameFolder);
        }

        /// <summary>
        /// Sets the owner object of each <see cref="Province"/> object in the 
        /// <see cref="Provinces"/> array according to the start date.
        /// <param name="updateOwner">true to ignore not empty owner.</param>
        /// </summary>
        public static void OwnerSetup(bool updateOwner = false, bool parallel = true)
        {
            if (parallel)
            {
                Parallel.ForEach(ProvFiles, p_file =>
                {
                    var match = ProvFileRE.Match(p_file.File);
                    if (!match.Success) return;
                    int i = match.Value.ToInt();
                    if (!Provinces.ContainsKey(i)) return;

                    var prov = Provinces[i];
                    if (!prov || (!updateOwner && prov.Owner)) return;

                    string provFile = File.ReadAllText(p_file.Path);
                    var currentOwner = LastEvent(provFile, EventType.Province, StartDate);

                    // Order is inverted to make handling of no result from LastEvent easier.
                    // On the other hand, a successful LastEvent result removes the need for the ProvOwnerRE search.
                    if (currentOwner == "")
                    {
                        match = ProvOwnerRE.Match(provFile);
                        if (!match.Success) return;
                        currentOwner = match.Groups["owner"].Value;
                    }

                    if (Countries.Find(c => c.Name == currentOwner) is Country country)
                        prov.Owner = country;
                });
            }
            else
            {
                foreach (var p_file in ProvFiles)
                {
                    var match = ProvFileRE.Match(p_file.File);
                    if (!match.Success) continue;
                    int i = match.Value.ToInt();
                    if (!Provinces.ContainsKey(i)) continue;

                    var prov = Provinces[i];
                    if (!prov || (!updateOwner && prov.Owner)) continue;

                    string provFile = File.ReadAllText(p_file.Path);
                    var currentOwner = LastEvent(provFile, EventType.Province, StartDate);

                    // Order is inverted to make handling of no result from LastEvent easier.
                    // On the other hand, a successful LastEvent result removes the need for the ProvOwnerRE search.
                    if (currentOwner == "")
                    {
                        match = ProvOwnerRE.Match(provFile);
                        if (!match.Success) continue;
                        currentOwner = match.Groups["owner"].Value;
                    }

                    if (Countries.Find(c => c.Name == currentOwner) is Country country)
                        prov.Owner = country;
                }
            }
            
        }

        /// <summary>
        /// Finds the result of the last relevant event in the file 
        /// (last owner for <see cref="Province"/>, last culture for <see cref="Country"/>).
        /// </summary>
        /// <param name="eFile">The file to be searched.</param>
        /// <param name="scope">Province or Country.</param>
        /// <returns>Last owner or last culture.</returns>
        public static string LastEvent(string eFile, EventType scope, DateTime selectedStartDate)
        {
            var lastDate = DateTime.MinValue;
            DateTime currentDate;
            MatchCollection eventMatch;
            Match match;
            var currentResult = "";

            eventMatch = scope switch
            {
                EventType.Province => ProvEventRE.Matches(eFile),
                EventType.Country => CulEventRE.Matches(eFile),
                _ => throw new NotImplementedException(),
            };

            foreach (Match evnt in eventMatch)
            {
                currentDate = DateParser(evnt.Value.Split('=')[0].Trim(), true);
                if (currentDate < lastDate) continue;
                if (currentDate > selectedStartDate) break;

                match = scope switch
                {
                    EventType.Province => DateOwnerRE.Match(evnt.Value),
                    EventType.Country => DateCulRE.Match(evnt.Value),
                    _ => throw new NotImplementedException(),
                };

                if (!match.Success) continue;
                currentResult = match.Groups["value"].Value;
                lastDate = currentDate;
                currentDate = DateTime.MinValue;
            }

            return currentResult;
        }

        /// <summary>
        /// Converts <see cref="string"/> to <see cref="DateTime"/>. Also handles years 2 - 999.
        /// </summary>
        /// <param name="str">The string to be parsed.</param>
        /// <returns>The parsed date as a <see cref="DateTime"/> object in case of successful conversion; <see cref="DateTime.MinValue"/> upon failure.</returns>
        public static DateTime DateParser(string str, bool extended = false)
        {
            if (!DateTime.TryParseExact(extended ? DateBooster(str) : str,
                EUDF, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime pDate))
                return DateTime.MinValue;
            else if (!extended)
                return pDate;

            if (pDate < DateTime.MinValue.AddYears(1000)) pDate = DateTime.MinValue;
            if (pDate != DateTime.MinValue) pDate = pDate.AddYears(-1000);
            return pDate;
        }

        /// <summary>
        /// Adds 1000 years to a date <see cref="string"/>, after performing a simple check.
        /// </summary>
        /// <param name="str">A <see cref="string"/> representing a date.</param>
        /// <returns>The original date + 1000 years if the <see cref="string"/> is in a valid format and the year is a valid <see cref="int"/>.<br />
        /// Otherwise - an empty <see cref="string"/>.
        /// </returns>
        private static string DateBooster(string str)
        {
            var arr = str.Split('.');
            if (arr.Length != 3) return "";

            if (!int.TryParse(arr[0], out int year)) return "";
            return $"{year + 1000}.{arr[1]}.{arr[2]}";
        }

        /// <summary>
        /// Creates the pattern for the multi-bookmark search <see cref="Regex"/>.
        /// </summary>
        /// <returns>The <see cref="Regex"/> pattern.</returns>
        private static string BookPattern()
        {
            return string.Join('|', Bookmarks.Select(b => b.Code));
            //string pattern = "";
            //foreach (var book in Bookmarks)
            //{
            //	pattern += $"{book.Code}|";
            //}
            //return pattern[..^1];
        }

        #region Culture

        /// <summary>
        /// Creates the <see cref="Country"/> objects in the <see cref="Countries"/> list. <br />
        /// Initializes with code and culture object.
        /// </summary>
        public static void CountryCulSetup()
        {
            object countryLock = new();
            Parallel.ForEach(CountryFiles, cFile =>
            {
                var code = "";
                if (RemoveFileExtRE.Match(cFile.File) is Match fileName && fileName.Success)
                {
                    code = fileName.Groups["name"].Value;
                }
                else
                    return;
                
                string countryFile = File.ReadAllText(cFile.Path);
                string priCul = "";
                var match = PriCulRE.Match(countryFile);
                if (!match.Success) return;

                // Order is inverted to make handling of no result from LastEvent easier
                priCul = LastEvent(countryFile, EventType.Country, StartDate);
                if (priCul == "") { priCul = match.Value; }

                if (UpdateCountries)
                {
                    var tempCountry = Countries.Where(c => c.Name == code);
                    var tempCulture = Cultures.Where(cul => cul.Name == priCul);

                    if (tempCountry.Any() && tempCulture.Any())
                        tempCountry.First().Culture = tempCulture.First();
                }
                else
                {
                    var cul = Cultures.Where(cul => cul.Name == priCul);
                    if (!cul.Any()) return;

                    var country = new Country
                    {
                        Name = code,
                        Culture = cul.First()
                    };

                    lock (countryLock)
                    {
                        if (!Countries.Any(c => c.Name == code))
                            Countries.Add(country);
                    }
                }
            });

        }

        private static void CultureSetup(string culFile, object mutex) => CultureSetup(culFile, mutex, Cultures);

        /// <summary>
        /// Creates the <see cref="Culture"/> objects in the <see cref="Cultures"/> list. <br />
        /// Initializes with code and culture group object.
        /// </summary>
        /// <param name="culFile">The culture file to work on.</param>
        /// <param name="mutex">External lock to be provided for adding cultures.</param>
        public static void CultureSetup(string culFile, object mutex, List<Culture> cultureList)
        {
            string text;
            try
            {
                text = File.ReadAllText(culFile, UTF7);
            }
            catch (Exception)
            {
                return;
            }
            var groups = CulGroupsRE.Matches(CulClearRE.Replace(text, ""));

            Parallel.ForEach(groups, group =>
            {
                var cultures = CulSingleRE.Matches(group.Groups["cultures"].Value);
                var groupName = new Culture(group.Groups["group"].Value);
                lock (mutex)
                {
                    cultureList.Add(groupName);
                }

                foreach (Match culture in cultures)
                {
                    Culture newCul = new() { Name = culture.Groups["name"].Value, Group = groupName };
                    lock (mutex)
                    {
                        cultureList.Add(newCul);
                    }
                }
            });
        }

        /// <summary>
        /// Handles the culture setup, along with files preparations.
        /// </summary>
        public static void CulturePrep()
        {
            var cultureLock = new object();
            CulFilePrep();

            // Separate handling because parallel loop seems to work slower for one file.
            if (CultureFiles.Count == 1)
                CultureSetup(CultureFiles[0], cultureLock);
            else if (CultureFiles.Count > 1)
            {
                Parallel.ForEach(CultureFiles, culFile =>
                {
                    CultureSetup(culFile, cultureLock);
                });
            }
        }

        /// <summary>
        /// Gathers all <see cref="Culture"/> files into the <see cref="CultureFiles"/> list.
        /// </summary>
        private static void CulFilePrep()
        {
            if (SelectedMod)
            {
                try
                {
                    CultureFiles.AddRange(Directory.GetFiles(SteamModPath + CulturePath).ToList());
                }
                catch (Exception) { }
            }
            try
            {
                CultureFiles.AddRange(Directory.GetFiles(GamePath + CulturePath).ToList());
            }
            catch (Exception) { }
        }

        #endregion

        /// <summary>
        /// Attaches (the contents of) each file from province names to the relevant <see cref="Country"/> / <see cref="Culture"/>.
        /// </summary>
        public static void ProvNameSetup()
        {
            Parallel.ForEach(ProvNameFiles, (item) =>
            {
                var provName = ProvNamePrep(item);
                if (!provName)
                    return;
                
                ProvNameClass query = Countries.Find(c => c.Name == provName.Name);
                if (!query) query = Cultures.Find(c => c.Name == provName.Name);
                if (query) query.ProvNames = provName.ProvNames;
            });
        }

        public static ProvNameClass ProvNamePrep(FileObj file)
        {
            var name = "";
            if (RemoveFileExtRE.Match(file.File) is Match fileName && fileName.Success)
            {
                name = fileName.Groups["name"].Value;
            }
            else
                return null;

            var fileContent = File.ReadAllText(file.Path, UTF7);
            Dictionary<int, string> provNames = new();
            foreach (Match prov in ProvNamesRE.Matches(fileContent))
            {
                var index = prov.Groups["index"].Value.ToInt();
                if (provNames.ContainsKey(index)) continue;

                provNames.Add(index, prov.Groups["name"].Value);
            }
            
            return new(name, provNames);
        }

        /// <summary>
        /// Finds the correct dynamic name source (owner / culture / group). <br />
        /// Also applies RNW / Unused policy.
        /// </summary>
        public static void DynamicSetup()
        {
            var showIllegal = Storage.RetrieveBool(General.ShowIllegalProv);

            foreach (var prov in Provinces.Values.Where(p => p))
            {
                //prov.Name.Dynamic = "";
                if (!ShowRnw) prov.IsRNW();
                if (!prov.Owner)
                {
                    if (!showIllegal && prov.Name.ToString() is null)
                        prov.Show = false;
                    continue;
                }
                if (DynamicName(prov, NameType.Country)
                    || !prov.Owner.Culture
                    || DynamicName(prov, NameType.Culture))
                    continue;

                if (prov.Owner.Culture.Group)
                    DynamicName(prov, NameType.Group);
            }
        }

        /// <summary>
        /// Selects the owner / culture / culture group for the dynamic name.
        /// </summary>
        /// <param name="prov">The <see cref="Province"/> to select a dynamic name for.</param>
        /// <param name="mode"><see cref="Country"/> / <see cref="Culture"/> / Culture group.</param>
        /// <returns><see langword="true"/> if a name was successfully selected.</returns>
        private static bool DynamicName(Province prov, NameType mode)
        {
            var source = mode switch
            {
                NameType.Country => prov.Owner.ProvNames,
                NameType.Culture => prov.Owner.Culture.ProvNames,
                NameType.Group => prov.Owner.Culture.Group.ProvNames,
                _ => throw new NotImplementedException()
            };

            if (source is null || !source.ContainsKey(prov.Index)) return false;
            prov.Name.Dynamic = source[prov.Index];
            return true;
        }

        /// <summary>
        /// Creates a <see cref="Bookmark"/> object from a bookmark file. <br /> 
        /// Initializes with code and date.
        /// </summary>
        /// <param name="bookFile">Full path of the bookmark file</param>
        /// <returns>A Bookmark object with the correct Code, Date and Default parameters.</returns>
        public static Bookmark BookPrep(string bookFile)
        {
            // Multiple RegEx patterns are used to allow different order of bookmark code, date and default
            string bFile = File.ReadAllText(bookFile);
            var codeMatch = BookmarkCodeRE.Match(bFile);
            var dateMatch = BookmarkDateRE.Match(bFile);
            if (!codeMatch.Success || !dateMatch.Success)
                return null;

            DateTime tempDate = DateParser(dateMatch.Groups["date"].Value, StartDate.Year < 1000);
            if (tempDate == DateTime.MinValue)
                return null;

            return new()
            {
                Code = codeMatch.Groups["name"].Value,
                Date = tempDate,
                IsDefault = BookmarkDefRE.Match(bFile).Groups["default"].Success
            };
        }

        /// <summary>
        /// Populates the <see cref="Bookmarks"/> list.
        /// </summary>
        public static void BookSetup()
        {
            Bookmarks = (from bookFile in BookFiles
                         let book = BookPrep(bookFile.Path)
                         where book
                         select book).ToList();

            if (Bookmarks.Any())
                Bookmarks = SortBooks(Bookmarks);
        }

        /// <summary>
        /// Removes bookmarks of the same date as the default one, and sorts them by date.
        /// </summary>
        private static List<Bookmark> SortBooks(List<Bookmark> bookmarks)
        {
            var sortedBooks = new List<Bookmark>();
            foreach (var item in bookmarks.GroupBy(book => book.Date).OrderBy(books => books.Key))
            {
                if (item.Count() == 1)
                    sortedBooks.Add(item.Single());
                else if (item.Count(b => b.IsDefault) == 1)
                    sortedBooks.Add(item.First(book => book.IsDefault));
            };

            return sortedBooks;
        }

        /// <summary>
        /// Prepares mod defines files.
        /// </summary>
        public static void FetchDefines()
        {
            if (!SelectedMod) return;

            if (!Directory.Exists(SteamModPath + DefinesFolderPath))
                return;

            try
            {
                DefinesFiles = Directory.GetFiles(SteamModPath + DefinesFolderPath, "*.lua").ToList();
            }
            catch (Exception) { }

            var path = SteamModPath + DefinesFilePath;
            if (File.Exists(path))
                DefinesFiles.Add(path);
        }

        /// <summary>
        /// Handles multiple defines files parsing.
        /// </summary>
        public static void DefinesPrep()
        {
            StartDate = DateTime.MinValue;

            if (SelectedMod)
            {
                object mutex = new();

                Parallel.ForEach(DefinesFiles, dFile =>
                {
                    DefinesSetup(dFile, mutex);
                });

                if (StartDate != DateTime.MinValue)
                    return;
            }

            DefinesSetup(GamePath + DefinesFilePath);
        }

        /// <summary>
        /// Parses the start date from a defines file.
        /// </summary>
        /// <param name="path">The defines file to parse.</param>
        private static void DefinesSetup(string path, object mutex = null)
        {
            Match match;
            string fileContent;

            try
            {
                fileContent = File.ReadAllText(path);
            }
            catch (Exception)
            {
                return;
            }

            match = DefinesDateRE.Match(fileContent);
            if (!match.Success)
                return;

            var date = DateParser(match.Value, true);

            if (mutex is null)
            {
                StartDate = date;
            }
            else
            {
                lock (mutex)
                {
                    if (StartDate != DateTime.MinValue)
                        return;

                    StartDate = date;
                }
            }
        }

        /// <summary>
        /// Reads the files from the mod folder, and creates the <see cref="ModObj"/> 
        /// objects in the <see cref="Mods"/> list.
        /// </summary>
        public static void ModSetup(bool parallel = true)
        {
            object modLock = new();
            IEnumerable<string> files;
            try
            {
                files = Directory.GetFiles(ParadoxModPath, "*.mod");
            }
            catch (Exception) { return; }

            var modParentDir = Directory.GetParent(ParadoxModPath).FullName;
            if (parallel)
            {
                Parallel.ForEach(files, modFile =>
                {
                    var mod = ModPrep(modFile, modParentDir);
                    if (!mod) return;

                    // With many mods in the folder, a context switch that will cause an OutOfRange exception is more likely
                    lock (modLock)
                    {
                        Mods.Add(mod);
                    }
                });
            }
            else
            {
                Mods = (from modFile in files
                        let mod = ModPrep(modFile, modParentDir)
                        where mod
                        select mod).ToList();
            }

            Mods.Sort();
        }

        public static ModObj ModPrep(string modFile, string modParentDir)
        {
            // Multiple RegEx patterns are used to allow different order of mod name, path and compatible version

            string mFile = File.ReadAllText(modFile);
            var nameMatch = ModNameRE.Match(mFile);
            var pathMatch = ModPathRE.Match(mFile);
            var verMatch = ModVerRE.Match(mFile);

            if (!(nameMatch.Success && pathMatch.Success && verMatch.Success))
                return null;

            var modPath = ModPathPrep(pathMatch.Groups["path"].Value, modParentDir);
            if (modPath is null)
                return null;

            return new ModObj()
            {
                Name = nameMatch.Groups["name"].Value,
                Path = modPath,
                GameVer = verMatch.Groups["gameVer"].Value,
                Replace = ReplacePrep(mFile)
            };
        }

        private static string ModPathPrep(string scriptedPath, string modParentDir)
        {
            if (!Directory.Exists(scriptedPath))
            {
                var tempPath = $@"{modParentDir}\{scriptedPath.TrimStart('/', '\\')}";
                if (!Directory.Exists(tempPath))
                    return null;

                scriptedPath = tempPath;
            }

            return Path.GetFullPath(scriptedPath);
        }

        /// <summary>
        /// Parses replace folders out of the mod file.
        /// </summary>
        /// <param name="rFile">The mod file to examine.</param>
        /// <returns>Folders to replace as a <see cref="Replace"/> object.</returns>
        private static Replace ReplacePrep(string rFile)
        {
            var vals = ModReplaceRE.Matches(rFile)
                .OfType<Match>()
                .Select(m => m.Groups["replace"].Value);

            if (!vals.Any()) return new Replace();

            return new Replace
            {
                Cultures = vals.Contains(CulturesRep),
                Bookmarks = vals.Contains(BookmarksRep),
                ProvNames = vals.Contains(ProvNamesRep),
                Countries = vals.Contains(CountriesRep),
                Provinces = vals.Contains(ProvincesRep),
                Localisation = vals.Contains(LocalisationRep)
            };
        }

        /// <summary>
        /// Selects mod path if a mod is selected, or game path if not.
        /// </summary>
        /// <returns>SteamModPath or GamePath.</returns>
        public static string DecidePath()
        {
            return SelectedMod ? SteamModPath : GamePath;
        }

        /// <summary>
        /// All array clearing.
        /// </summary>
        public static void ClearArrays()
        {
            CultureFiles.Clear();
            DefinesFiles.Clear();
            CountryFiles.Clear();
            Countries.Clear();
            Cultures.Clear();
            BookFiles.Clear();
            Bookmarks.Clear();
            ProvFiles.Clear();
            ProvNameFiles.Clear();
        }

        /// <summary>
        /// Checks if the start date is greater than 01/01/0001, otherwise prompts.
        /// </summary>
        /// <returns><see langword="true"/> if the date is valid.</returns>
        public static bool ValDate()
        {
            return StartDate > DateTime.MinValue || ErrorMsg(ErrorType.ValDate);
        }

        /// <summary>
        /// Checks game validity.
        /// </summary>
        /// <returns><see langword="false"/> upon failure or if auto-loading is disabled.</returns>
        public static bool ValGame()
        {
            string tempGamePath = Storage.RetrieveValue(General.GamePath);

            if (Storage.RetrieveBoolEnum(AutoLoad.Disable) ||
                string.IsNullOrEmpty(tempGamePath) ||
                !PathHandler(Scope.Game)) return false;
            GamePath = tempGamePath;

            return true;
        }

        /// <summary>
        /// Handles path validation for game and mod.
        /// </summary>
        /// <param name="scope">Game / Mod.</param>
        /// <returns><see langword="true"/> if the validation was successful.</returns>
        public static bool PathHandler(Scope scope) => PathHandler(scope, PathRead(scope));

        /// <summary>
        /// Handles path validation for game and mod.
        /// </summary>
        /// <param name="scope">Game / Mod.</param>
        /// <param name="setting">The setting from which to read.</param>
        /// <returns><see langword="true"/> if the validation was successful.</returns>
        private static bool PathHandler(Scope scope, string setting)
        {
            if (setting.Contains('|')) // Legacy support
                setting = setting.Split('|')[0];

            switch (scope)
            {
                case Scope.Game when !File.Exists(setting + GameFile):
                    return ErrorMsg(ErrorType.GameExe);
                case Scope.Mod:
                    if (string.IsNullOrEmpty(setting))
                    {
                        var path = SelectedDocsPath + ModPath;
                        if (Directory.Exists(path))
                        {
                            ParadoxModPath =
                            setting = path;
                        }
                        else return false;
                    }
                    else
                        ParadoxModPath = setting;
                    break;
                default:
                    break;
            }
            PathWrite(scope, setting);

            return true;
        }

        /// <summary>
        /// Decide whether to update the bookmarks - if a bookmark is selected.
        /// <br /> <br />
        /// The function is used to prevent updating the bookmarks CB when a bookmark is selected,
        /// thus preventing the start date from changing, which in turn prevents useless
        /// activation of some sequences again.
        /// <br />
        /// Optionally combined with enabled bookmarks check, to disable some sequences
        /// when the bookmarks are disabled.
        /// </summary>
        /// <param name="enabled"><see langword="true"/> to ignore bookmarks being enabled or disabled 
        /// (tied to dynamic enabled).</param>
        /// <returns><see langword="true"/> if Bookmarks should be updated.</returns>
        public static bool BookStatus(bool enabled)
        {
            return (enabled || Storage.RetrieveBoolEnum(ProvinceNames.Dynamic)) &&
                SelectedBookmarkIndex < 1;
        }

        /// <summary>
        /// Reads path from game / mod settings.
        /// </summary>
        /// <param name="scope">Game / Mod.</param>
        /// <returns>The path as string.</returns>
        private static string PathRead(Scope scope) 
            => Storage.RetrieveValue(Enum.GetName(typeof(Scope), scope) + "Path") is string str 
            ? str 
            : "";

        /// <summary>
        /// Writes path to game / mod settings.
        /// </summary>
        /// <param name="scope">Game / Mod.</param>
        /// <param name="path">The path as string.</param>
        private static void PathWrite(Scope scope, string path)
        {
            Storage.StoreValue(path, Enum.GetName(typeof(Scope), scope) + "Path");
        }

        /// <summary>
        /// Reads game version from the game logs.
        /// </summary>
        public static void GameVer()
        {
            var gameVer = "Game";
            var logText = "";

            try
            {
                logText = File.ReadAllText(SelectedDocsPath + GameLogPath, UTF8);
            }
            catch (Exception) { }

            if (GameVerRE.Match(logText) is Match match && match.Success)
                gameVer += $" - {match.Value}";

            GameVersion = gameVer;
        }

        /// <summary>
        /// A smart count of overall Provinces.
        /// </summary>
        /// <param name="scope"></param>
        public static void CountProv(Scope scope)
        {
            var provCount = Provinces.Values.Count(p => p && p.IsNameLegal() && p.Color.IsLegal()).ToString();
            var illegalCount = "";

            ShowIllegal = Storage.RetrieveBool(General.ShowIllegalProv);
            if (ShowIllegal)
                illegalCount = Provinces.Values.Count(p => p && !(p.IsNameLegal() && p.Color.IsLegal())).ToString();

            switch (scope)
            {
                case Scope.Game:
                    GameProvinceCount = provCount;
                    ModProvinceCount =
                    ModIllegalProvinceCount = "";
                    break;
                case Scope.Mod:
                    ModProvinceCount = provCount;
                    ModIllegalProvinceCount = illegalCount;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update the bookmark list with all relevant bookmarks.
        /// </summary>
        public static void PopulateBooks()
        {
            if (!BookStatus(false)) return;

            if (!Bookmarks.Any(b => b.Code != null))
            {
                BookmarkList = null;
                return;
            }

            BookmarkList = Bookmarks.Select(b => new ListBookmark(b)).ToList();

            SelectedBookmarkIndex = 0;
        }

        /// <summary>
        /// Default file max provinces.
        /// </summary>
        /// <param name="scope">Game / Mod.</param>
        /// <returns><see langword="false"/> upon failure.</returns>
        public static bool MaxProvinces(Scope scope)
        {
            var filePath = (scope == Scope.Mod) ? SteamModPath : GamePath;

            string d_file;
            try
            {
                d_file = File.ReadAllText(filePath + DefMapPath, UTF8);
            }
            catch (Exception)
            {
                return ErrorMsg(ErrorType.DefMapRead);
            }
            var match = MaxProvRE.Match(d_file);

            if (!match.Success)
            {
                switch (scope)
                {
                    case Scope.Game:
                        GameMaxProvinces = "";
                        break;
                    case Scope.Mod:
                        ModMaxProvinces = "";
                        SelectedModIndex = -1;
                        break;
                    default:
                        break;
                }
                return ErrorMsg(ErrorType.DefMapMaxProv);
            }

            switch (scope)
            {
                case Scope.Game:
                    GameMaxProvinces = match.Value;
                    ModMaxProvinces = "";
                    break;
                case Scope.Mod:
                    ModMaxProvinces = match.Value;
                    break;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Combines the game and mod bookmark CB index changed handlers.
        /// </summary>
        public static void EnactBook()
        {
            if (Lockdown) return;
            if (SelectedBookmarkIndex < 0)
            {
                if (BookmarkList.Any())
                    SelectedBookmarkIndex = 0;
                else return;
            }

            StartDate = Bookmarks[SelectedBookmarkIndex].Date;
            StartDateStr = CurrentDateFormat(true, StartDate);

            ShowRnw = Storage.RetrieveBool(General.ShowAllProvinces);
            UpdateCountries = true;
            CountryCulSetup();
            OwnerSetup(true);
            ProvNameSetup();
            DynamicSetup();
        }

        public static string CurrentDateFormat(bool upperCase = false, DateTime? date = null, bool useDefaultFormat = false)
        {
            int index = 0;
            if (!useDefaultFormat)
            {
                index = Storage.RetrieveValue(General.DateFormat) is string strIndex
                ? strIndex.ToInt()
                : (int)General.DateFormat.ToString().GetDefault();
            }

            var format = DATE_FORMATS[index];
            var outString = date is DateTime dateTime
                ? dateTime.ToString(format, CultureInfo.CreateSpecificCulture("en-US"))
                : format;

            return upperCase switch
            {
                true => outString.ToUpper(),
                false => outString
            };
        }

        /// <summary>
        /// Updates the definition.csv file by writing all current provinces.
        /// </summary>
        /// <returns><see langword="false"/> if the operation was not successful.</returns>
        public static bool WriteProvinces()
        {
            string stream = "province;red;green;blue;x;x\r\n";

            var csvProvs = from prov in Provinces
                           orderby prov.Key
                           select prov.Value.ToCsv();

            stream += string.Join("\r\n", csvProvs) + "\r\n";

            try
            {
                File.WriteAllBytes(SteamModPath + DefinPath, stream.Select(c => (byte)c).ToArray());
            }
            catch (Exception)
            {
                return ErrorMsg(ErrorType.DefinWrite);
            }

            return true;
        }

        /// <summary>
        /// Updates the default.map file.
        /// </summary>
        /// <param name="newMax">The new max_provinces value</param>
        /// <returns></returns>
        public static bool WriteDefines(string newMax)
        {
            string defMap;
            try
            {
                defMap = File.ReadAllText(SteamModPath + DefMapPath);
            }
            catch (Exception)
            {
                return ErrorMsg(ErrorType.DefMapRead);
            }
            defMap = DefMapRE.Replace(defMap, $"max_provinces = {newMax}");
            try
            {
                File.WriteAllText(SteamModPath + DefMapPath, defMap, UTF8);
            }
            catch (Exception)
            {
                return ErrorMsg(ErrorType.DefMapWrite);
            }

            return true;
        }

        /// <summary>
        /// The default overload to call the function with <see cref="Provinces"/>
        /// </summary>
        public static void DupliPrep()
        {
            foreach (var prov in Provinces)
            {
                prov.Value.NextDupli = null;
            }

            if (!CheckDupli || !SelectedMod) return;

            ModDupliProvinceCount = DupliPrep(Provinces.Values.ToList()).ToString();
        }

        /// <summary>
        /// Handles duplicate provinces - sets for each province its next duplicate.
        /// </summary>
        public static int DupliPrep(List<Province> provinces)
        {
            var dupliGroups = (from prov in provinces
                               where prov && prov.Show && prov.Color.IsLegal()
                               group prov by (Color)prov.Color)
                              .Where(g => g.Count() > 1);

            foreach (var group in dupliGroups)
            {
                for (int i = 0; i < group.Count(); i++)
                {
                    var nextIndex = (i == group.Count() - 1) ? 0 : i + 1;

                    group.ElementAt(i).NextDupli = group.ElementAt(nextIndex);
                }
            }

            return dupliGroups.Count();
        }

        /// <summary>
        /// Marker preparation logic.<br/>
        /// Assigns the relevant background color, and calculates the relative position of province markers in the table.
        /// </summary>
        /// <param name="provinces">The list of provinces from which to search for provinces that need to be marked.</param>
        /// <param name="dupliEnabled">Whether duplicates are enabled in settings.</param>
        /// <param name="illegalEnabled">Whether illegal provinces are enabled in settings.</param>
        /// <returns>A list of tuples that include all the information needed to create each marker.</returns>
        public static List<Tuple<Province, SolidColorBrush, double>> MarkerPrep(IEnumerable<Province> provinces, bool dupliEnabled, bool illegalEnabled)
        {

            // PROVS THAT ARE BOTH DUPLICATE AND ILLEGAL ARE NOT SUPPORTED (YET)

            var markers = new List<Tuple<Province, SolidColorBrush, double>>();
            if (!dupliEnabled && !illegalEnabled) return markers;

            var shownProvs = provinces.Where(p => p && p.Show).OrderBy(p => p.Index).ToList();

            foreach (var prov in provinces.Where(prov => prov.IsDupli(dupliEnabled) || prov.IsIllegal(illegalEnabled)))
            {
                int index = shownProvs.IndexOf(prov);
                if (index < 0) continue;

                var fill = new SolidColorBrush(prov.NextDupli ? RedBackground : PurpleBackground);
                double ratio = index / (double)shownProvs.Count;

                markers.Add(new(prov, fill, ratio));
            }

            return markers;
        }

        /// <summary>
        /// Adds the given <see cref="Province"/> to the list if it doesn't exist. Otherwise, updates its color and definition name.
        /// </summary>
        /// <param name="newProv">The province to add / update</param>
        /// <returns></returns>
        public static bool AddProv(Province newProv)
        {
            var update = !(ChosenProv && ChosenProv.province.Color.IsLegal() && ChosenProv.IsNameLegal());

            if (Provinces.ContainsKey(newProv.Index))
            {
                var prov = Provinces[newProv.Index];
                prov.Color = newProv.Color;
                prov.Name.Definition = newProv.Name.Definition;
            }
            else
            {
                newProv.IsRNW();
                Provinces.Add(newProv.Index, newProv);
            }

            if (update)
            {
                ModMaxProvinces = Storage.RetrieveBool(General.IterateMaxProv)
                    ? Inc(ModMaxProvinces, 1)
                    : Inc(ModProvinceCount, 2);

                if (Storage.RetrieveBool(General.UpdateMaxProv) && !WriteDefines(ModMaxProvinces))
                    return false;

                ProvincesShown = Provinces.Values.Count(prov => prov && prov.Show).ToString();
            }

            if (!WriteProvinces())
                return false;

            ChosenProv = null;

            return true;
        }

        /// <summary>
        /// Generates an exclusive random <see cref="Color"/>, that doesn't exist in the given <see cref="Province"/> array.
        /// </summary>
        /// <param name="provList">The <see cref="Province"/> array to be searched.</param>
        /// <returns>The generated <see cref="Color"/>.</returns>
        public static Color RandomProvColor(Dictionary<int, Province> provList, int red = -1, int green = -1, int blue = -1)
        {
            var rnd = new Random();
            byte r, g, b;
            var tempColor = new Color();
            if (red >= 0 && green >= 0 && blue >= 0)
                return tempColor;

            do
            {
                r = (byte)(red < 0 ? rnd.Next(0, 255) : red);
                g = (byte)(green < 0 ? rnd.Next(0, 255) : green);
                b = (byte)(blue < 0 ? rnd.Next(0, 255) : blue);
                tempColor = Color.FromRgb(r, g, b);
            } while (provList.Values.Any(p => p && p.Color == tempColor));

            return tempColor;
        }

        /// <summary>
        /// Selects a background color depending on color value.
        /// </summary>
        /// <param name="pickedColor">The color to evaluate</param>
        /// <param name="chosenProv">The province to compare with the picked color</param>
        /// <returns>Green background color for a color that doesn't exist in the provinces list, except for the chosen province. Red otherwise.</returns>
        public static SolidColorBrush SelectBG(P_Color pickedColor, Province chosenProv = null)
            => new(ColorExist(pickedColor, Provinces, chosenProv)
                ? RedBackground
                : GreenBackground);

        /// <summary>
        /// Determines whether a color exists in a province list.
        /// </summary>
        /// <param name="pickedColor">The color to evaluate</param>
        /// <param name="provList">The list of provinces to search</param>
        /// <param name="chosenProv">The province to compare with the picked color</param>
        /// <returns><see langword="false"/> for a color that doesn't exist in the given list, except for the chosen province. <see langword="true"/> otherwise.</returns>
        public static bool ColorExist(P_Color pickedColor, Dictionary<int, Province> provList, Province chosenProv = null) => 
            provList.Values.Count(prov => prov.Color.Equals(pickedColor)) switch
        {
            < 1 => false,
            < 2 when chosenProv && chosenProv.Show && chosenProv.Color.Equals(pickedColor) => false,
            _ => true
        };

        /// <summary>
        /// Selects a color depending on color channel value.
        /// </summary>
        /// <param name="channel">Color channel value to evaluate</param>
        /// <returns>Red background color for a value smaller than 0, green otherwise.</returns>
        public static SolidColorBrush LegalBG(short channel) =>
            new(channel < 0 ? RedBackground : GreenBackground);

        public static bool LocalisationSetup(bool enBooks)
        {
            var scope = Scope.Game;
            string path = GamePath;
            string[] dirs = new string[1];

            if (SelectedMod)
            {
                scope = Scope.Mod;

                if (!SelectedMod.Replace.Localisation)
                {
                    Array.Resize(ref dirs, 2);
                    dirs[1] = path + LocPath;
                }

                path = SteamModPath;
            }

            dirs[0] = path + LocPath;
            var indexers = PathIndexer(PathCombiner(dirs), scope, enBooks);

            if (indexers is null) return false;

            ReadProvLoc(indexers);
            if (enBooks)
                ReadBookLoc(indexers);

            return true;
        }

        private static List<string> PathCombiner(params string[] dirs)
        {
            List<string> files;
            try
            {
                files = dirs.Where(d => Directory.Exists(d)).SelectMany(d => Directory.GetFiles(d, "*", SearchOption.AllDirectories).Where(f => LocFileRE.Match(f).Success)).ToList();
            }
            catch (Exception)
            {
                return null;
            }


            if (dirs.Length > 1 && files.Any(d => d.Contains(RepLocPath)))
            {
                var replace = files.Where(f => f.Contains(RepLocPath)).ToDictionary(f => Path.GetFileName(f), f => f);

                files.RemoveAll(f => replace.Keys.Contains(Path.GetFileName(f)) && !replace.Values.Contains(f));
            }
            
            return files;
        }

        private static List<Indexer> PathIndexer(List<string> files, Scope scope, bool enBooks)
        {
            if (files is null) return null;

            var source = IndexerSource(scope);
            string storageName = source + LocIndexer;
            List<Indexer> current;

            try
            {
                current = files.Select(f => new Indexer(f, File.GetLastWriteTime(f), IsGameDirectory(f) ? "Game" : source)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            if (!Storage.RetrieveBool(General.InC))
            {
                CacheLoc(ref current, enBooks);
                return current;
            }

            var previous = Storage.RetrieveIndexer(storageName);
            
            if (previous is null)
            {
                CacheLoc(ref current, enBooks);
            }
            else
            {
                var modified = current.Where(i =>
                    !previous.Exists(p => p.Path == i.Path)
                    || previous.Find(p => p.Path == i.Path)?.LastModified.CompareTo(i.LastModified) != 0).ToList();

                if (modified.Any())
                    CacheLoc(ref modified, enBooks);

                foreach (var item in previous)
                {
                    if (modified.Any(i => i.Path == item.Path)) continue;

                    var curr = current.Find(i => i.Path == item.Path);
                    if (curr is null) continue;

                    curr.ProvDict = item.ProvDict;
                    curr.BookDict = item.BookDict;
                }
            }

            Storage.StoreValue(current, storageName);
            return current;
        }

        private static string IndexerSource(Scope scope) => scope switch
        {
            Scope.Game => scope.ToString(),
            _ => SelectedMod.Name
        };

        private static void CacheLoc(ref List<Indexer> indexList, bool enBooks)
        {
            Regex bookRE = null;
            if (enBooks)
                bookRE = new Regex($@"^ *(?<code>{BookPattern()}):\d* *""(?<name>.+?)""", RegexOptions.Multiline);

            Parallel.ForEach(indexList, item =>
            {
                string text = File.ReadAllText(item.Path);
                try
                {
                    text = File.ReadAllText(item.Path);
                }
                catch (Exception)
                {
                    return;
                }

                var provMatches = LocProvsRE.Matches(text);
                
                item.ProvDict.Clear();
                ProvLocDict(provMatches, ref item.ProvDict);

                if (enBooks)
                {
                    var bookMatches = bookRE.Matches(text);
                    item.BookDict.Clear();
                    BookLocDict(bookMatches, ref item.BookDict);
                }
            });
        }

        private static void ProvLocDict(MatchCollection collection, ref Dictionary<int, string> dict)
        {
            foreach (Match match in collection)
            {
                var id = match.Groups["index"].Value.ToInt();
                var name = match.Groups["name"].Value;

                if (dict.ContainsKey(id)) continue;

                dict.Add(id, name);
            }
        }

        private static void ReadProvLoc(List<Indexer> indexers)
        {
            foreach (var item in indexers)
            {
                foreach (var prov in item.ProvDict)
                {
                    if (Provinces.ContainsKey(prov.Key)
                        && (string.IsNullOrEmpty(Provinces[prov.Key].Name.Localisation) || item.Source != "Game"))
                        Provinces[prov.Key].Name.Localisation = prov.Value;
                }
            }
        }

        private static void BookLocDict(MatchCollection collection, ref Dictionary<string, string> dict)
        {
            foreach (Match match in collection)
            {
                var code = match.Groups["code"].Value;
                var name = match.Groups["name"].Value;

                if (dict.ContainsKey(code)) continue;

                dict.Add(code, name);
            }
        }

        private static void ReadBookLoc(List<Indexer> indexers)
        {
            foreach (var item in indexers)
            {
                foreach (var bookLoc in item.BookDict)
                {
                    if (Bookmarks.Find(b => b.Code == bookLoc.Key) is Bookmark bookObj
                        && (string.IsNullOrEmpty(bookObj.Name) || item.Source != "Game"))
                        bookObj.Name = bookLoc.Value;
                }
            }
            
        }

        public static void ClearCache()
        {
            var keys = App.Current.Properties.Keys.Cast<string>().ToList();

            foreach (var item in keys)
            {
                if (LEGACY_CACHE.Contains(item) || item.Contains("Loc Indexer"))
                    App.Current.Properties.Remove(item);
            }
        }

        #region File Fetching

        /// <summary>
        /// General function for fetching files from folders. <br />
        /// Handles game, regular mod, and replace mod folders.
        /// </summary>
        /// <param name="scope">The type of the files to process.</param>
        /// <returns><see langword="false"/> if an exception occurred and the processing was unsuccessful.</returns>
        public static bool FetchFiles(FileType scope)
        {
            var filesList = SelectList(scope);
            IEnumerable<string> baseFiles, addFiles;

            if (scope == FileType.Bookmark)
                AreBooksOverridden = false;

            try
            {
                baseFiles = Directory.GetFiles(PathRep(scope));
            }
            catch (Exception) { return false; }

            if (!SelectedMod || SelectReplace(scope))
            {
                filesList.AddRange(baseFiles.Select(f => new FileObj(f)));
                return true;
            }

            try
            {
                addFiles = Directory.GetFiles(SteamModPath + SelectFolder(scope), "*", SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                // If no mod files were found - just use the game files (except bookmarks, unless overridden)

                if (scope == FileType.Bookmark)
                {
                    if (!Storage.RetrieveBool(General.OverrideBooks))
                        return false;

                    AreBooksOverridden = true;
                }

                filesList.AddRange(baseFiles.Select(f => new FileObj(f)));

                return false;
            }

            filesList.AddRange(addFiles.Select(f => new FileObj(f)));

            foreach (var b_file in baseFiles)
            {
                var TempFile = new FileObj(b_file);
                if (ReplacedFile(scope, filesList.Where(c => c == TempFile))) { continue; }
                filesList.Add(TempFile);
            }

            return true;
        }

        /// <summary>
        /// Selects the path for the base files - game, or mod if relevant folder is to be replaced.
        /// </summary>
        /// <param name="scope">The type of the files to process.</param>
        /// <returns>Path for base files.</returns>
        public static string PathRep(FileType scope)
        {
            string path;
            if (SelectedMod && SelectReplace(scope))
                path = SteamModPath;
            else path = GamePath;

            return path + SelectFolder(scope);
        }

        /// <summary>
        /// Folder replacement selector. <br />
        /// Can be called only when <see cref="SelectedMod"/> is not <see langword="null"/>.
        /// </summary>
        /// <param name="scope">The type of the files to process.</param>
        /// <returns><see langword="true"/> if the current scope requires replacement.</returns>
        private static bool SelectReplace(FileType scope)
        {
            var rep = SelectedMod.Replace;
            switch (scope)
            {
                case FileType.Country when rep.Countries:
                case FileType.Bookmark when rep.Bookmarks:
                case FileType.Province when rep.Provinces:
                case FileType.ProvName when rep.ProvNames:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Converts scope to relevant path.
        /// </summary>
        /// <param name="scope">The type of the files to process.</param>
        /// <returns>The path corresponding to the scope.</returns>
        private static string SelectFolder(FileType scope) => scope switch
        {
            FileType.Country => HistCountryPath,
            FileType.Bookmark => BookmarksPath,
            FileType.Province => HistProvPath,
            FileType.ProvName => ProvNamesPath,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Converts scope to relevant list.
        /// </summary>
        /// <param name="scope">The type of the files to process.</param>
        /// <returns>The files list corresponding to the scope.</returns>
        private static List<FileObj> SelectList(FileType scope) => scope switch
        {
            FileType.Country => CountryFiles,
            FileType.Bookmark => BookFiles,
            FileType.Province => ProvFiles,
            FileType.ProvName => ProvNameFiles,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Checks if the query contains province name files.
        /// </summary>
        /// <param name="scope">The type of the files to process.</param>
        /// <param name="query">The file to process.</param>
        /// <returns><see langword="true"/> for positive file count of: 
        /// country, bookmark, province. 
        /// <see langword="false"/> otherwise, and always for ProvName.</returns>
        private static bool ReplacedFile(FileType scope, IEnumerable<FileObj> query)
        {
            return scope switch
            {
                FileType.Country or FileType.Bookmark or FileType.Province when query.Any() => true,
                FileType.ProvName => false,
                _ => false,
            };
        }

        #endregion

        /// <summary>
        /// Handles all error messages. Returns a value to be used later.
        /// </summary>
        /// <param name="type">The error type</param>
        /// <param name="returnVal">The boolean value to return</param>
        /// <returns>returnVal</returns>
        public static bool ErrorMsg(ErrorType type, bool returnVal = false)
        {
            Error_Msg(type);
            return returnVal;
        }

        /// <summary>
        /// Handles all error messages.
        /// </summary>
        /// <param name="type">The error type</param>
        private static void Error_Msg(ErrorType type) => _ = (type switch
        {
            ErrorType.DefinRead => MessageBox.Show("The definition.csv file is missing or corrupt",
                "Unable to parse definition file", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.DefinWrite => MessageBox.Show("The definition.csv file is inaccessible for writing",
                "Unable to access definition file", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.DefMapRead => MessageBox.Show("The default.map file is missing or corrupt",
                "Unable to parse default file", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.DefMapWrite => MessageBox.Show("The default.map file is inaccessible for writing",
                "Unable to access default file", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.DefMapMaxProv => MessageBox.Show("The 'default.map' file has no max_provinces definition!",
                "Missing max_provinces", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.LocRead => MessageBox.Show("Localisation folder and its content is missing or inaccessible",
                "Error reading localisation files", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.HistoryProvFolder => MessageBox.Show($"The {HistProvPath} folder is missing or inaccessible",
                "Unable to access province history files", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.ValDate => MessageBox.Show("At least one bookmark, or a defines entry are required to determine the start date",
                "Unable to determine start date", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.GameExe => MessageBox.Show("Cannot find the game executable!",
                "Missing EXE", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.NoCultures => MessageBox.Show("Unable to find any cultures in the culture file(s), or the file(s) / folder are inaccessible",
                "Missing cultures", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.NoCulGroups => MessageBox.Show("Unable to find any culture groups in the culture file(s)",
                "No culture groups", MessageBoxButton.OK, MessageBoxImage.Error),
            ErrorType.NoCountries => MessageBox.Show("Unable to find the countries folder, or the files are inaccessible",
                "Missing countries", MessageBoxButton.OK, MessageBoxImage.Error),
            _ => throw new NotImplementedException(),
        });
    }
}

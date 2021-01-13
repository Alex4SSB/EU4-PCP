using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static EU4_PCP_WPF.PCP_Const;
using static EU4_PCP_WPF.PCP_Data;
using static EU4_PCP_WPF.PCP_Paths;
using static EU4_PCP_WPF.PCP_RegEx;

namespace EU4_PCP_WPF
{
    public static class PCP_Implementations
	{
		#region Overrides and Helper Functions

		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string ToCsv(this Color color)
        {
			return $"{color.R};{color.G};{color.B}";
		}

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
		/// Converts a <see cref="string"/> array to <see cref="byte"/> array.
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
				if (!byte.TryParse(s[i], out res[i - startIndex]))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Converts a <see cref="string"/> array to <see cref="byte"/> array.
		/// </summary>
		/// <param name="s">The <see cref="string"/> array to convert.</param>
		/// <returns><see cref="byte"/> array that contains the result of the conversion.</returns>
		public static byte[] ToByte(this string[] s)
		{
			var res = new byte[s.Length];
			for (int i = 0; i < s.Length; i++)
			{
				res[i] = byte.Parse(s[i]);
			}
			return res;
		}

		public static Color ToColor(this byte[] color)
        {
			return Color.FromArgb(color[0], color[1], color[2]);
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

		/// <summary>
		/// Converts LocScope to FileType.
		/// </summary>
		/// <param name="scope"><see cref="LocScope"/> enum.</param>
		/// <returns><see cref="FileType"/> enum.</returns>
		public static FileType FromLoc(LocScope scope) => scope switch
		{
			LocScope.Province => FileType.Province,
			LocScope.Bookmark => FileType.Bookmark,
			_ => throw new NotImplementedException()
		};

		/// <summary>
		/// [Default overload for MemberScope] <br />
		/// Determines member scope by whether it contains the game path.
		/// </summary>
		/// <param name="member">The member of which to determine the scope.</param>
		public static void MemberScope(this MembersCount member) => MemberScope(member, GamePath);

		/// <summary>
		/// Determines member scope by whether it contains the provided path.
		/// </summary>
		/// <param name="member">The member of which to determine the scope.</param>
		public static void MemberScope(this MembersCount member, string path)
		{
			member.Scope = 
				member.Path.Contains(Directory.GetParent(path + LocPath).FullName) ? 
				Scope.Game : Scope.Mod;
		}

		#endregion

		/// <summary>
		/// Creates the <see cref="Province"/> objects in the <see cref="Provinces"/> array. <br />
		/// Initializes with index, color, and name from definition file.
		/// </summary>
		/// <param name="path">Root folder to work on.</param>
		/// <returns><see langword="false"/> if an exception occurs while trying to read from the definition file.</returns>
		public static bool DefinSetup(string path)
		{
			var definLock = new object();
			string[] dFile;
			try
			{
				dFile = File.ReadAllText(path + DefinPath, UTF7).Split(
					SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
			}
			catch (Exception)
			{ return false; }

			Provinces.Clear();
			Provinces.Add(new Province());

            Parallel.ForEach(dFile, p =>
            {
                string[] list = p.Split(';');
                int i = -1;
                byte[] provColor = new byte[3];
                if (!(int.TryParse(list[0], out i) &&
                    list[1..].ToByte(out provColor)))
                    return;

                Province prov = new Province
				(
					index: i,
					color: provColor.ToColor(),
					name: list[4].Trim()
				);

                lock (definLock)
                {
					Provinces.Add(prov);
                    //Add(ref Provinces, prov);
                }
            });
			Provinces.Sort();
            return true;
		}

		/// <summary>
		/// Dynamically prepares the localisation files.
		/// </summary>
		/// <param name="scope">Province or Bookmark.</param>
		public static bool LocPrep(LocScope scope)
		{
			bool readSuccess;

			var setting = "";
			//string setting = scope switch
			//{
			//	LocScope.Province => Settings.Default.ProvLocFiles,
			//	LocScope.Bookmark => Settings.Default.BookLocFiles,
			//	_ => "",
			//};

			if (setting.Length > 0)
			{
				LocMembers(Mode.Read, scope);
				List<FileObj> filesList = new List<FileObj>();
				foreach (var member in Members.Where(m => m.Type == scope))
				{
					if (!SelectedMod && member.Scope == Scope.Mod)
						continue;
					filesList.Add(new FileObj(member.Path, FromLoc(scope)));
				}

				bool abort = false;
				for (int i = 0; i < filesList.Count; i++)
				{
					var lFile = filesList[i];
					var memberFiles = LocFiles.Where(f => f == lFile);
					if (memberFiles?.Any() == false || (SelectedMod &&
						!lFile.Path.Contains(Directory.GetParent(SteamModPath + LocPath).FullName)))
					{
						abort = true;
						break;
					}

					filesList[i] = memberFiles.First(); // For mod replacement files
					LocFiles.Remove(memberFiles.First());
				}

				// If members were recovered successfully 
				if ((!abort && filesList.Any()) && NameSetup(filesList, scope, out readSuccess))
					return readSuccess;
			}

			// If there are no members in the settings, or the members have changed
			NameSetup(LocFiles, scope, out readSuccess);
			if (readSuccess)
				LocMembers(Mode.Write, scope);

			return readSuccess;
		}

		/// <summary>
		/// Finds localisation names for Provinces and Bookmarks.
		/// </summary>
		/// <param name="lFiles">List of localisation files to work on.</param>
		/// <param name="scope">Province or Bookmark.</param>
		/// <param name="readSuccess"><see langword="false"/> if there was an error reading one of the files.</param>
		/// <returns><see langword="false"/> if the member count of the game was NOT according to the settings.</returns>
		private static bool NameSetup(List<FileObj> lFiles, LocScope scope, out bool readSuccess)
		{
			bool success = true;
			bool recall = Members.Any(m => m.Type == scope);
			bool locSuccess = true;
			var nameLock = new object();

			Regex locRE = scope switch // Select province or bookmark RegEx
			{
				LocScope.Province => LocProvRE,

				// bookRE is a dynamic RegEx so it is local.
				LocScope.Bookmark => new Regex($@"^ *({BookPattern()}):\d* *"".+""", RegexOptions.Multiline),
				_ => throw new NotImplementedException()
			};

			Parallel.ForEach(lFiles, locFile =>
			{
				string l_file;
				try
				{
					l_file = File.ReadAllText(locFile.Path, UTF7);
				}
				catch (Exception)
				{
					locSuccess = false;
					return;
				}

				var collection = locRE.Matches(l_file);
				if (recall)
				{
					var member = new MembersCount();
					var tempMembers = Members.Where(m => Path.GetFileName(m.Path) == locFile.File);
					
					if (tempMembers.Any())
						member = tempMembers.First();

					if (!SelectedMod && member && member.Count != collection.Count)
					{
						success = false;
						member.Count = collection.Count;
					}
				}
				else if (collection.Count > 0)
				{
					lock (nameLock)
					{
						Members.Add(new MembersCount
						{
							Count = collection.Count,
							Path = locFile.Path,
							Type = scope
						});
						Members.Last().MemberScope();
					}
				}

				foreach (Match prov in collection)
				{
					NameSelect(prov.Value, locFile.Path, scope);
				}
			});

			if (scope == LocScope.Bookmark &&
				!success &&
				Bookmarks.Count(book => book.Name == null) == 0)
			{ success = true; }

			readSuccess = locSuccess;
			return success;
		}

		/// <summary>
		/// Calls a function to write the name of either a <see cref="Province"/> or a <see cref="Bookmark"/>.
		/// </summary>
		/// <param name="match">The RegEx match result.</param>
		/// <param name="path">Localisation file path.</param>
		/// <param name="scope">Province or Bookmark.</param>
		/// <returns><see langword="true"/> if the name was written.</returns>
		private static bool NameSelect(string match, string path, LocScope scope) => scope switch
		{
			LocScope.Province => NameProv(match, path),
			LocScope.Bookmark => NameBook(match, path),
			_ => false
		};

		/// <summary>
		/// Writes the localisation name to the matching <see cref="Province"/>.
		/// </summary>
		/// <param name="match">The RegEx match result. (Localisation file line)</param>
		/// <param name="path">Localisation file path.</param>
		/// <returns><see langword="true"/> if the name was written.</returns>
		private static bool NameProv(string match, string path)
		{
			string name = match.Split('"')[1].Trim();
			var provId = match.Split(':')[0].ToInt();

			if (name.Length < 1 || provId >= Provinces.Count) return false;
			if (!Provinces[provId]) return false;
			if (Provinces[provId].Name.Localisation != "" && path.Contains(GamePath)) return false;

			Provinces[provId].Name.Localisation = name;
			return true;
		}

		/// <summary>
		/// Writes the name to the matching <see cref="Bookmark"/>.
		/// </summary>
		/// <param name="match">The RegEx match result. (Bookmark name)</param>
		/// <param name="path">Localisation file path.</param>
		/// <returns><see langword="true"/> if the name was written.</returns>
		private static bool NameBook(string match, string path)
		{
			var tempBook = Bookmarks.First(book => book.Code == BookLocCodeRE.Match(match).Value);
			if (SelectedMod && tempBook.Name != null &&
			path.Contains(Directory.GetParent(GamePath + LocPath).FullName))
				return false;
			tempBook.Name = LocNameRE.Match(match).Value;

			return true;
		}

		/// <summary>
		/// A link between LocFiles <see cref="Settings"/> and <see cref="Members"/> list.
		/// </summary>
		/// <param name="mode">Read from the <see cref="Settings"/>, or Write to the <see cref="Settings"/></param>
		/// <param name="scope">Province or Bookmark</param>
		public static void LocMembers(Mode mode, LocScope scope)
		{
			switch (mode)
			{
				case Mode.Read:
					string[] lines = { };
					
					//lines = scope switch
					//{
					//	LocScope.Province => Settings.Default.ProvLocFiles.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries),
					//	LocScope.Bookmark => Settings.Default.BookLocFiles.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries),
					//	_ => throw new NotImplementedException()
					//};

					foreach (var member in lines.Where(l => l.Length > 5))
					{
						Members.Add(new MembersCount(member.Split('|')));
						Members.Last().MemberScope();
					}
					break;
				case Mode.Write:
					string join = "";
					foreach (var member in Members.Where(m => m.Type == scope))
					{
						join += $"{member}\r\n";
					}
					join = join[..^2];
					//switch (scope)
					//{
					//	case LocScope.Province:
					//		Settings.Default.ProvLocFiles = join;
					//		break;
					//	case LocScope.Bookmark:
					//		Settings.Default.BookLocFiles = join;
					//		break;
					//	default:
					//		break;
					//}
					break;
			}
		}

		/// <summary>
		/// Sets the owner object of each <see cref="Province"/> object in the 
		/// <see cref="Provinces"/> array according to the start date.
		/// <param name="updateOwner">true to ignore not empty owner.</param>
		/// </summary>
		public static void OwnerSetup(bool updateOwner = false)
		{
			Parallel.ForEach(ProvFiles, p_file =>
			{
				var match = ProvFileRE.Match(p_file.File);
				if (!match.Success) return;
				int i = match.Value.ToInt();
				if (i >= Provinces.Count) return;
				if (!updateOwner && Provinces[i].Owner) return;

				string provFile = File.ReadAllText(p_file.Path);
				var currentOwner = LastEvent(provFile, EventType.Province, StartDate);

				// Order is inverted to make handling of no result from LastEvent easier.
				// On the other hand, a successful LastEvent result removes the need for the ProvOwnerRE search.
				if (currentOwner == "")
				{
					match = ProvOwnerRE.Match(provFile);
					if (!match.Success) return;
					currentOwner = match.Value;
				}

				var owner = Countries.Where(c => c.Name == currentOwner);
				if (owner.Any())
					Provinces[i].Owner = owner.First();
			});
		}

		/// <summary>
		/// Checks validity of the line.
		/// </summary>
		/// <param name="line">The line to check.</param>
		/// <param name="eq"><see langword="true"/> to disable check for '='.</param>
		/// <returns><see langword="true"/> if the line should be skipped.</returns>
		public static bool NextLine(string line, bool eq = false)
		{
			// validLineRE is a dynamic RegEx so it is local.
			var validLineRE = new Regex(@$"^(?!#|[\s]+#)({(eq ? "" : ".+=")}[^#]+)");

			return !validLineRE.IsMatch(line);
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
				currentResult = match.Value;
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
			string pattern = "";
			foreach (var book in Bookmarks)
			{
				pattern += $"{book.Code}|";
			}
			return pattern[..^1];
		}

		#region Culture

		/// <summary>
		/// Creates the <see cref="Country"/> objects in the <see cref="Countries"/> list. <br />
		/// Initializes with code and culture object.
		/// </summary>
		public static void CountryCulSetup()
		{
			object countryLock = new object();
			Parallel.ForEach(CountryFiles, cFile =>
			{
				string code = cFile.File[..3];
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
					{ Countries.Add(country); }
				}
			});

		}

		/// <summary>
		/// Creates the <see cref="Culture"/> objects in the <see cref="Cultures"/> list. <br />
		/// Initializes with code and culture group object.
		/// </summary>
		/// <param name="culFile">The culture file to work on.</param>
		/// <param name="mutex">External lock to be provided when the function is parallelized.</param>
		private static void CultureSetup(string culFile, object mutex = null)
		{
			string[] cFile;
			try
			{
				cFile = File.ReadAllLines(culFile, UTF7);
			}
			catch (Exception) { return; }

			int brackets = 0;
			var cGroup = new Culture();
			foreach (string line in cFile)
			{
				if (NextLine(line, true)) continue;
				if (line.Contains('{'))
				{
					if (line.Contains('}')) continue;
					brackets++;
				}

				int eIndex = line.IndexOf('=');
				if (eIndex > -1 && brackets.Range(1, 2))
				{
					string temp = line[..eIndex].Trim();
					if (!NOT_CUL.Contains(temp))
					{
						var culture = new Culture(temp);
						switch (brackets)
						{
							case 1:
								cGroup = culture;
								break;
							case 2:
								culture.Group = cGroup;
								break;
						}

						// If the function isn't parallelized, there's no need for the lock
						if (mutex != null)
						{
							lock (mutex)
							{
								Cultures.Add(culture);
							}
						}
						else
							Cultures.Add(culture);
					}
				}

				if (line.Contains('}') && brackets > 0)
					brackets--;
			}
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
				CultureSetup(CultureFiles[0]);
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
			Parallel.ForEach(ProvNameFiles, fileName =>
			{
				string[] nFile = File.ReadAllLines(fileName.Path, UTF7);
				string name = fileName.File.Split('.')[0];
				List<ProvName> names = new List<ProvName>();

				foreach (var line in nFile)
				{
					if (NextLine(line)) { continue; }
					var split = line.Split('=');
					names.Add(new ProvName { 
						Index = split[0].Trim().ToInt(), 
						Name = LocNameRE.Match(split[1]).Value });
				}

				// store country query in a parent class
				IEnumerable<ProvNameClass> query = Countries.Where(cnt => cnt.Name == name);
				
				// If no matching country was found, update the query with culture
				if (!query.Any())
					query = Cultures.Where(cul => cul.Name == name);

				// If the query contains a country or a culture (or a culture group)
				if (query.Any())
					query.First().ProvNames = names;
			});
		}
		/// <summary>
		/// Finds the correct dynamic name source (owner / culture / group). <br />
		/// Also applies RNW / Unused policy.
		/// </summary>
		public static void DynamicSetup()
		{
			foreach (var prov in Provinces)
			{
				if (!prov) { continue; }
				prov.Name.Dynamic = "";
				if (!ShowRnw) { prov.IsRNW(); }
				if (!prov.Owner)
				{
					if (prov.Show
						&& prov.Name.Definition.Length < 1
						&& prov.Name.Localisation.Length < 1)
					{ prov.Show = false; }
					continue;
				}
				if (DynamicName(prov, NameType.Country)) { continue; }
				if (!prov.Owner.Culture) { continue; }
				if (DynamicName(prov, NameType.Culture)) { continue; }
				if (prov.Owner.Culture.Group) { DynamicName(prov, NameType.Group); }
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
			List<ProvName> source = mode switch
			{
				NameType.Country => prov.Owner.ProvNames,
				NameType.Culture => prov.Owner.Culture.ProvNames,
				NameType.Group => prov.Owner.Culture.Group.ProvNames,
				_ => throw new NotImplementedException()
			};

			if (source == null) return false;
			var query = source.Where(prv => prv.Index == prov.Index);
			if (query.Count() != 1) return false;
			prov.Name.Dynamic = query.First().Name.ToString();
			return true;
		}

		/// <summary>
		/// Creates the <see cref="Bookmark"/> objects in the <see cref="Bookmarks"/> array. <br /> 
		/// Initializes with code and date.
		/// </summary>
		public static void BookPrep()
		{
			foreach (var bookFile in BookFiles)
			{
				string bFile = File.ReadAllText(bookFile.Path);
				var codeMatch = BookmarkCodeRE.Match(bFile);
				var dateMatch = BookmarkDateRE.Match(bFile);
				if (!codeMatch.Success || !dateMatch.Success) { continue; }

				DateTime tempDate = DateParser(dateMatch.Value, StartDate.Year < 1000);
				if (tempDate == DateTime.MinValue) { continue; }
				Bookmarks.Add(new Bookmark {
					Code = codeMatch.Value,
					BookDate = tempDate,
					DefBook = BookmarkDefRE.Match(bFile).Success
				});
			}
			if (!Bookmarks.Any()) return; 
			LocPrep(LocScope.Bookmark);
			SortBooks();
		}

		/// <summary>
		/// Sorts the <see cref="Bookmarks"/> by date and removes bookmarks of the same date as the default one.
		/// </summary>
		private static void SortBooks()
		{
			Bookmarks.Sort();
			StartDate = Bookmarks[0].BookDate;
			if (Bookmarks.Count(book => book.DefBook) != 1) return;

			var tempBooks = Bookmarks.ToArray();
			Bookmarks.Clear();
			Bookmarks.Add(new Bookmark());
			int counter = 1;

			for (int i = 1; i < tempBooks.Length; i++)
			{
				if (tempBooks[i] == tempBooks[i - 1])
					counter++;
				else
				{
					if (counter > 1)
					{
						Bookmarks[0] = tempBooks.First(b => b.DefBook);
						counter = 1;
					}
					Bookmarks.Add(tempBooks[i]);
				}
			}
			if (counter > 1)
				Bookmarks[0] = tempBooks.First(b => b.DefBook);
		}

		/// <summary>
		/// Prepares mod defines files.
		/// </summary>
		public static void FetchDefines()
		{
			if (!SelectedMod) return;

			try
			{
				DefinesFiles = Directory.GetFiles(SteamModPath + DefinesPath).ToArray().Where(
					f => DefinesFileRE.Match(f).Success).ToList();
			}
			catch (Exception) { }
			DefinesFiles.Add(SteamModPath + DefinesLuaPath);
		}

		/// <summary>
		/// Handles multiple defines files parsing.
		/// </summary>
		public static void DefinesPrep()
		{
			if (SelectedMod)
			{
				Parallel.ForEach(DefinesFiles, dFile =>
				{
					DefinesSetup(dFile);
				});
				if (StartDate != DateTime.MinValue) return;
			}
			DefinesSetup(GamePath + DefinesLuaPath);
		}

		/// <summary>
		/// Parses the start date from a defines file.
		/// </summary>
		/// <param name="path">The defines file to parse.</param>
		private static void DefinesSetup(string path)
		{
			Match match;
			try
			{
				match = DefinesDateRE.Match(File.ReadAllText(path));
				StartDate = DateParser(match.Value, true);
			}
			catch (Exception) { return; }
		}

		/// <summary>
		/// Reads the files from the mod folder, and creates the <see cref="ModObj"/> 
		/// objects in the <see cref="Mods"/> list.
		/// </summary>
		public static void ModPrep()
		{
			var modLock = new object();
			string[] files;
			try
			{
				files = Directory.GetFiles(ParadoxModPath).ToArray().Where(
					f => ModFileRE.Match(f).Success).ToArray();
			}
			catch (Exception) { return; }

            //         foreach (var modFile in files)
            //         {
            //	string mFile = File.ReadAllText(modFile);
            //	var nameMatch = modNameRE.Match(mFile);
            //	var pathMatch = modPathRE.Match(mFile);
            //	var verMatch = modVerRE.Match(mFile);

            //	if (!(nameMatch.Success && pathMatch.Success && verMatch.Success)) return;

            //	var modPath = pathMatch.Value;

            //	if (!Directory.Exists(modPath))
            //	{
            //		var tempPath = $@"{Directory.GetParent(paradoxModPath).FullName}\{modPath.TrimStart('/', '\\')}";
            //		if (Directory.Exists(tempPath))
            //			modPath = tempPath;
            //		else return;
            //	}

            //	// With many mods in the folder, a context switch that will cause an OutOfRange exception is more likely
            //	//lock (modLock)
            //	//{
            //		mods.Add(new ModObj
            //		{
            //			Name = nameMatch.Value,
            //			Path = modPath,
            //			Ver = verMatch.Value,
            //			Replace = ReplacePrep(mFile)
            //		});
            //	//}
            //}

            Parallel.ForEach(files, modFile =>
            {
                string mFile = File.ReadAllText(modFile);
                var nameMatch = ModNameRE.Match(mFile);
                var pathMatch = ModPathRE.Match(mFile);
                var verMatch = ModVerRE.Match(mFile);

                if (!(nameMatch.Success && pathMatch.Success && verMatch.Success)) return;

                var modPath = pathMatch.Value;

                if (!Directory.Exists(modPath))
                {
                    var tempPath = $@"{Directory.GetParent(ParadoxModPath).FullName}\{modPath.TrimStart('/', '\\')}";
                    if (Directory.Exists(tempPath))
                        modPath = tempPath;
                    else return;
                }

                // With many mods in the folder, a context switch that will cause an OutOfRange exception is more likely
                lock (modLock)
                {
                    Mods.Add(new ModObj
                    {
                        Name = nameMatch.Value,
                        Path = modPath,
                        Ver = verMatch.Value,
                        Replace = ReplacePrep(mFile)
                    });
                }
            });

            Mods.Sort();
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
				.Select(m => m.Value);
				
			if (!vals.Any()) return new Replace();

			return new Replace {
				Cultures = vals.Contains(CulturesRep),
				Bookmarks = vals.Contains(BookmarksRep),
				ProvNames = vals.Contains(ProvNamesRep),
				Countries = vals.Contains(CountriesRep),
				Provinces = vals.Contains(ProvincesRep),
				Localisation = vals.Contains(LocalisationRep)
			};
		}

		/// <summary>
		/// Checks presence of OneDrive and selects the correct Documents folder
		/// </summary>
		public static void DocsPrep()
		{
			if (Directory.Exists(OneDrivePath) && Directory.EnumerateFileSystemEntries(OneDrivePath).Any())
				SelectedDocsPath = OneDrivePath;
			else
				SelectedDocsPath = DocsPath;
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
			Members.Clear();
			CultureFiles.Clear();
			DefinesFiles.Clear();
			LocFiles.Clear();
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
        /// Generates an exclusive random <see cref="Color"/>, that doesn't exist in the given <see cref="Province"/> array.
        /// </summary>
        /// <param name="provList">The <see cref="Province"/> array to be searched.</param>
        /// <returns>The generated <see cref="Color"/>.</returns>
        public static Color RandomProvColor(List<Province> provList, int red = -1, int green = -1, int blue = -1)
		{
			var rnd = new Random();
			int r, g, b;
			var tempColor = new Color();
			if (red >= 0 && green >= 0 && blue >= 0)
				return tempColor;

			do
			{
				r = red < 0 ? rnd.Next(0, 255) : red;
				g = green < 0 ? rnd.Next(0, 255) : green;
				b = blue < 0 ? rnd.Next(0, 255) : blue;
				tempColor = Color.FromArgb(r, g, b);
			} while (provList.Any(p => p && p.Color == tempColor));

			return tempColor;
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

			try
			{
				baseFiles = ApplyRegEx(scope, Directory.GetFiles(PathRep(scope)));
			}
			catch (Exception) { return false; }

			if (!SelectedMod || SelectReplace(scope))
			{
				filesList.AddRange(baseFiles.Select(f => new FileObj(f, scope)));
				return true;
			}

			try
			{
				addFiles = ApplyRegEx(scope, Directory.GetFiles(
					SteamModPath + SelectFolder(scope), "*", SearchOption.AllDirectories));
			}
			catch (Exception)
			{
				// If no mod files were found - just use the game files (except bookmarks)
				if (scope != FileType.Bookmark)
					filesList.AddRange(baseFiles.Select(f => new FileObj(f, scope)));

				return false;
			}

			filesList.AddRange(addFiles.Select(f => new FileObj(f, scope)));

			foreach (var b_file in baseFiles)
			{
				var TempFile = new FileObj(b_file, scope);
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
		private static string PathRep(FileType scope)
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
				case FileType.Localisation when rep.Localisation:
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
			FileType.Localisation => LocPath,
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
			FileType.Localisation => LocFiles,
			FileType.Country => CountryFiles,
			FileType.Bookmark => BookFiles,
			FileType.Province => ProvFiles,
			FileType.ProvName => ProvNameFiles,
			_ => throw new NotImplementedException(),
		};

		/// <summary>
		/// Applies RegEx on localisation files. <br />
		/// Other files are returned unchanged.
		/// </summary>
		/// <param name="scope">The type of the files to process.</param>
		/// <param name="query">The file to process.</param>
		/// <returns>The input query.</returns>
		private static IEnumerable<string> ApplyRegEx(FileType scope, IEnumerable<string> query)
		{
			if (scope == FileType.Localisation)
				return query.Where(f => LocFileRE.Match(f).Success);
			return query;
		}

		/// <summary>
		/// Checks if the query contains province name files or localisation files from /replace.
		/// </summary>
		/// <param name="scope">The type of the files to process.</param>
		/// <param name="query">The file to process.</param>
		/// <returns><see langword="true"/> for positive file count of: 
		/// country, bookmark, province, and replace localisation files. 
		/// <see langword="false"/> otherwise, and always for ProvName.</returns>
		private static bool ReplacedFile(FileType scope, IEnumerable<FileObj> query)
		{
			return scope switch
			{
				FileType.Localisation => RepLocCheck(query),
				FileType.Country when query.Any() => true,
				FileType.Bookmark when query.Any() => true,
				FileType.Province when query.Any() => true,
				FileType.ProvName => false,
				_ => false,
			};
		}

		/// <summary>
		/// Checks if the query contains localisation files from /replace.
		/// </summary>
		/// <param name="query"></param>
		/// <returns><see langword="true"/> if there is at least one replace file in the query.</returns>
		private static bool RepLocCheck(IEnumerable<FileObj> query)
		{
			return query.Any(f => f.Path.Contains(RepLocPath));
		}

		#endregion

		public static bool ErrorMsg(ErrorType type, bool returnVal = false)
        {
			//Error_Msg(type);
			return returnVal;
        }

        /// <summary>
        /// Handles all error messages.
        /// </summary>
        /// <param name="type">The error type</param>
        //private static void Error_Msg(ErrorType type) => _ = (type switch
        //{
        //    ErrorType.DefinRead => MessageBox.Show("The definition.csv file is missing or corrupt",
        //        "Unable to parse definition file", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.DefinWrite => MessageBox.Show("The definition.csv file is inaccessible for writing",
        //        "Unable to access definition file", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.DefMapRead => MessageBox.Show("The default.map file is missing or corrupt",
        //        "Unable to parse default file", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.DefMapWrite => MessageBox.Show("The default.map file is inaccessible for writing",
        //        "Unable to access default file", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.DefMapMaxProv => MessageBox.Show("The 'default.map' file has no max_provinces definition!",
        //        "Missing max_provinces", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.LocFolder => MessageBox.Show("The localisation folder is missing or inaccessible",
        //        "Unable to access localisation files", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.LocRead => MessageBox.Show("One or more of the localisation files are missing or corrupt",
        //        "Unable to parse localisation file(s)", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.HistoryProvFolder => MessageBox.Show($"The {histProvPath} folder is missing or inaccessible",
        //        "Unable to access province history files", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.ValDate => MessageBox.Show("At least one bookmark, or a defines entry are required to determine the start date",
        //        "Unable to determine start date", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.GameExe => MessageBox.Show("Cannot find the game executable!",
        //        "Missing EXE", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.NoCultures => MessageBox.Show("Unable to find any cultures in the culture file(s), or the file(s) / folder are inaccessible",
        //        "Missing cultures", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.NoCulGroups => MessageBox.Show("Unable to find any culture groups in the culture file(s)",
        //        "No culture groups", MessageBoxButton.OK, MessageBoxImage.Error),
        //    ErrorType.NoCountries => MessageBox.Show("Unable to find the countries folder, or the files are inaccessible",
        //        "Missing countries", MessageBoxButton.OK, MessageBoxImage.Error),
        //    _ => throw new NotImplementedException(),
        //});
    }
}

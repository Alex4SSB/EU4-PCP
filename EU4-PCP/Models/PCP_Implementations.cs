using EU4_PCP.Models;
using EU4_PCP.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
				if (!byte.TryParse(s[i], out res[i - startIndex]))
					return false;
			}
			return true;
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
			LocScope.ProvLoc => FileType.Province,
			LocScope.BookLoc => FileType.Bookmark,
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

			var provName = list.Length < 5 ? "" : list[4].Trim();

			return new Province(i, provName, provColor);
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
					if (DefinParse(line) is Province prov)
						provList.Add(prov.Index, prov);
				}
			}

			return provList;
		}

		/// <summary>
		/// Dynamically prepares the localisation files.
		/// </summary>
		/// <param name="scope">Province or Bookmark.</param>
		public static bool LocPrep(LocScope scope)
		{
			bool readSuccess;

			LocMembers(Mode.Read);
			if (Members.Any(m => m.Type == scope))
			{
				List<FileObj> filesList = new();
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
				//if (!abort && filesList.Any() && NameSetup(filesList, scope, out readSuccess))
				//{
				//	if (scope == LocScope.BookLoc) return readSuccess;

				//	var current = Provinces.Count(p => string.IsNullOrEmpty(p.Name.Localisation));
				//	var prev = Storage.RetrieveValue(General.NoLocProvCount.ToString());

				//	if (prev is not int prevI || prevI >= current)
				//	{
				//		Storage.StoreValue(current, General.NoLocProvCount);
				//		return readSuccess;
				//	}
				//}
			}

			// If there are no members in the settings, or the members have changed
			NameSetup(LocFiles, scope, out readSuccess);
			if (readSuccess)
				LocMembers(Mode.Write);

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
				LocScope.ProvLoc => LocProvRE,

				// bookRE is a dynamic RegEx so it is local.
				LocScope.BookLoc => new Regex($@"^ *({BookPattern()}):\d* *"".+""", RegexOptions.Multiline),
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

				Parallel.ForEach(collection, (prov) =>
				{
					NameSelect(prov.Value, IsGameDirectory(locFile.Path), scope);
				});
			});

			if (scope == LocScope.BookLoc &&
				!success &&
				!Bookmarks.Any(book => book.Name == null))
			{ success = true; }

			readSuccess = locSuccess;
			return success;
		}

		/// <summary>
		/// Calls a function to write the name of either a <see cref="Province"/> or a <see cref="Bookmark"/>.
		/// </summary>
		/// <param name="match">The RegEx match result.</param>
		/// <param name="scope">Province or Bookmark.</param>
		/// <returns><see langword="true"/> if the name was written.</returns>
		private static bool NameSelect(string match, bool gameDir, LocScope scope) => scope switch
		{
			LocScope.ProvLoc => NameProv(match, gameDir),
			LocScope.BookLoc => NameBook(match, gameDir),
			_ => false
		};

		/// <summary>
		/// Writes the localisation name to the matching <see cref="Province"/>.
		/// </summary>
		/// <param name="match">The RegEx match result. (Localisation file line)</param>
		/// <returns><see langword="true"/> if the name was written.</returns>
		private static bool NameProv(string match, bool gameDir)
		{
			string name = match.Split('"')[1].Trim();
			var provId = match.Split(':')[0].ToInt();

			Provinces.ContainsKey(provId);
			if (string.IsNullOrWhiteSpace(name) || !Provinces.ContainsKey(provId))
				return false;

			var prov = Provinces[provId];
			if (!string.IsNullOrEmpty(prov.Name.Localisation) && gameDir)
				return false;

			prov.Name.Localisation = name;
			return true;
		}

		/// <summary>
		/// Writes the name to the matching <see cref="Bookmark"/>.
		/// </summary>
		/// <param name="match">The RegEx match result. (Bookmark name)</param>
		/// <returns><see langword="true"/> if the name was written.</returns>
		private static bool NameBook(string match, bool gameDir)
		{
			var tempBook = Bookmarks.First(book => book.Code == BookLocCodeRE.Match(match).Value);
			if (SelectedMod && tempBook.Name != null && gameDir)
				return false;
			tempBook.Name = LocNameRE.Match(match).Value;

			return true;
		}

		private static bool IsGameDirectory(string path)
		{
			return path.Contains(Directory.GetParent(GamePath + LocPath).FullName);
		}

		/// <summary>
		/// A link between LocFiles <see cref="Settings"/> and <see cref="Members"/> list.
		/// </summary>
		/// <param name="mode">Read from the <see cref="Settings"/>, or Write to the <see cref="Settings"/></param>
		public static void LocMembers(Mode mode)
		{
			switch (mode)
			{
				case Mode.Read:
					var membersObj = Storage.RetrieveValue(General.LocFiles.ToString());
					if (membersObj is not JArray membersJA) return;

					Members = membersJA.ToObject<List<MembersCount>>();
					break;
				case Mode.Write:
					Storage.StoreValue(Members, General.LocFiles);
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
					currentOwner = match.Value;
				}

				if (Countries.Find(c => c.Name == currentOwner) is Country country)
					prov.Owner = country;
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
				List<ProvName> names = new();

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
			var showIllegal = Storage.RetrieveBool(General.ShowIllegalProv);

			foreach (var prov in Provinces.Values.Where(p => p))
			{
				prov.Name.Dynamic = "";
				if (!ShowRnw) prov.IsRNW();
				if (!prov.Owner)
				{
					if (!showIllegal && prov.Name.ToString() == "")
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
				if (!codeMatch.Success || !dateMatch.Success) continue;

				DateTime tempDate = DateParser(dateMatch.Value, StartDate.Year < 1000);
				if (tempDate == DateTime.MinValue) { continue; }
				Bookmarks.Add(new Bookmark {
					Code = codeMatch.Value,
					BookDate = tempDate,
					DefBook = BookmarkDefRE.Match(bFile).Success
				});
			}
			if (!Bookmarks.Any()) return; 
			//LocPrep(LocScope.BookLoc);
			Bookmarks = SortBooks(Bookmarks);
		}

		/// <summary>
		/// Removes bookmarks of the same date as the default one, and sorts them by date.
		/// </summary>
		private static List<Bookmark> SortBooks(List<Bookmark> bookmarks)
		{
			var sortedBooks = new List<Bookmark>();
			foreach (var item in bookmarks.GroupBy(book => book.BookDate).OrderBy(books => books.Key))
			{
				if (item.Count() == 1)
					sortedBooks.Add(item.Single());
				else if (item.Count(b => b.DefBook) == 1)
					sortedBooks.Add(item.First(book => book.DefBook));
			};

			return sortedBooks;
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
					GameIllegalProvinceCount = illegalCount;
					ModProvinceCount =
					ModIllegalProvinceCount = "";
					break;
				case Scope.Mod:
					ModProvinceCount = provCount;
					ModIllegalProvinceCount = illegalCount;
					if (string.IsNullOrEmpty(GameIllegalProvinceCount))
						GameIllegalProvinceCount = "?";
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

			BookmarkList = Bookmarks.Select(b => b.Name).ToList();

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

			StartDate = Bookmarks[SelectedBookmarkIndex].BookDate;
			StartDateStr = StartDate.ToString(DATE_FORMAT);

			ShowRnw = Storage.RetrieveBool(General.ShowAllProvinces);
			UpdateCountries = true;
			CountryCulSetup();
			OwnerSetup(true);
			ProvNameSetup();
			DynamicSetup();
		}

		/// <summary>
		/// Updates the definition.csv file by writing all current provinces.
		/// </summary>
		/// <returns><see langword="false"/> if the operation was not successful.</returns>
		public static bool WriteProvinces()
		{
			string stream = "province;red;green;blue;x;x\r\n";

			foreach (var prov in Provinces.Values)
			{
				stream += prov.ToCsv() + "\r\n";
			}

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
		/// Handles duplicate provinces - sets for each province its next duplicate.
		/// </summary>
		public static void DupliPrep()
		{
			foreach (var prov in Provinces)
			{
				prov.Value.NextDupli = null;
			}
			
			if (!CheckDupli || !SelectedMod) return;

			var dupliGroups = (from prov in Provinces.Values
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
		}

		/// <summary>
		/// Adds the given <see cref="Province"/> to the list if it doesn't exist. Otherwise, updates its color and definition name.
		/// </summary>
		/// <param name="newProv">The province to add / update</param>
		/// <returns></returns>
		public static bool AddProv(Province newProv)
		{
			var update = !(ChosenProv && ChosenProv.Color.IsLegal() && ChosenProv.IsNameLegal());

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

		public static List<Indexer> PathIndexer(string path, Scope scope, bool enBooks)
		{
			var source = IndexerSource(scope);
			string storageName = source + LocIndexer;
			var current = (from f in Directory.GetFiles(path, "*", SearchOption.AllDirectories)
						   where LocFileRE.Match(f).Success
						   select new Indexer(f, File.GetLastWriteTime(f), source)).ToList();

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
				{
					CacheLoc(ref modified, enBooks);
					foreach (var item in modified)
					{
						if (previous.Find(i => i.Path == item.Path) is Indexer prev)
							prev = item;
						else
							previous.Add(item);
					}
				}

				previous.RemoveAll(i => !current.Exists(c => c.Path == i.Path));
				current = previous;
			}

			Storage.StoreValue(current, storageName);
			return current;
		}

		private static string IndexerSource(Scope scope) => scope switch
		{
			Scope.Game => scope.ToString(),
			_ => SelectedMod.Name
		};

		public static void CacheLoc(ref List<Indexer> indexList, bool enBooks)
		{
			Regex bookRE = null;
			if (enBooks)
				bookRE = new Regex($@"^ *({BookPattern()}):\d* *"".+""", RegexOptions.Multiline);

			Parallel.ForEach(indexList, item =>
			{
				var text = File.ReadAllText(item.Path);
				var provMatches = LocProvRE.Matches(text);
				
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
				var val = match.Value;
				var name = val.Split('"')[1].Trim();
				var id = val.Split(':')[0].ToInt();

				dict.Add(id, name);
			}
		}

		public static void ReadProvLoc(List<Indexer> indexers)
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
				var val = match.Value;
				var code = BookLocCodeRE.Match(val).Value;
				var name = LocNameRE.Match(val).Value;

				dict.Add(code, name);
			}
		}

		public static void ReadBookLoc(List<Indexer> indexers)
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
				FileType.Country or FileType.Bookmark or FileType.Province when query.Any() => true,
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
			ErrorType.LocFolder => MessageBox.Show("The localisation folder is missing or inaccessible",
				"Unable to access localisation files", MessageBoxButton.OK, MessageBoxImage.Error),
			ErrorType.LocRead => MessageBox.Show("One or more of the localisation files are missing or corrupt",
				"Unable to parse localisation file(s)", MessageBoxButton.OK, MessageBoxImage.Error),
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

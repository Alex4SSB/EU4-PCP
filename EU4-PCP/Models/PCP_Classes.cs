using System;
using System.Collections.Generic;
using System.Windows.Media;
using static EU4_PCP.PCP_Implementations;

namespace EU4_PCP
{

    public abstract class AbstractProvince : IComparable<AbstractProvince>
    {
        public int Index { get; set; }
        public CompositeName Name;

        public AbstractProvince(int index = -1, CompositeName name = null)
        {
            Index = index;
            Name = name is null ? new() : name;
        }

        public bool IsNameLegal()
        {
            return ToString() is not null;
        }

        public int CompareTo(AbstractProvince other)
        {
            return Index.CompareTo(other.Index);
        }

        public override string ToString() => Name.ToString();

        public static implicit operator bool(AbstractProvince obj)
        {
            return obj is object && obj.Index > -1;
        }

        public static implicit operator int(AbstractProvince prov)
        {
            return prov.Index;
        }

    }

    public class Province : AbstractProvince
    {
        public P_Color Color;
        public Country Owner;
        public bool Show = true;
        public Province NextDupli = null;

        public Province(int index = -1, CompositeName name = null, P_Color color = null) : base(index, name)
        {
            Color = new(color);
        }

        public Province(Province prov) : base(prov.Index, prov.Name)
        {
            Color = prov.Color;
            Owner = prov.Owner;
            Show = prov.Show;
            NextDupli = prov.NextDupli;
        }

        public string ToCsv()
        {
            return $"{Index};{Color.ToCsv()};{Name.Definition};{AddSuffix()}";
        }

        private string AddSuffix()
        {
            if (string.IsNullOrEmpty(Name.AltDefin))
            {
                return !IsRNW(false) && IsNameLegal() && Color.IsLegal()
                    ? "x" : "";
            }
            else
                return Name.AltDefin;
        }

        public bool IsRNW(bool updateShow = true)
        {
            bool isRnw = Name.Definition is not null && PCP_RegEx.RnwRE.Match(Name.Definition).Success;
            if (updateShow && isRnw)
                Show = false;
            return isRnw;
        }

        public bool IsDupli(bool dupliEnabled) => dupliEnabled && NextDupli;

        public bool IsIllegal(bool illegalEnabled) => illegalEnabled && (!IsNameLegal() || !Color.IsLegal());

    }

    public class TableProvince : AbstractProvince
    {
        public readonly Province province;
        public string Color { get { return $"#{(province.Color.IsLegal() ? province.Color.Name : Colors.Transparent)}"; } }
        public new string Name { get { return province.Name.ToString(); } }
        public short Red { get { return province.Color.R; } }
        public short Green { get { return province.Color.G; } }
        public short Blue { get { return province.Color.B; } }

        public bool IsProvDupli { get { return province.NextDupli; } }
        public string IsColorLegal { get { return province.Color.IsLegal() ? "" : "\uE711"; } }
        public bool IsProvLegal { get { return province.IsNameLegal() && province.Color.IsLegal(); } }

        public TableProvince(Province prov) : base(prov)
        {
            province = prov;
        }

        public override string ToString() => Name;
    }

    public class ProvNameClass
    {
        public Dictionary<int, string> ProvNames;
        public string Name;

        public ProvNameClass(string name, Dictionary<int, string> provNames)
        {
            ProvNames = provNames;
            Name = name;
        }

        public ProvNameClass()
        { }

        public static implicit operator bool(ProvNameClass obj)
        {
            return obj is object;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Country : ProvNameClass
    {
        public Culture Culture;
    }

    public class Culture : ProvNameClass
    {
        public Culture Group;

        public Culture() { }

        public Culture(string name)
        {
            Name = name;
        }
    }

    public class CompositeName
    {
        public string Definition;
        public string Localisation;
        public string Dynamic;
        public string AltDefin;

        public CompositeName(string definition = null, string localisation = null, string dynamic = null, string alt = null)
        {
            Definition = definition;
            Localisation = localisation;
            Dynamic = dynamic;
            AltDefin = alt;
        }

        public override string ToString()
        {
            if (Dynamic is not null) { return Dynamic; }
            if (Localisation is not null) { return Localisation; }
            return Definition;
        }

        public static implicit operator CompositeName(string name)
        {
            return new CompositeName(name);
        }

        public static implicit operator bool(CompositeName obj)
        {
            return obj is object;
        }
    }

    public class P_Color
    {
        public short R, G, B;

        /// <summary>
        /// Gets the name of this <see cref="Color"/>.
        /// </summary>
        public string Name => ((Color)this).ToString().TrimStart('#');

        public P_Color(P_Color obj) : this(obj.R, obj.G, obj.B)
        { }

        public P_Color(Color obj) : this(obj.R, obj.G, obj.B)
        { }

        public P_Color(params byte[] arr) : this(arr[0], arr[1], (short)arr[2]) // One cast is enough to call the other C-tor (otherwise, tries to call itself)
        { }

        public P_Color(string[] str)
        {
            short[] s_arr = new short[3];
            for (int i = 0; i < 3; i++)
            {
                if (!short.TryParse(DigitStr(str[i]), out s_arr[i]))
                    s_arr[i] = -1;
            }

            R = s_arr[0];
            G = s_arr[1];
            B = s_arr[2];
        }

        public P_Color(short r, short g, short b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R_()
        {
            return (byte)R;
        }

        public byte G_()
        {
            return (byte)G;
        }

        public byte B_()
        {
            return (byte)B;
        }

        public byte[] AsByteArr()
        {
            return new byte[] { R_(), G_(), B_() };
        }

        public bool IsLegal() => R.Range(0, 255) && G.Range(0, 255) && B.Range(0, 255);

        public string ToCsv()
        {
            return $"{(R < 0 ? "" : R)};{(G < 0 ? "" : G)};{(B < 0 ? "" : B)}";
        }

        public string AsHex()
        {
            return IsLegal() ? $"#{R:x2}{G:x2}{B:x2}".ToUpper() : "";
        }

        public override bool Equals(object obj)
        {
            return obj is P_Color color &&
                   R == color.R &&
                   G == color.G &&
                   B == color.B;
        }

        public static implicit operator Color(P_Color obj)
        {
            return Color.FromRgb(obj.R_(), obj.G_(), obj.B_());
        }

        public static implicit operator P_Color(Color obj)
        {
            return new P_Color(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return AsHex();
        }

    }

    public class AbstractBookmark : IComparable<AbstractBookmark>
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public AbstractBookmark()
        { }

        public AbstractBookmark(AbstractBookmark other)
        {
            Date = other.Date;
            Name = other.Name;
        }

        public int CompareTo(AbstractBookmark other)
        {
            return Date.CompareTo(other.Date);
        }

        public override bool Equals(object obj)
        {
            return obj is AbstractBookmark bookmark && Date == bookmark.Date;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(AbstractBookmark left, AbstractBookmark right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(AbstractBookmark left, AbstractBookmark right)
        {
            return left.CompareTo(right) != 0;
        }

        public static implicit operator bool(AbstractBookmark obj)
        {
            return obj is object;
        }
    }

    public class Bookmark : AbstractBookmark
    {
        public string Code { get; set; }
        public bool IsDefault { get; set; }

        public Bookmark() : base()
        { }

        public Bookmark(Bookmark book) : base(book)
        {
            Code = book.Code;
            IsDefault = book.IsDefault;
        }

        public override string ToString()
        {
            if (Name != null)
                return Name;

            return Code;
        }
    }

    public class ListBookmark : AbstractBookmark
    {
        public new string Date { get; set; }

        public ListBookmark(Bookmark book, bool useDefaultFormat = false) : base(book)
        {
            Date = CurrentDateFormat(false, base.Date, useDefaultFormat);
        }
    }

    public class ModObj : IComparable<ModObj>
    {
        public string Name;
        public string Path;
        public string Ver; // Supported game version
        public Replace Replace;

        public static implicit operator bool(ModObj obj)
        {
            return obj is object;
        }

        public static implicit operator Scope(ModObj obj)
        {
            return obj ? Scope.Mod : Scope.Game;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(ModObj other)
        {
            return Name.CompareTo(other.Name);
        }

        public ModObj() { }

    }

    public class Replace
    {
        public bool Countries = false;
        public bool Provinces = false;
        public bool Cultures = false;
        public bool Bookmarks = false;
        public bool ProvNames = false;
        public bool Localisation = false;

        public static implicit operator bool(Replace obj)
        {
            return obj is object;
        }

        public Replace() { }
    }

    public class FileObj
    {
        public string Path;
        public string File;

        public static implicit operator bool(FileObj obj)
        {
            return obj is object;
        }

        public FileObj(string fPath)
        {
            Path = fPath;
            File = System.IO.Path.GetFileName(fPath);
        }

        public static bool operator ==(FileObj left, FileObj right)
        {
            return left.File == right.File;
        }

        public static bool operator !=(FileObj left, FileObj right)
        {
            return left.File != right.File;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Path;
        }
    }

    public class Indexer
    {
        public string Path;
        public DateTime LastModified;
        public string Source;
        public Dictionary<int, string> ProvDict;
        public Dictionary<string, string> BookDict;

        public Indexer(string path, DateTime lastModified, string source, Dictionary<int, string> provDict = null, Dictionary<string, string> bookDict = null)
        {
            Path = path;
            LastModified = lastModified;
            Source = source;

            ProvDict = provDict is null ? new() : provDict;
            BookDict = bookDict is null ? new() : bookDict;
        }

        public Indexer() { }

        public static implicit operator bool(Indexer obj)
        {
            return obj is object;
        }

        public override string ToString()
        {
            return $"{Source} Loc Indexer";
        }
    }
}

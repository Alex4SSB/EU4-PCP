using System;
using System.Collections.Generic;
using System.Windows.Media;
using static EU4_PCP.PCP_Implementations;

namespace EU4_PCP
{

    public abstract class ProvinceAbstract : IComparable<ProvinceAbstract>
    {
        public int Index;
        public CompositeName Name;

        public ProvinceAbstract(int index = -1, CompositeName name = null)
        {
            Index = index;
            Name = name is null ? new() : name;
        }

        public bool IsNameLegal()
        {
            return ToString() is not null;
        }

        public int CompareTo(ProvinceAbstract other)
        {
            return Index.CompareTo(other.Index);
        }

        public override string ToString() => Name.ToString();

        public static implicit operator bool(ProvinceAbstract obj)
        {
            return obj is object && obj.Index > -1;
        }

        public static implicit operator int(ProvinceAbstract prov)
        {
            return prov.Index;
        }

    }

    public class Province : ProvinceAbstract
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

    }

    public class TableProvince : Province
    {
        public string B_Color { get { return $"#{(Color.IsLegal() ? Color.Name : "00ffffff")}"; } }
        public int ID { get { return Index; } }
        public string P_Name { get { return Name.ToString(); } }
        public short Red { get { return Color.R; } }
        public short Green { get { return Color.G; } }
        public short Blue { get { return Color.B; } }

        public bool IsDupli { get { return NextDupli; } }
        public string IsColorLegal { get { return Color.IsLegal() ? "" : "\uE711"; } }
        public bool IsProvLegal { get { return Color.IsLegal() && IsNameLegal(); } }

        public TableProvince(Province prov) : base(prov) { }
    }

    public class ProvName : ProvinceAbstract
    { }

    public class ProvNameClass
    {
        public List<ProvName> ProvNames;
        public string Name;

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

    public class Bookmark : IComparable<Bookmark>
    {
        public string Code;
        public DateTime BookDate;
        public string Name;
        public bool DefBook;

        public static implicit operator bool(Bookmark obj)
        {
            return obj is object;
        }

        public override string ToString()
        {
            if (Name != null)
                return Name;

            return Code;
        }

        public int CompareTo(Bookmark other)
        {
            return BookDate.CompareTo(other.BookDate);
        }

        public override bool Equals(object obj)
        {
            return obj is Bookmark bookmark && BookDate == bookmark.BookDate;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Bookmark left, Bookmark right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Bookmark left, Bookmark right)
        {
            return left.CompareTo(right) != 0;
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

namespace EU4_PCP;


public enum Mode
{
    Read,
    Write
}

public enum Scope
{
    Game,
    Mod
}

public enum EventType
{
    Province,
    Country
}

public enum FileType
{
    Country,
    Bookmark,
    Province,
    ProvName
}

public enum NameType
{
    Country,
    Culture,
    Group
}

public enum CriticalScope
{
    Game,
    Mod,
    Bookmark
}

public enum ErrorType
{
    DefinRead,
    DefinWrite,
    DefMapRead,
    DefMapWrite,
    DefMapMaxProv,
    LocRead,
    HistoryProvFolder,
    ValDate,
    GameExe,
    NoCultures,
    NoCulGroups,
    NoCountries
}

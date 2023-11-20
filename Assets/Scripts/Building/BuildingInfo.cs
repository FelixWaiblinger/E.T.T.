public class BuildingInfo
{
    public string Name { get; private set; }
    public string Level { get; private set; }
    public string Gain { get; private set; }
    public string Cost { get; private set; }
    public int Index { get; private set; }

    public BuildingInfo(string n, string l, string g, string c, int i)
    {
        Name = n;
        Level = l;
        Gain = g;
        Cost = c;
        Index = i;
    }
}

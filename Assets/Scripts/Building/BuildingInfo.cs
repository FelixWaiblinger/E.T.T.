public class BuildingInfo
{
    public string Name;
    public string Level;
    public string Gain;
    public string Cost;
    public int Index;

    public BuildingInfo() {}

    public BuildingInfo(string n, string l, string g, string c, int i)
    {
        Name = n;
        Level = l;
        Gain = g;
        Cost = c;
        Index = i;
    }
}

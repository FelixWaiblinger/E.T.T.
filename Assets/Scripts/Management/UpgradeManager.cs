public static class UpgradeManager
{
    public static Money FactoryUpgrade(Money current)
    {
        return current * 5f;
    }

    public static float NatureUpgrade(float current, int level)
    {
        return current * (0.2f * (level - 1));
    }

    public static float MilitaryUpgrade(float current)
    {
        return current * 10f;
    }

    public static float ReactorUpgrade(float current)
    {
        return current * 10f;
    }

    public static float HeadquarterUpgrade(float current)
    {
        return current * 10f;
    }

    public static float TransportationUpgrade(float current)
    {
        return current * 10f;
    }

    public static float DysonUpgrade(float current)
    {
        return current * 10f;
    }

    public static float CoreUpgrade(float current)
    {
        return current * 10f;
    }

    public static float TimeUpgrade(float current)
    {
        return current * 10f;
    }
}

using UnityEngine;

[System.Serializable]
public class Money
{
    [SerializeField] private float _mantissa;
    [SerializeField] private int _exponent;
    private static string[] _letters = new string[]
    {
        "", "k", "m", "M", "b", "B", "t", "T", "q", "Q", "p", "P", "s", "S",
        "h", "H", "o", "O", "n", "N", "d", "D", "ud", "uD", "dd", "dD", "td",
        "tD", "qd", "qD", "pd", "pD", "sd", "sD", "hd", "hD", "od", "oD", "nd",
        "nD", // "S", "h", "H", "o", "O", "n", "N", "d", "D", "ud", "uD", "dd"
    };
    private static string[] _words = new string[]
    {
        "", "Thousand", "Million", "Milliard", "Billion", "Billiard",
        "Trillion", "Trilliard", "Quadrillion", "Quadrilliard", "Pentillion",
        "Pentilliard", "Sextillion", "Sextilliard", "Heptillion",
        "Heptilliard", "Octillion", "Octilliard", "Nonillion", "Nonilliard",
        "Decillion", "Decilliard", "Undecillion", "Undecilliard",
        "Duodecillion", "Duodecilliard", "Tredecillion", "Tredecilliard",
        "Quattuordecillion", "Quattuordecilliard", "Pendecillion",
        "Pendecilliard", "Sedecillion", "Sedecilliard", "Heptdecillion",
        "Heptdecilliard", "Octodecillion", "Octodecilliard", "Novendecillion",
        "Nondecilliard",
        // "S", "h", "H", "o", "O", "n", "N", "d", "D", "ud", "uD", "dd"
    };

    public Money(float m, int e)
    {
        _mantissa = m;
        _exponent = e;
    }

    public static Money operator +(Money m1, Money m2)
    {
        var ediff = m1._exponent - m2._exponent;

        // m1 >> m2
        if (ediff > 5) return new Money(m1._mantissa, m1._exponent);
        // m2 >> m1
        else if (ediff < -5) return new Money(m2._mantissa, m2._exponent);
        
        // m1 ~= m2
        var m = m1._mantissa * (ediff < 0 ? Mathf.Pow(10, ediff) : 1) +
                m2._mantissa * (ediff > 0 ? Mathf.Pow(10, -ediff) : 1);

        // move to next unit if necessary
        var e = ediff > 0 ? m1._exponent : m2._exponent;
        if (m >= 1000f) { m /= 1000f; e += 3; }

        return new Money(m, e);
    }

    public static Money operator -(Money m1, Money m2)
    {
        var ediff = m1._exponent - m2._exponent;

        // m1 >> m2
        if (ediff > 5) return new Money(m1._mantissa, m1._exponent);
        // m2 >> m1
        else if (ediff < -5) return new Money(m2._mantissa, m2._exponent);
        
        // m1 ~= m2
        var m = m1._mantissa * (ediff < 0 ? Mathf.Pow(10, ediff) : 1) -
                m2._mantissa * (ediff > 0 ? Mathf.Pow(10, -ediff) : 1);

        // move to next unit if necessary
        var e = ediff > 0 ? m1._exponent : m2._exponent;
        if (Mathf.Abs(m) < 1f) { m *= 1000f; e -= 3; }

        return new Money(m, e);
    }

    public static bool operator <(Money m1, Money m2)
    {
        if (m1._exponent < m2._exponent) return true;
        else if (m1._exponent > m2._exponent) return false;
        else return m1._mantissa < m2._mantissa;
    }

    public static bool operator >(Money m1, Money m2)
    {
        return m2 < m1;
    }

    public override string ToString()
    {
        return _mantissa.ToString("0.00") + _letters[_exponent / 3];
    }
}

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
        "nD"
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
        "Nondecilliard"
    };

    public Money(float m, int e)
    {
        _mantissa = m;
        _exponent = e;
    }

    public float Mantissa()
    {
        return _mantissa;
    }

    public int Exponent()
    {
        return _exponent;
    }
    
    // negation
    public static Money operator -(Money m)
    {
        return new Money(-m._mantissa, m._exponent);
    }

    // addition
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
        if (m >= 1000f)
        {
            m /= 1000f;
            e += 3;
        }

        return new Money(m, e);
    }

    // subtraction
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

    // scaling
    public static Money operator *(Money m, float f)
    {
        var mf = m._mantissa * f;
        var e = m._exponent;

        if (mf >= 1000f)
        {
            mf /= 1000f;
            e += 3;
        }
        else if (mf < 1f)
        {
            mf *= 1000f;
            e -= 3;
        }

        return new Money(mf, e);
    }

    // less
    public static bool operator <(Money m1, Money m2)
    {
        if (m1._exponent < m2._exponent) return true;
        else if (m1._exponent > m2._exponent) return false;
        else return m1._mantissa < m2._mantissa;
    }

    // greater
    public static bool operator >(Money m1, Money m2)
    {
        return m2 < m1;
    }

    // display
    public override string ToString()
    {
        return _mantissa.ToString("0.00") + _letters[_exponent / 3];
    }
}

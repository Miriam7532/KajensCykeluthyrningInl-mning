
namespace KajensCykeluthyrning;

public class Elcykel : Fordon
{
    public int Batteriniva { get; private set; }

    public Elcykel(int nummer, int batteriniva)
        : base(nummer, 70m)
    {
        if (batteriniva < 0 || batteriniva > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(batteriniva),
                "Battery level must be between 0 and 100."
            );
        }

        Batteriniva = batteriniva;
    }

    public void Ladda()
    {
        Batteriniva = 100;
    }

    public void ForbrukaBatteri(int timmar)
    {
        Batteriniva = Math.Max(0, Batteriniva - (timmar * 10));
    }
}

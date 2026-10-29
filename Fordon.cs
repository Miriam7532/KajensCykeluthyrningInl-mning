
namespace KajensCykeluthyrning;

public class Fordon
{
    public int Nummer { get; set; }
    public decimal Timpris { get; set; }
    public bool ArLedig { get; set; }

    public Fordon(int nummer, decimal timpris)
    {
        Nummer = nummer;
        Timpris = timpris;
        ArLedig = true;
    }

    public decimal BeraknaPris(int timmar)
    {
        return Timpris * timmar;
    }
}


namespace KajensCykeluthyrning;

public class Uthyrning
{
    public Kund Kund { get; }
    public Fordon Fordon { get; }
    public int Timmar { get; }

    public Uthyrning(Kund kund, Fordon fordon, int timmar)
    {
        Kund = kund;
        Fordon = fordon;
        Timmar = timmar;
    }

    public decimal BeraknaGrundpris()
    {
        return Fordon.BeraknaPris(Timmar);
    }

    public decimal BeraknaRabatt()
    {
        if (Kund.ArMedlem)
        {
            return BeraknaGrundpris() * 0.10m;
        }

        return 0m;
    }

    public decimal BeraknaTotalpris()
    {
        return BeraknaGrundpris() - BeraknaRabatt();
    }

    public void VisaKvitto()
    {
        Console.WriteLine("\n===== RECEIPT =====");
        Console.WriteLine($"Customer: {Kund.Namn}");
        Console.WriteLine($"Vehicle: {Fordon.GetType().Name} {Fordon.Nummer}");
        Console.WriteLine($"Rental duration: {Timmar} hours");
        Console.WriteLine($"Hourly rate: {Fordon.Timpris:F2} kr");
        Console.WriteLine($"Subtotal: {BeraknaGrundpris():F2} kr");
        Console.WriteLine($"Member discount: -{BeraknaRabatt():F2} kr");
        Console.WriteLine($"Total to pay: {BeraknaTotalpris():F2} kr");
        Console.WriteLine("===================");
    }
}


namespace KajensCykeluthyrning;

internal class Program
{
    static List<Fordon> fordon = new List<Fordon>();
    static List<Kund> kunder = new List<Kund>();
    static List<Uthyrning> uthyrningar = new List<Uthyrning>();

    static void Main()
    {
        // Add the starting vehicles
        fordon.Add(new Cykel(1));
        fordon.Add(new Cykel(2));
        fordon.Add(new Elcykel(3, 80));
        fordon.Add(new Elcykel(4, 25));

        bool korProgram = true;

        while (korProgram)
        {
            Console.WriteLine("\n=== Kajens Cykeluthyrning ===");
            Console.WriteLine("1. Show vehicles");
            Console.WriteLine("2. Register customer");
            Console.WriteLine("3. Rent a vehicle");
            Console.WriteLine("4. Return a vehicle");
            Console.WriteLine("5. Charge electric bicycle");
            Console.WriteLine("6. Exit");
            Console.Write("Choice: ");

            string val = Console.ReadLine() ?? "";

            switch (val)
            {
                case "1":
                    VisaFordon();
                    break;

                case "2":
                    RegistreraKund();
                    break;

                case "3":
                    HyrUt();
                    break;

                case "4":
                    LamnaTillbaka();
                    break;

                case "5":
                    LaddaFordon();
                    break;

                case "6":
                    korProgram = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    static void VisaFordon()
    {
        Console.WriteLine("\n--- Vehicles ---");

        foreach (Fordon f in fordon)
        {
            string status = f.ArLedig ? "Available" : "Rented";

            Console.Write($"Number: {f.Nummer}, ");
            Console.Write($"Type: {f.GetType().Name}, ");
            Console.Write($"Status: {status}");

            if (f is Elcykel elcykel)
            {
                Console.Write($", Battery: {elcykel.Batteriniva}%");
            }

            Console.WriteLine();
        }
    }

    static void RegistreraKund()
    {
        Console.Write("Name: ");
        string namn = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(namn))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        if (kunder.Any(k =>
            k.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("This customer is already registered.");
            return;
        }

        int alder = LasHeltal("Age: ", 0, 120);

        Console.Write("Member (y/n): ");
        string medlemSvar = (Console.ReadLine() ?? "").Trim();

        bool arMedlem;

        if (medlemSvar.Equals("y", StringComparison.OrdinalIgnoreCase) ||
            medlemSvar.Equals("j", StringComparison.OrdinalIgnoreCase))
        {
            arMedlem = true;
        }
        else if (medlemSvar.Equals("n", StringComparison.OrdinalIgnoreCase))
        {
            arMedlem = false;
        }
        else
        {
            Console.WriteLine("Please enter y, j or n.");
            return;
        }

        Kund kund = new Kund(namn, alder, arMedlem);
        kunder.Add(kund);

        Console.WriteLine($"{namn} has been registered.");
    }

    static void HyrUt()
    {
        Console.Write("Customer name: ");
        string namn = (Console.ReadLine() ?? "").Trim();

        Kund? kund = kunder.FirstOrDefault(k =>
            k.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase));

        if (kund == null)
        {
            Console.WriteLine("Customer not found. Register the customer first.");
            return;
        }

        int nummer = LasHeltal("Vehicle number: ", 1, int.MaxValue);

        Fordon? f = fordon.FirstOrDefault(x => x.Nummer == nummer);

        if (f == null)
        {
            Console.WriteLine("Vehicle not found.");
            return;
        }

        if (!f.ArLedig)
        {
            Console.WriteLine("This vehicle is already rented.");
            return;
        }

        if (f is Cykel && f is not Elcykel && kund.Alder < 15)
        {
            Console.WriteLine("The customer must be at least 15 to rent a bicycle.");
            return;
        }

        if (f is Elcykel elcykel)
        {
            if (kund.Alder < 18)
            {
                Console.WriteLine("The customer must be at least 18 to rent an electric bicycle.");
                return;
            }

            if (elcykel.Batteriniva < 30)
            {
                Console.WriteLine(
                    $"Electric bicycle {nummer} has too little battery " +
                    $"({elcykel.Batteriniva}%). Charge it first.");
                return;
            }
        }

        int timmar = LasHeltal("Number of hours: ", 1, int.MaxValue);

        Uthyrning uthyrning = new Uthyrning(kund, f, timmar);
        uthyrningar.Add(uthyrning);

        f.ArLedig = false;

        Console.WriteLine(
            $"{kund.Namn} has rented {f.GetType().Name} {f.Nummer} " +
            $"for {timmar} hours.");
    }

    static void LamnaTillbaka()
    {
        int nummer = LasHeltal("Vehicle number to return: ", 1, int.MaxValue);

        Uthyrning? uthyrning = uthyrningar.FirstOrDefault(
            u => u.Fordon.Nummer == nummer);

        if (uthyrning == null)
        {
            Console.WriteLine("No active rental found for this vehicle.");
            return;
        }

        uthyrning.VisaKvitto();

        if (uthyrning.Fordon is Elcykel elcykel)
        {
            elcykel.ForbrukaBatteri(uthyrning.Timmar);
        }

        uthyrning.Fordon.ArLedig = true;
        uthyrningar.Remove(uthyrning);

        Console.WriteLine("Vehicle returned successfully.");
    }

    static void LaddaFordon()
    {
        int nummer = LasHeltal("Electric bicycle number: ", 1, int.MaxValue);

        Fordon? f = fordon.FirstOrDefault(x => x.Nummer == nummer);

        if (f == null)
        {
            Console.WriteLine("Vehicle not found.");
            return;
        }

        if (f is not Elcykel elcykel)
        {
            Console.WriteLine("This vehicle does not have a battery.");
            return;
        }

        if (!elcykel.ArLedig)
        {
            Console.WriteLine("Return the electric bicycle before charging it.");
            return;
        }

        elcykel.Ladda();

        Console.WriteLine(
            $"Electric bicycle {nummer} is fully charged (100%).");
    }

    static int LasHeltal(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int value) &&
                value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine(
                $"Enter a whole number between {min} and {max}.");
        }
    }
}


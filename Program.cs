using System.Text.Json;

Random rng = new();

string civilisationName = "";

int gold =0;
//float levelOfCulture =0;

List<City> Cities = new();

string json = File.ReadAllText("PossibleEvents.json");
List<Evenement> PossiblesEvents = JsonSerializer.Deserialize<List<Evenement>>(json)!;

int maxEvent = PossiblesEvents.Count;

Game();

void Game()
{
    civilisationName = SelectCivilisationName();
    InititFirstCity();

    foreach (City city in Cities)
    {
        Console.WriteLine(city.name);
        Console.WriteLine($"Nombre de citoyens : {city.numberOfCivilian}");
        Console.WriteLine(city.numberOfScientist);
        Console.WriteLine(city.numberOfWorker);
    }
    Console.WriteLine($"Gold actuel : {gold}");

    Console.WriteLine("+------------------+");


    foreach (City city in Cities)
    {
        city.currentEvent = PickRandomEvent();
        Console.WriteLine(city.currentEvent.Description);
        ExecuteEvent(city);
    }

    Console.WriteLine("+------------------+");

    
    foreach (City city in Cities)
    {
        Console.WriteLine(city.name);
        Console.WriteLine($"Nombre de citoyens : {city.numberOfCivilian}");
        Console.WriteLine(city.numberOfScientist);
        Console.WriteLine(city.numberOfWorker);
    }
    Console.WriteLine($"Gold actuel : {gold}");
}

string SelectCivilisationName()
{
    string nameChoosed = "";

    Console.Write("Choissisez le nom de votre Civilisation : ");
    nameChoosed = Console.ReadLine() ?? string.Empty;

    return nameChoosed;
}

void InititFirstCity()
{
    Cities.Add(new City("Ferdonia",10,0,0, null));
}

Evenement PickRandomEvent()
{
    Evenement choosedEvent = PossiblesEvents[rng.Next(0,maxEvent)];
    return choosedEvent;
}

void ExecuteEvent(City city)
{
    switch (city.currentEvent?.effects?.TypeRequested)
    {
        case "Gold":
        gold += city.currentEvent.effects.Modificator;
        break;
        
        case "numberOfCivilian":
        city.numberOfCivilian += city.currentEvent.effects.Modificator;
        break;

        default:
        break;
    }
}
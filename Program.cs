using System.Text.Json;

Random rng = new();

string civilisationName = "";

int gold =0;
float levelOfCulture =0;

List<City> Cities = new();

string json = File.ReadAllText("PossibleEvents.json");
List<Evenement> PossiblesEvents = JsonSerializer.Deserialize<List<Evenement>>(json)!;

int maxEvent = PossiblesEvents.Count;

bool canRun = true;

Init();

void Init()
{
    civilisationName = SelectCivilisationName();
    InititFirstCity();
    Game();
}
void Game()
{
    while(canRun){
        
        foreach (City city in Cities)
        {
            city.currentEvent = PickRandomEvent();
            Console.WriteLine(city.currentEvent.Description);
            ExecuteEvent(city);
        }
        DisplayInfos();
        Console.Write("Voulez vous continuer ?(y/n): ");
        string response = Console.ReadLine() ?? string.Empty;
        if (response == "n")
        {
            canRun = false;
        }
    }
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
    foreach (Effects effect in city.currentEvent!.effects!){

    switch (effect.typeRequested)
    {
        case "Gold":
        gold += effect.modificator;
        break;
        
        case "numberOfCivilian":
        city.numberOfCivilian += effect.modificator;
        break;

        case "LevelOfCulture":
        levelOfCulture += effect.modificator;
        break;

        case "numberOfScientist":
        city.numberOfScientist += effect.modificator;
        break;

        case "numberOfWorker":
        city.numberOfWorker += effect.modificator;
        break;

        case "City":
        if (effect.modificator == -1)
            {
                DestroyCity(city);
                return;
            }
        else if (effect.modificator == 1)
        {
            CreateNewCity();
        }   
        break;

        default:
        break;
    }
    }
}

void CreateNewCity()
{
    City newCity = new("Extalia",10,0,0, null);
    Cities.Add(newCity);
}

void DestroyCity(City city)
{
    Console.WriteLine($"{city.name} s'écroule !");
    Cities.Remove(city);
}

void DisplayInfos()
{
    Console.WriteLine("+------------------+");
    foreach (City city in Cities)
    {
        Console.WriteLine($"+---------{city.name}--------+");
        Console.WriteLine($"Nombre de citoyens : {city.numberOfCivilian}");
        Console.WriteLine($"Nombre de Scientifiques :{city.numberOfScientist}");
        Console.WriteLine($"Nombre de Workers :{city.numberOfWorker}");
    }
    Console.WriteLine($"Gold actuel : {gold}");

    Console.WriteLine("+------------------+");

}
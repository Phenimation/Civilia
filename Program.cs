using System.Text.Json;

Random rng = new();

string civilisationName = "";

int gold =0;
float levelOfCulture =0;

List<City> Cities = new();

string json = File.ReadAllText("PossibleEvents.json");
List<Evenement> PossiblesEvents = JsonSerializer.Deserialize<List<Evenement>>(json)!;

json = File.ReadAllText("ChoosedEvents.json");
List<Evenement> choosedEvents = JsonSerializer.Deserialize<List<Evenement>>(json)!;

List<Evenement> eduInvest = choosedEvents.Where(e => e.Status == "investment" && e.subCategory == "education").ToList();
List<Evenement> immoInvest = choosedEvents.Where(e => e.Status == "investment" && e.subCategory == "immobilier").ToList();
List<Evenement> companyInvest = choosedEvents.Where(e => e.Status == "investment" && e.subCategory == "entreprises").ToList();

List<Evenement> workEvents = choosedEvents.Where(e => e.Status == "work").ToList();

List<Evenement> researchEvents = choosedEvents.Where(e => e.Status == "research").ToList();

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
            ExecuteEvent(city, city.currentEvent);
            DisplayInfos(city);
            Invest(city);
        }


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

void ExecuteEvent(City city, Evenement eventToExecute)
{
    foreach (Effects effect in eventToExecute.effects!){

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
                if (Cities.Count == 0)
                {
                    Console.WriteLine("Toutes vos villes sont détruites, vous avez perdu !");
                    canRun = false;
                }
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

void Invest(City city)
{
    Console.WriteLine("Vous pouvez : ");
    Console.WriteLine("1-Investir du Gold");
    Console.WriteLine("2-Faire travaillez vos Ouvriers");
    Console.WriteLine("3-Lancer des recherches scientifiques");
    Console.WriteLine("4-Ne rien faire");
    Console.Write("Que voulez vous faire ? (1,2,3 ou 4):");
    string response = Console.ReadLine() ?? string.Empty;
    Console.WriteLine("+-----------------+");
    switch (response)
    {
        case "1":
        Console.WriteLine("Dans quoi voulez vous investir ?:");
        Console.WriteLine("1-L'éducation");
        Console.WriteLine("2-L'immobilier");
        Console.WriteLine("3-Les entreprises");
        string res = Console.ReadLine() ?? string.Empty;
        switch(res)
        {
            case "1":
            Evenement choosedEventEdu = eduInvest[rng.Next(0,eduInvest.Count)];
            ExecuteEvent(city, choosedEventEdu);
            break;

            case "2":
            Evenement choosedEventImo = immoInvest[rng.Next(0,immoInvest.Count)];
            ExecuteEvent(city, choosedEventImo);
            break;

            case "3":
            Evenement choosedEventCompany = companyInvest[rng.Next(0,companyInvest.Count)];
            ExecuteEvent(city, choosedEventCompany);
            break;

            default:
            break;
        }
        break;

        case "2":
        Evenement choosedEvent = workEvents[rng.Next(0,workEvents.Count)];
        ExecuteEvent(city, choosedEvent);
        break;

        case "3":
        Evenement choosedEventScience = researchEvents[rng.Next(0,researchEvents.Count)];
        ExecuteEvent(city, choosedEventScience);
        break;

        case"4":
        default:
        break;
    }
}

void DisplayInfos(City city)
{
    Console.WriteLine("+------------------+");
    
    Console.WriteLine($"+---------{city.name}--------+");
    Console.WriteLine($"Nombre de citoyens : {city.numberOfCivilian}");
    Console.WriteLine($"Nombre de Scientifiques :{city.numberOfScientist}");
    Console.WriteLine($"Nombre de Workers :{city.numberOfWorker}");
    
    Console.WriteLine($"Gold actuel : {gold}");

    Console.WriteLine("+------------------+");

}
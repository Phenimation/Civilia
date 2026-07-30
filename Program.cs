using System.Runtime.Versioning;
using System.Text.Json;

Random rng = new();

string json = File.ReadAllText("Resources/PossibleEvents.json");
List<Evenement> PossiblesEvents = JsonSerializer.Deserialize<List<Evenement>>(json)!;

json = File.ReadAllText("Resources/ChoosedEvents.json");
List<Evenement> choosedEvents = JsonSerializer.Deserialize<List<Evenement>>(json)!;

List<Evenement> eduInvest = choosedEvents.Where(e => e.Status == "investment" && e.subCategory == "education").ToList();
List<Evenement> immoInvest = choosedEvents.Where(e => e.Status == "investment" && e.subCategory == "immobilier").ToList();
List<Evenement> companyInvest = choosedEvents.Where(e => e.Status == "investment" && e.subCategory == "entreprises").ToList();

List<Evenement> workEvents = choosedEvents.Where(e => e.Status == "work").ToList();
List<Evenement> researchEvents = choosedEvents.Where(e => e.Status == "research").ToList();

List<City> Cities = new();
List<City> citiesToDestroy = new();

int maxEvent = PossiblesEvents.Count;

bool canRun = true;
bool gameOver = false;

int createNewCityNumber = 0;

string civilisationName = "";
int gold =0;
float levelOfCulture =0;

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
            DisplayInfos(city);
            city.currentEvent = PickRandomEvent();
            Invest(city);
            ExecuteEventsCity(city);

            if (gameOver)
            {
                Console.WriteLine("Toutes vos villes sont détruites, vous avez perdu !");
                return;
            }
        }
        if (createNewCityNumber>0)
        {
            for (int i = 0; i < createNewCityNumber; i++)
            {
                CreateNewCity();
            }
        }
        if (citiesToDestroy.Count>0)
        {
            foreach (City city in citiesToDestroy)
            {
                DestroyCity(city);
            }
            if (Cities.Count == 0)
            {
                canRun = false;
                gameOver = true;
            }
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

void ExecuteEventsCity(City city)
{
    List<Evenement> eventsToExecute = [city.currentChoosedEvent!,city.currentEvent!];

    foreach (Evenement even in eventsToExecute){
        if (even == null){continue;}

        Console.WriteLine("---------------------------");
        if (even == city.currentChoosedEvent)
        {
            Console.WriteLine($"Conséquence de vos actions : {even.Description}");
        }
        else
        {
            Console.WriteLine($"Evenement aléatoire : {even.Description}");
        }
        Console.WriteLine("---------------------------");

        foreach (Effects effect in even.effects!){

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
                        citiesToDestroy.Add(city);
                        break;
                    }
                else if (effect.modificator == 1)
                {
                    createNewCityNumber ++;
                }   
                break;

                default:
                break;
            }
        }
    }
}

void CreateNewCity()
{
    Console.Write("Quel nom voulez vous donner à cette nouvelle ville ?: ");
    string name = Console.ReadLine() ?? string.Empty;
    City newCity = new(name,10,0,0, null);
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
    Console.WriteLine("5-Quitter le jeu");
    Console.Write("Que voulez vous faire ? (1,2,3,4 ou 5):");
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
            city.currentChoosedEvent = choosedEventEdu;
            break;

            case "2":
            Evenement choosedEventImo = immoInvest[rng.Next(0,immoInvest.Count)];
            city.currentChoosedEvent = choosedEventImo;
            break;

            case "3":
            Evenement choosedEventCompany = companyInvest[rng.Next(0,companyInvest.Count)];
            city.currentChoosedEvent = choosedEventCompany;
            break;

            default:
            break;
        }
        break;

        case "2":
        Evenement choosedEvent = workEvents[rng.Next(0,workEvents.Count)];
        city.currentChoosedEvent = choosedEvent;
        break;

        case "3":
        Evenement choosedEventScience = researchEvents[rng.Next(0,researchEvents.Count)];
        city.currentChoosedEvent = choosedEventScience;
        break;

        case"4":
        default:
        break;

        case "5":
        canRun = false;
        break;
    }
}

void DisplayInfos(City city)
{   
    Console.WriteLine($"+---------{city.name}--------+");
    Console.WriteLine($"Nombre de citoyens : {city.numberOfCivilian}");
    Console.WriteLine($"Nombre de Scientifiques :{city.numberOfScientist}");
    Console.WriteLine($"Nombre de Workers :{city.numberOfWorker}");
    
    Console.WriteLine($"Gold actuel : {gold}");
    Console.WriteLine($"Niveau de Culture : {levelOfCulture}");

    Console.WriteLine("+------------------+");
}

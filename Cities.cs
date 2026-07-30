public class City
{
    public string? name{get;set;}
    public int numberOfCivilian {get;set;}
    public int numberOfWorker {get;set;}
    public int numberOfScientist {get;set;}
    public Evenement? currentEvent {get;set;}
    public Evenement? currentChoosedEvent {get;set;}

    public City(string? name, int civilian, int worker, int scientist, Evenement? currentEvent)
    {
        this.name = name;
        this.numberOfCivilian = civilian;
        this.numberOfWorker = worker;
        this.numberOfScientist = scientist;
        this.currentEvent = currentEvent;
    }
}
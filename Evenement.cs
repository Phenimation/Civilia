public class Evenement
{
    public int Id{get;set;}
    public string? Description{get;set;}
    public string? Status{get;set;}

    public List<Effects>? effects{get;set;}
}
public class Effects
    {
        public string? typeRequested{get;set;}
        public int modificator {get;set;}
        
    }
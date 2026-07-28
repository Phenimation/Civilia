public class Evenement
{
    public int Id{get;set;}
    public string? Description{get;set;}
    public string? Status{get;set;}

    public Effects? effects{get;set;}
    public class Effects
    {
        public string? TypeRequested {get;set;}
        public int Modificator {get;set;}
    }
}
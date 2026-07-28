string civilisationName = "";

Game();

void Game()
{
    civilisationName = SelectCivilisationName();
}

string SelectCivilisationName()
{
    string nameChoosed = "";

    Console.Write("Choissisez le nom de votre Civilisation : ");
    nameChoosed = Console.ReadLine() ?? string.Empty;

    return nameChoosed;
}
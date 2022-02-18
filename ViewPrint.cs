namespace CSGO_v2;

public class ViewPrint
{
    private string _message;


    public void PrintCenter(string message)
    {
        Console.WriteLine("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message);
    }
}
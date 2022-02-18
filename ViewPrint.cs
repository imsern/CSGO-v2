namespace CSGO_v2;

public class ViewPrint
{
    private string _message;


    public void PrintCenter(string message)
    {
        Console.WriteLine("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message);
    }
    
    
    public void PrintPlayerInfo(string message)
    {
        var StringToCenterLength = message.Length;
        var titleString = "             NAME            WEAPON      ARMOR    HEALTH   MONEY                         MONEY   HEALTH    ARMOR      WEAPON             NAME            ";
        var titleLength = titleString.Length;
        var centeredString = message.PadLeft(((titleLength - StringToCenterLength) / 2) + StringToCenterLength).PadRight(titleLength);
        Console.WriteLine(centeredString);
    }
    
}
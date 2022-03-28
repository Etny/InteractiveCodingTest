
using System.IO;

namespace DynamicCheck.Security;
internal class ConsolePasswordProvider : IPasswordProvider {

    private string _password;
    
    private readonly MessageWriter _io;

    public ConsolePasswordProvider(MessageWriter io)
    {
        _io = io;
    }

    public void ObtainPassword() {
        Console.Clear();

        _io.WriteFormatted("Have and <DarkBlue>Admin</> enter the verification password: ");

        var password = string.Empty;
        ConsoleKey key;
        do {
            var key_info = Console.ReadKey(true);
            key = key_info.Key;

            if(key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if(!char.IsControl(key_info.KeyChar) && key != ConsoleKey.Enter) 
            {
                password += key_info.KeyChar;
                Console.Write("*");
            }

        } while(key != ConsoleKey.Enter);

        _password = password;
    }

    public string GetPassword() => _password;

}

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DynamicCheck.Security;
internal class PasswordResultSigner {


    private readonly IPasswordProvider _password;
    public PasswordResultSigner(IPasswordProvider password)
    {
        _password = password;
    }

    public void SignResult() {
        
        using var sig_file = File.Create("./Results.cert");
        using var sig_writer = new StreamWriter(sig_file);
        sig_writer.Write(GetSignature());
    }

    public string GetSignature() {
        var password = _password.GetPassword();

        using var hasher = SHA1.Create();
        var data = File.ReadAllText("./Results.txt");
        var data_bytes = Encoding.UTF8.GetBytes(data + password);
        var hash = hasher.ComputeHash(data_bytes);

        return Convert.ToBase64String(hash);
    }
}
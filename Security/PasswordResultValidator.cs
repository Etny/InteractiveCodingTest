
using System.IO;

namespace DynamicCheck.Security;
internal class PasswordResultValidator {

    private readonly PasswordResultSigner _signer;
    private readonly MessageWriter _io;

    public PasswordResultValidator(PasswordResultSigner signer, MessageWriter io)
    {
        _signer = signer;
        _io = io;
    }

    public void ShowValidation() {
        var valid = Validate();

        Console.Clear();

        if(valid) {
            _io.WriteFormatted("Result.txt is <Green>Valid!</>");
        } else {
            _io.WriteFormatted("Results could <Red>NOT</> be valided. Double-check password.");
        }
    
        MessageWriter.WaitForEnter();
    }

    public bool Validate() {
        var sig = _signer.GetSignature();
        var cert = File.ReadAllText("./Results.cert");
        return cert == sig;
    }
}
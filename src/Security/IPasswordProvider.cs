
namespace DynamicCheck.Security;
internal interface IPasswordProvider {
    void ObtainPassword();
    string GetPassword();
}
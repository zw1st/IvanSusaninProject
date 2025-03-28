
namespace IvanSusaninProject_Contracts.DataModels;

public class GuarantorDataModel(string id, string login, string password, string email)
{
    public string Id { get; private set; } = id;

    public string Login { get; private set; } = login;

    public string Password { get; private set; } = password;

    public string Email { get; private set; } = email;
}
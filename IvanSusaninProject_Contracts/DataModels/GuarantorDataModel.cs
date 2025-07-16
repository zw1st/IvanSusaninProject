
using IvanSusaninProject_Contracts.Exceptions;
using IvanSusaninProject_Contracts.Extentions;
using IvanSusaninProject_Contracts.Infrastructure;
using System.Text.RegularExpressions;

namespace IvanSusaninProject_Contracts.DataModels;

public class GuarantorDataModel(string id, string login, string password, string email) : IValidation
{
    public string Id { get; private set; } = id;

    public string Login { get; private set; } = login;

    public string Password { get; private set; } = password;

    public string Email { get; private set; } = email;

    public void IValidate()
    {
        if (!Id.IsGuid())
            throw new ValidationException("This Id is not unique");

        if (Id.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Login.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Password.IsEmpty())
            throw new ValidationException("This field is empty");

        if (Email.IsEmpty())
            throw new ValidationException("This field is empty");

        if (!Regex.IsMatch(Email, "^\\S+@\\S+\\.\\S+$"))
            throw new ValidationException("This field is not email");

        if (!Regex.IsMatch(Login, "^[a-zA-Z0-9]{3,10}$"))
            throw new ValidationException("This field is not login");

        if (!Regex.IsMatch(Password, "^[a-zA-Z0-9]{8}$"))
            throw new ValidationException("This field is not password");

    }
}
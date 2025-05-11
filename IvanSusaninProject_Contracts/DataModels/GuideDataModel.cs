
namespace IvanSusaninProject_Contracts.DataModels;

public class GuideDataModel(string id, string fio, int experience, int age, string guaranderId)
{
    public string Id { get; private set; } = id;

    public string Fio { get; private set;} = fio;

    public int Experience { get; private set;} = experience;

    public int Age { get; private set;} = age;

    public string GuarandorId { get; private set;} = guaranderId;
}
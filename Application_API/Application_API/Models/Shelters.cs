using Application_API.Models.Enums;

namespace Application_API.Models;

public class Shelters
{
    public Guid Id { get; set; }
    public string ShelterName { get; set; } = String.Empty;
    public string ShelterDescription { get; set; } = String.Empty;
    public Cities ShelterLocation { get; set; }
    public int Capacity { get; set; }
    public string ShelterAddress { get; set; } = String.Empty;
    public ICollection<Animals> Animals { get; set; } = new List<Animals>();

    public Shelters(string shelterName, string shelterDescription, Cities shelterLocation, int capacity,
        string shelterAddress)
    {
        Id = Guid.NewGuid();
        ShelterName = shelterName;
        ShelterLocation = shelterLocation;
        ShelterDescription = shelterDescription;
        Capacity = capacity;
        ShelterAddress = shelterAddress;
    }
}
using Application_API.Models.Enums;

namespace Application_API.Models;

public class Animals
{
    public Guid Id { get; set; }
    public string Type { get; set; } = String.Empty;
    public string Breed { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public int Age { get; set; }
    public Sizes Size { get; set; }
    public string Health { get; set; } = String.Empty;
    public AnimalGenders Gender { get; set; }
    public GoodWith GoodWith { get; set; }
    public CoatTypes CoatLength { get; }
    public bool SpecialTrained { get; set; }
    
    public Guid ShelterId { get; set; }
    public Shelters Shelter { get; set; }

    public Animals(string type, string breed, string name, int age, Sizes size, string health,
        AnimalGenders gender,
        GoodWith goodWith, CoatTypes coatLength, bool specialTrained)
    {
        Id = Guid.NewGuid();
        Type = type;
        Breed = breed;
        Name = name;
        Age = age;
        Size = size;
        Health = health;
        Gender = gender;
        GoodWith = goodWith;
        CoatLength = coatLength;
        SpecialTrained = specialTrained;
    }
}
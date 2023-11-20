namespace Application_API.Models;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string FullName { get; set; }
    public string IdDetails { get; set; }
    public Guid AnimalId { get; set; }
    public Animals Animal { get; set; }

    public Appointment(DateTime appointmentDate, string fullName, string idDetails, Guid animalId)
    {
        Id = Guid.NewGuid();
        AppointmentDate = appointmentDate;
        FullName = fullName;
        IdDetails = idDetails;
        AnimalId = animalId;
    }
}
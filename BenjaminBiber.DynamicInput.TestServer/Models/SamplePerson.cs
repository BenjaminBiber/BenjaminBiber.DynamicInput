using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BenjaminBiber.DynamicInput;

namespace BenjaminBiber.DynamicInput.TestServer.Models;

public class SamplePerson
{
    [Display(Name = "Vorname")]
    [DynamicInputHeading("Persönliche Daten")]
    public string? FirstName { get; set; }

    [Display(Name = "Nachname")]
    [DynamicInputHeading("Persönliche Daten")]
    public string? LastName { get; set; }

    [Display(Name = "Alter")]
    [DynamicInputHeading("Persönliche Daten")]
    public int Age { get; set; }

    [Display(Name = "Aktiv")]
    [DynamicInputHeading("Persönliche Daten")]
    [DynamicInputBoolean(DynamicInputBooleanDisplay.Switch)]
    public bool IsActive { get; set; }

    [Display(Name = "Newsletter erhalten?")]
    [DynamicInputHeading("Persönliche Daten")]
    public bool ReceivesNewsletter { get; set; }

    [Display(Name = "Geburtstag")]
    [DynamicInputHeading("Persönliche Daten")]
    public DateTime? Birthday { get; set; }

    [Display(Name = "Monatsgehalt"), DataType(DataType.Currency)]
    [DynamicInputClass("w-100", "max-width-300")]
    [DynamicInputHeading("Beschäftigung")]
    public decimal Salary { get; set; }

    [Display(Name = "Status")]
    [DynamicInputOptions(nameof(GetStatusOptionsAsync))]
    [DynamicInputHeading("Beschäftigung")]
    public string? Status { get; set; }

    [DynamicInputIgnore]
    [Display(Name = "Offene Aufgaben")]
    public int? OpenTasks { get; set; }
    private Task<IDictionary<string, object?>> GetStatusOptionsAsync()
    {
        IDictionary<string, object?> options = new Dictionary<string, object?>
        {
            ["Aktiv"] = "active",
            ["Pausiert"] = "paused",
            ["Archiviert"] = "archived"
        };

        return Task.FromResult(options);
    }
}

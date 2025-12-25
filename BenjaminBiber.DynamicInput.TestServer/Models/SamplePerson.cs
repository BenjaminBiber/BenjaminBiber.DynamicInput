using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BenjaminBiber.DynamicInput;

namespace BenjaminBiber.DynamicInput.TestServer.Models;

public class SamplePerson
{
    [Display(Name = "Vorname")]
    public string? FirstName { get; set; }

    [Display(Name = "Nachname")]
    public string? LastName { get; set; }

    [Display(Name = "Alter")]
    public int Age { get; set; }

    [Display(Name = "Aktiv")]
    public bool IsActive { get; set; }

    [Display(Name = "Geburtstag")]
    public DateTime? Birthday { get; set; }

    [Display(Name = "Monatsgehalt"), DataType(DataType.Currency)]
    [DynamicInputClass("w-100", "max-width-300")]
    public decimal Salary { get; set; }

    [Display(Name = "Status")]
    [DynamicInputOptions(nameof(GetStatusOptionsAsync))]
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

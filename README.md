# BenjaminBiber.DynamicInput

Eine kleine Razor-Class-Library, die mithilfe von MudBlazor Formulare automatisch aus beliebigen Objektinstanzen erzeugt. Die Komponente `DynamicObjectEditor` liest per Reflexion alle öffentlichen, schreibbaren Properties und rendert passende Eingaben (Text, Switch, Numeric, DateTime sowie Selects über Optionsattribute). Änderungen können optional gepuffert und erst über einen Submit-Button übernommen werden.

## Installation

```bash
# innerhalb deines Blazor-Projekts
Dotnet add package BenjaminBiber.DynamicInput
```

> Tipp: Achte darauf, dass dein Host-Projekt MudBlazor korrekt initialisiert (CSS/JS + `MudThemeProvider`, `MudDialogProvider`, `MudPopoverProvider`, `MudSnackbarProvider`).

## Verwendung

1. Referenz auf die Bibliothek setzen (`@using BenjaminBiber.DynamicInput` in `_Imports.razor`).
2. Ein Modell übergeben, dessen öffentliche Properties automatisch editiert werden sollen.
3. `DynamicObjectEditor` einbinden:

```razor
<DynamicObjectEditor Model="@Person"
                      ModelChanged="OnPersonChanged"
                      RequireSubmit="true"
                      SubmitButtonText="Änderungen anwenden"
                      WrapperClass="pa-4"
                      InputClass="w-100" />
```

### Parameter

`Model` | Objekt, das reflektiert wird (Pflicht). Änderungen werden direkt auf dem Instanzobjekt vorgenommen.

`ModelChanged` | Wird aufgerufen, wenn Werte direkt geschrieben oder nach Submit übernommen wurden.

`RequireSubmit` | Steuert, ob Eingaben sofort geschrieben (`false`) oder in `_pendingValues` gehalten und per Button übernommen werden (`true`).

`SubmitButtonText` | Beschriftung des Buttons im Submit-Modus.

`WrapperClass` | Zusätzliche CSS-Klassen für das umschließende `<div class="dynamic-object-editor pa-4">`.

`InputClass` | Globale CSS-Klassen, die auf jedes erzeugte Mud-Input angewendet werden (lassen sich pro Property ergänzen, siehe unten).

## Property-Attribute

| Attribut | Zweck |
|---|---|
`[DynamicInputIgnore]` | Property wird komplett ausgeblendet (z. B. IDs, Computed Values).
`[DynamicInputClass("w-100", "max-width-300")]` | Ergänzt pro Property zusätzliche CSS-Klassen zum Input. Mehrere Attribute oder mehrere Klassen pro Attribut möglich.
`[DynamicInputOptions(nameof(GetStatusOptionsAsync))]` | Markiert ein Feld als Select. Die angegebene parameterlose Instanzmethode liefert die Optionen.

### Options-Provider

Die Optionsmethode darf die folgenden Rückgabetypen (auch als `Task<>`) liefern:

- `IDictionary<string, object?>`
- `IEnumerable<KeyValuePair<string, object?>>`
- `IEnumerable<DynamicInputOption>` oder `IReadOnlyList<DynamicInputOption>`

```csharp
public class SamplePerson
{
    [DynamicInputOptions(nameof(GetStatusOptionsAsync))]
    public string? Status { get; set; }

    private Task<IDictionary<string, object?>> GetStatusOptionsAsync()
        => Task.FromResult<IDictionary<string, object?>>(new Dictionary<string, object?>
        {
            ["Aktiv"] = "active",
            ["Pausiert"] = "paused",
            ["Archiviert"] = "archived"
        });
}
```

## Selects & andere Typen

`DynamicObjectEditor` erkennt aktuell folgende Typen und rendert passende MudBlazor-Komponenten:

- `string`, `Guid` → `MudTextField`
- `bool` → `MudSwitch`
- Zahlen (alle gängigen `int`, `long`, `decimal`, `float`, `double` inkl. Nullable) → `MudNumericField`
- `DateTime` → `MudDatePicker`
- Properties mit `[DynamicInputOptions]` → `MudSelect`

Nicht unterstützte Typen werden als `MudAlert` angezeigt, damit du sofort weißt, was noch fehlt.

## Deferred Editing

Wenn `RequireSubmit` auf `true` steht, werden Änderungen zunächst in einer internen `Dictionary<string, object?>` gehalten. Erst der Submit-Button schreibt die Werte ins Modell und löst optional `ModelChanged` aus. Das erlaubt z. B. Review- oder Wizard-Szenarien.

## Beispielprojekt

Im Repo befindet sich `BenjaminBiber.DynamicInput.TestServer`, eine Blazor-Server-App, die sämtliche Features demonstriert (Sofort- vs. Submit-Modus, `[DynamicInputIgnore]`, `[DynamicInputClass]`, Selects). Starte sie mit:

```bash
dotnet run --project BenjaminBiber.DynamicInput.TestServer
```

## Roadmap / Ideen

- Eigene Templates für Layout/Labels
- Unterstützung weiterer Typen (z. B. `TimeOnly`, `DateOnly`, File Uploads)
- Validierungsausgabe über `EditContext`

Beiträge und Vorschläge sind willkommen!

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
                      InputClass="w-100"
                      BooleanDisplay="DynamicInputBooleanDisplay.Checkbox"
                      SubmitButtonClass="mt-4" />
```

### Parameter

| Parameter | Beschreibung |
|---|---|
| `Model` | Objekt, das reflektiert wird (Pflicht). Änderungen werden direkt auf dem Instanzobjekt vorgenommen. |
| `ModelChanged` | Wird aufgerufen, wenn Werte sofort geschrieben oder nach Submit übernommen wurden. |
| `RequireSubmit` | `true` hält Werte zunächst in `_pendingValues` und übernimmt sie erst über den Button, `false` schreibt sofort ins Modell. |
| `SubmitButtonText` | Beschriftung des Buttons im Submit-Modus (Standard: „Änderungen übernehmen“). |
| `WrapperClass` | Zusätzliche CSS-Klassen für das umschließende `<div class="dynamic-object-editor pa-4">`. |
| `InputClass` | Globale CSS-Klassen, die auf jedes erzeugte Mud-Input angewendet werden (lassen sich pro Property ergänzen, siehe unten). |
| `InputColor` | Optionaler `MudBlazor.Color`, der auf Inputs mit `Color`-Parameter angewendet wird (z. B. Switch, Checkbox, Slider, DatePicker). |
| `InputVariant` | Optionaler `MudBlazor.Variant`, der auf Inputs mit `Variant`-Parameter angewendet wird (z. B. Text-/NumericFields, Selects, DatePicker). |
| `BooleanDisplay` | Standard-Darstellung für boolsche Werte: `Switch` (Default) oder `Checkbox`. Einzelne Properties können dies via `[DynamicInputBoolean]` überschreiben. |
| `SubmitButtonClass` | Ermöglicht es, zusätzliche Klassen auf den Submit-Button zu legen (z. B. Breite, Ausrichtung). |

## Property-Attribute

| Attribut | Zweck |
|---|---|
`[DynamicInputIgnore]` | Property wird komplett ausgeblendet (z. B. IDs, Computed Values).
`[DynamicInputClass("w-100", "max-width-300")]` | Ergänzt pro Property zusätzliche CSS-Klassen zum Input. Mehrere Attribute oder mehrere Klassen pro Attribut möglich.
`[DynamicInputOptions(nameof(GetStatusOptionsAsync))]` | Markiert ein Feld als Select. Die angegebene parameterlose Instanzmethode liefert die Optionen.
`[DynamicInputHeading("Beschäftigung")]` | Rendert eine MudText-Überschrift vor dem Property und gruppiert direkt folgende Properties mit derselben Überschrift unter diesem Block.
`[DynamicInputBoolean(DynamicInputBooleanDisplay.Checkbox)]` | Erzwingt für ein boolsches Feld eine Checkbox (oder Switch) – unabhängig vom globalen `BooleanDisplay`-Parameter.
`[DynamicInputInteger(0, 100, displayAsSlider: true)]` | Definiert Mindest- und Höchstwert für `int`/`int?` und kann zusätzlich eine Slider-Darstellung erzwingen (Slider nur für nicht-nullable `int`).
`[DynamicInputDisabled(nameof(IsNameLocked))]` | Ruft eine parameterlose Instanzmethode auf, die `bool` zurückgibt; `true` deaktiviert das jeweilige Input (z. B. für ReadOnly- oder Feature-Flags).

> Tipp: Platziere Eigenschaften, die dieselbe Überschrift erhalten sollen, unmittelbar hintereinander – so entsteht ein sauberer Abschnitt ohne doppelte Headings.

> Hinweis: `BooleanDisplay="DynamicInputBooleanDisplay.Checkbox"` macht Checkboxen zur Voreinstellung – einzelne Felder kannst du mit `[DynamicInputBoolean(DynamicInputBooleanDisplay.Switch)]` weiter als Switch darstellen.

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
- `bool` → `MudSwitch` oder `MudCheckBox` (steuerbar via `BooleanDisplay` bzw. `[DynamicInputBoolean]`)
- Zahlen (alle gängigen `int`, `long`, `decimal`, `float`, `double` inkl. Nullable) → `MudNumericField`; für `int`/`int?` lassen sich Min/Max + Slider über `[DynamicInputInteger]` festlegen
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

# DisplayModeMatrix

DisplayModeMatrix is used to extend the compoundability of a single dimension ASP.NET MVC Display Modes.

## Example

### Multiple optional dimensions

- **Device**, optional :  
  Mobile | Tablet | Default (empty suffix)

- **Theme**, optional :  
  Dark | Default (empty suffix)

- **Preview**, optional :  
  Preview | No Preview (empty suffix)

### Expected combination and sequence

- Mobile-Dark-Preview
- Tablet-Dark-Preview
- Mobile-Dark
- Tablet-Dark
- Mobile-Preview
- Tablet-Preview
- Dark-Preview
- Dark-Preview
- Mobile
- Tablet
- Dark
- Preview

### Use the DisplayModeMatrixBuilder to create a list of Display Modes

```csharp
var builder = new DisplayModeMatrixBuilder();

var matrix = builder
                .AddOptionalLayer("Device", l => l.Suffix("Mobile", x => IsMobile(x)).Suffix("Tablet", x => IsTablet(x)))
                .AddOptionalLayer("Theme", l => l.Suffix("Dark", x => CurrentTheme(x) == "dark"))
                .AddOptionalLayer("Preview", l => l.Suffix("Preview", x => IsPreview(x)))
                .Build();
```

builder.Build() produces an `IEnumerable <NamedCondition>` object that forms a result list for the display mode that can be used to generate Display Modes for ASP.NET MVC. 

Please see DisplayModeMatrix.Web in [~/App_Start/DisplayModeConfig.cs](DisplayModeMatrix.Web/App_Start/DisplayModeConfig.cs)

```csharp
foreach (var profile in matrix)
{
    instance.Modes.Add(new DefaultDisplayMode(profile.Name)
    {
        ContextCondition = x => profile.ContextCondition(x)
    });
}
```

## TODO:

- Performance impact comparing test. (DisplayModeMatrixBuilder | Manual | Without Display Modes)
- Support ASP.NET Core 

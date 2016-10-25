# DisplayModeMatrix

DisplayModeMatrix is used to extend the compoundability of a single dimension ASP.NET MVC Display Modes.

## Example

### Given a multiple optional factors, every factor has possible values

- **Device**, optional :  
  Mobile | Tablet | Default (empty suffix)

- **Theme**, optional :  
  Dark | Default (empty suffix)

- **Preview**, optional :  
  Preview | No Preview (empty suffix)

### Expected factor combination and sequencing result

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

The string of combination used to match Display Modes mechanism.

### Use the DisplayModeMatrixBuilder to create a list of Display Modes

```csharp
var builder = new DisplayModeMatrixBuilder();

var matrix = builder
                .AddOptionalLayer("Device", l => l.Suffix("Mobile", x => IsMobile(x)).Suffix("Tablet", x => IsTablet(x)))
                .AddOptionalLayer("Theme", l => l.Suffix("Dark", x => CurrentTheme(x) == "dark"))
                .AddOptionalLayer("Preview", l => l.Suffix("Preview", x => IsPreview(x)))
                .Build();
```

builder.Build() produces an computed `IEnumerable<DisplayModeProfile>` collection can be used to generate Display Modes for ASP.NET MVC. 

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

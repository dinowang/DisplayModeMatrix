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
- Mobile
- Tablet
- Dark
- Preview

The string of combination used to match Display Modes mechanism.

### Use the DisplayModeMatrixBuilder to create a list of Display Modes

```csharp
var builder = new DisplayModeMatrixBuilder();

var matrix = builder
                .AddOptionalFactor("Device", l => l.Evidence("Mobile", x => IsMobile(x)).Evidence("Tablet", x => IsTablet(x)))
                .AddOptionalFactor("Theme", l => l.Evidence("Dark", x => CurrentTheme(x) == "dark"))
                .AddOptionalFactor("Preview", l => l.Evidence("Preview", x => IsPreview(x)))
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

### Benchmarking

|                     | Apply DisplayModeMatrixBuilder |     Without Display Modes     |
|---------------------|--------------------------------|-------------------------------|
| SuperBenchmarker    | TPS: 133.2 (requests/second)   | TPS: 135.3 (requests/second)  |
| -n 1000 -c 10       | Max: 6018.8998ms               | Max: 6160.7143ms              |
|                     | Min: 2.4041ms                  | Min: 2.3731ms                 |
|                     | Avg: 67.8596257ms              | Avg: 67.777162ms              |

## TODO:

- Performance impact comparing test. (DisplayModeMatrixBuilder | Manual | Without Display Modes)
- Support ASP.NET Core 

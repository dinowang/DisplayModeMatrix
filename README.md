# DisplayModeMatrix

提供單一維度 ASP.NET MVC Display Modes 下的複式組合可能性

## Example

### 多重可選維度

Device (optional) : Mobile | Tablet | Default (empty suffix)

Theme (optional) : Dark | Default (empty suffix)

Preview (optional) : Preview | No Preview (empty suffix)

### DisplayModeMatrixBuilder

```csharp
var builder = new DisplayModeMatrixBuilder();

var matrix = builder
                .AddOptionalLayer("Device", l => l.Suffix("Mobile", x => IsMobile(x)).Suffix("Tablet", x => IsTablet(x)))
                .AddOptionalLayer("Theme", l => l.Suffix("Dark", x => CurrentTheme(x) == "dark"))
                .AddOptionalLayer("Preview", l => l.Suffix("Preview", x => IsPreview(x)))
                .Build();
```

### 組合及順序

- Mobile-Dark-Preview
- Tablet-Dark-Preview
- Mobile-Dark
- Tablet-Dark
- Mobile-Preview
- Tablet-Preview
- Dark-Preview
- Dark-Preview
- Dark
- Preview


## TODO:

- Performance impact comparing test
- Support ASP.NET Core

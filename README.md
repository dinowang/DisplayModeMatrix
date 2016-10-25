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

builder.Build() 產生一個 IEnumerable<NamedCondition> 物件，為顯示模式組成結果表，可用以生成 Display Modes，於 ASP.NET MVC 中使用請詳見 DisplayModeMatrix.Web 中 ~/App_Start/DisplayModeConfig.cs

```csharp
foreach (var composition in matrix)
{
    instance.Modes.Add(new DefaultDisplayMode(composition.Name)
    {
        ContextCondition = x => composition.Expression.Compile().Invoke(x)
    });
}
```

### Display Modes 組合及順序

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


## TODO:

- Performance impact comparing test
- Support ASP.NET Core

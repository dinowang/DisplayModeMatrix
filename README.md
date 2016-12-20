# DisplayModeMatrix

*閱讀其他語言版本: [English](README.en-us.md), [正體中文](README.md).*

DisplayModeMatrix 用來擴展 ASP.NET MVC Display Modes 預設預設的單一維度，提供可延展的組合性。  

此機制由 Android 所啟發, 詳可參考 [How Android Finds the Best-matching Resource](https://developer.android.com/guide/topics/resources/providing-resources.html#BestMatch)  

有了多維度的 Display Modes，你可以：

- 提供很棒的視圖 A/B testing 機制
- 在多租戶應用程式中非常容易的方式提供客製化報表格式
- 多組 Display Modes 同時工作

## 基本想法

開發者運用 ASP.NET MVC Display Modes 將 View 在不同情境下分為多個版本。 
 
常見的例子是區分出桌面版與行動版的 View。  

當應用程式執行 View 之前我們希望根據情境顯示以下適合的 View。

```
Index.cshtml  
Index.Mobile.cshtml  
```

桌面版執行 Index.cshtml，行動版本執行 Index.Mobile.cshtml。

這需要以標準的 Display Modes 註冊程序，使得 ASP.NET MVC 能夠進行 View 的情境分離。

```
DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile")
{
    ContextCondition = (context => context.IsMobile())    
});
```

以上是簡要的 Display Modes 機制敘述，相當簡單易用。

如上，因為 Display Modes 預設為單一維度的設計，有時我們需要更加彈性的 Display Modes 組態方式，否則 Display Modes 組態方式會變得非常難以撰寫或維護。
有些解決方案或許可由實作 IDisplayMode 介面來達成，但實作起來稍微複雜。

我的想法是將 suffix 轉變為多段組合而成，並以 "-" 連結號串起。

```
Index.{Devices}-{Preview}.cshtml
```

每一個部分擁有獨立的 *符號* 以及 *運算式 (expression)* 構成

- `{Device}` 置換為 "Mobile", 如果 HttpContext.Current.IsMobile() 成立
- `{Preview}` 置換為 "Preview", 如果特定的 cookie 存在於請求標頭上  

任何一個部分可為選擇性存在，如果該部分沒有滿足情境，那就留白。

沒有意義的連接符號不會成為構成 suffix 的元素(頭、尾、重複的)。

然後以 Builder pattern 協助計算出 Display Modes 的組合性。

## 如何使用

### 假設三組可選擇性的維度，每一個維度以及可能的值如下表

|           維度          |                       可能值                       |
|-------------------------|---------------------------------------------------|
| **Device** (可選)       | *Mobile*, *Tablet*, *Default* (空值)               |
| **Theme** (可選)       | *Dark*, *Default* (空值)                            |
| **Preview** (可選)     | *Preview*, *No Preview* (空值)                      |

### 預期產生的 suffix 組合以及正確順序

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

這些 suffix 將應用於標準 Display Modes 的組態機制中.

### Views 的結構

有了多維度的可能性，現在你可以更加有彈性的 Display Modes 組織 View。

![Views structure](screenshot/views-structure.png)

### 使用 DisplayModeMatrixBuilder 建立一系列的 Display Modes

```csharp
var builder = new DisplayModeMatrixBuilder();

var matrix = builder
                .AddOptionalFactor("Device", l => l.Evidence("Mobile", x => IsMobile(x)).Evidence("Tablet", x => IsTablet(x)))
                .AddOptionalFactor("Theme", l => l.Evidence("Dark", x => CurrentTheme(x) == "dark"))
                .AddOptionalFactor("Preview", l => l.Evidence("Preview", x => IsPreview(x)))
                .Build();
```

builder.Build() 可生成一組 `IEnumerable<DisplayModeProfile>` 集合能用來註冊 Display Modes。 

註冊方式如下，完整的範例請參考 DisplayModeMatrix.Web 專案的 [~/App_Start/DisplayModeConfig.cs](DisplayModeMatrix.Web/App_Start/DisplayModeConfig.cs)

```csharp
foreach (var profile in matrix)
{
    instance.Modes.Add(new DefaultDisplayMode(profile.Name)
    {
        ContextCondition = x => profile.ContextCondition(x)
    });
}
```

### 效能測試

使用 [SuperBenchmarker](https://github.com/aliostad/SuperBenchmarker) (-n 1000 -c 10)

|                     | 運用 DisplayModeMatrix         |    完全不使用 Display Modes    |
|---------------------|--------------------------------|-------------------------------|
| TPS                 | 133.2 (requests/second)        | 135.3 (requests/second)       |
| Max                 | 6018.8998ms                    | 6160.7143ms                   |
| Min                 | 2.4041ms                       | 2.3731ms                      |
| Avg                 | 67.8596257ms                   | 67.777162ms                   |

using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace QuizApp.UI.JavaScriptInterop;

[SupportedOSPlatform("browser")]
public static partial class Interop
{
    [JSImport("ScrollTo", "Interop")]
    internal static partial string ScrollToElement(string elementId, int timeout);
}
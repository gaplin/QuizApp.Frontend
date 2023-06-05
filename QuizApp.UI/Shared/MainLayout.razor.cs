using System.Security.Claims;

namespace QuizApp.UI.Shared;

public partial class MainLayout
{
    private string UserDisplayName(List<Claim> claims)
    {
        return "Authorized";
    }
}

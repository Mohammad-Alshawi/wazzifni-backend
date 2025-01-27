using System.Threading.Tasks;
using Wazzifni.Configuration.Dto;

namespace Wazzifni.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}

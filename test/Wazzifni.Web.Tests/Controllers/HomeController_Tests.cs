using System.Threading.Tasks;
using Wazzifni.Models.TokenAuth;
using Wazzifni.Web.Controllers;
using Shouldly;
using Xunit;

namespace Wazzifni.Web.Tests.Controllers
{
    public class HomeController_Tests: WazzifniWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Integration.Tests;

[TestClass]
public class WebAppTests
{
    private HttpClient _c;

    [TestInitialize]
    public void Initialize()
    {
        var a = new WebApplicationFactory<Program>();
        _c = a.CreateClient();
    }

    [TestMethod]
    public async Task TestMethod1()
    {
        var resp = await _c.GetAsync("todos");
        Assert.IsNotNull(resp);
    }
}
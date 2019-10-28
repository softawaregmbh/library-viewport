using Microsoft.VisualStudio.TestTools.UnitTesting;
using softaware.ViewPort.Uwp.UserInteraction;
using Windows.ApplicationModel.Resources.Core;

namespace softaware.ViewPort.Uwp.Tests
{
    [TestClass]
    public class UwpUserInteractionProviderTests
    {
        [TestMethod]
        [DataRow("en", "OK", "OK")]
        [DataRow("en", "Cancel", "Cancel")]
        [DataRow("de", "OK", "OK")]
        [DataRow("de", "Cancel", "Abbrechen")]
        [DataRow("fr", "OK", "OK")] // no french translations yet, use default
        [DataRow("fr", "Cancel", "Cancel")]
        public void TestLocalization(string culture, string key, string expectedValue)
        {
            ResourceContext.SetGlobalQualifierValue("Language", culture);

            var provider = new UwpUserInteractionProvider();
            var resourceLoader = provider.GetResourceLoader();
            Assert.AreEqual(expectedValue, resourceLoader.GetString(key));
        }
    }
}

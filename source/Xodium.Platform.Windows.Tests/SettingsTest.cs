using Xodium.Platform.Windows.Services;
using Xunit;

namespace Xodium.Platform.Windows.Tests
{
    public class SettingsTest
    {
        const string nameOne = "TestKey1";
        const string nameTwo = "TestKey2";
        const string valueOne = "1";
        const string valueTwo = "2";

        private readonly SettingsService service;

        public SettingsTest()
        {
            service = new SettingsService(new FakeRegistryKey());
            service.Clear();
            service.SetValue(nameOne, valueOne);
            service.SetValue(nameTwo, valueTwo);
        }

        [Fact]
        public void CanGetValue()
        {
            var value = service.GetValue(nameOne, null);

            Assert.Equal(valueOne, value);
        }

        [Fact]
        public void CanSetValue()
        {
            service.SetValue(nameOne, valueTwo);
            var newValue = service.GetValue(nameOne, null);

            Assert.Equal(valueTwo, newValue);
        }

        [Fact]
        public void CanRemove()
        {
            service.Remove(nameOne);

            Assert.False(service.Contains(nameOne));
            Assert.True(service.Contains(nameTwo));
        }

        [Fact]
        public void CanClear()
        {
            service.Clear();

            Assert.False(service.Contains(nameOne));
            Assert.False(service.Contains(nameTwo));
        }

        [Fact]
        public void CanGetAndSetWithSection()
        {
            service.SetValue("nameThree", "valueThree", "sectionThree");
            var value = service.GetValue("nameThree", null, "sectionThree");

            Assert.Equal("valueThree", value);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using softaware.ViewPort.Core;

namespace softaware.ViewPort.Tests
{
    [TestClass]
    public class PropertyHelperTests
    {
        private class Test : NotifyPropertyChanged
        {
            private string name;
            private int value;

            public string Name
            {
                get => this.name;
                set => this.SetProperty(ref this.name, value);
            }

            public int Value
            {
                get => this.value;
                set => this.SetProperty(ref this.value, value);
            }
        }

        public int Value { get; set; }

        [TestMethod]
        public void TestGetPropertyNameOfSameClass()
        {
            Assert.AreEqual("Value", this.GetPropertyName(() => this.Value));
        }

        [TestMethod]
        public void TestGetPropertyNameOfOtherClass() 
        {
            Test test = new Test();
            Assert.AreEqual("Name", test.GetPropertyName(t => t.Name));
        }


        [TestMethod]
        public void TestExecuteOnPropertyChangedWithArguments()
        {
            Test test = new Test();

            int newValue = 0;
            var handler = test.ExecuteOnPropertyChanged(t => t.Value, v => newValue = v);
            test.Value = 1;
            Assert.AreEqual(1, newValue);

            test.PropertyChanged -= handler;

            test.Value = 2;
            Assert.AreEqual(1, newValue);
        }
    }
}

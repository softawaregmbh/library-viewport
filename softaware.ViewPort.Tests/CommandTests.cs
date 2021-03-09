using System;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using softaware.ViewPort.Commands;

namespace softaware.ViewPort.Tests
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void TestIntParameter()
        {
            Command<int> command = new Command<int>(i => Console.WriteLine(i));
            ((ICommand)command).Execute(1);
        }

        [TestMethod]
        public void TestNullableIntParameter()
        {
            Command<int?> command = new Command<int?>(i => Console.WriteLine(i));
            ((ICommand)command).Execute(null);
        }

        [TestMethod]
        public void TestWrongParameter()
        {
            Command<int> command = new Command<int>(i => Console.WriteLine(i));
            Assert.ThrowsException<ArgumentException>(() => ((ICommand)command).Execute("Test"));
        }
    }
}

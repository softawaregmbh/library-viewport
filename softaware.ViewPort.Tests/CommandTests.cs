﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using softaware.ViewPort.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace softaware.ViewPort.Tests
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void TestIntParameter()
        {
            Command<int?> command = new Command<int?>(i => Console.WriteLine(i));
            ((ICommand)command).Execute(null);
        }

        [TestMethod]
        public async Task TestExceptionInAsyncCommand() 
        {
            AsyncCommand command = new AsyncCommand(this.ThrowAsync);
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await command.ExecuteAsync());
        }

        private async Task ThrowAsync() 
        {
            await Task.Delay(1);
            throw new InvalidOperationException();
        }
    }
}
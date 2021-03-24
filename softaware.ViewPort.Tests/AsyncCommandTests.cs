using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using softaware.ViewPort.Commands;

namespace softaware.ViewPort.Tests
{
    [TestClass]
    public class AsyncCommandTests
    {
        [TestMethod]
        public async Task TestExceptionInAsyncCommand()
        {
            var command = new AsyncCommand(this.ThrowAsync);
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await command.ExecuteAsync());
        }

        private async Task ThrowAsync()
        {
            await Task.Delay(1);
            throw new InvalidOperationException();
        }
    }
}

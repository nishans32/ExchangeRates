using System;
using Xunit;
using DockerExample.App.Services;

namespace DockerExample.Test
{
    public class MessagingServiceTests
    {
        const string EXPECTED_MESSAGE = "Hello World";
        
        [Fact]
        public void GetMessage_ReturnsMessage()
        {
            //Arrange
            var service = new MessagingService();

            //Act
            var result = service.GetMessage();

            //Assert
            Assert.Equal(EXPECTED_MESSAGE, result);
        }
    }
}

using System.Text.Json;
using FluentAssertions;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using Xunit;

namespace Lsp.Tests.Capabilities.Client
{
    public class DynamicCapabilityTests
    {
        [Theory, JsonFixture]
        public void SimpleTest(string expected)
        {
            var model = new DynamicCapability() { DynamicRegistration = false };
            var result = Fixture.SerializeObject(model);

            result.Should().Be(expected);

            var deresult = JsonSerializer.Deserialize<DynamicCapability>(expected, Serializer.Instance.Options);
            deresult.Should().BeEquivalentTo(model);
        }
    }
}

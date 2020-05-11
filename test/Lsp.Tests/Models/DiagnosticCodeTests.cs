using System.Text.Json;
using FluentAssertions;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using Xunit;

namespace Lsp.Tests.Models
{
    public class DiagnosticCodeTests
    {
        [Theory, JsonFixture]
        public void SimpleTest(string expected)
        {
            var model = new DiagnosticCode();
            var result = Fixture.SerializeObject(model);

            result.Should().Be(expected);

            var deresult = JsonSerializer.Deserialize<DiagnosticCode>(expected, Serializer.Instance.Options);
            deresult.Should().BeEquivalentTo(model);
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace FinanceGrid.Webhook.Tests;

public class HmacSignatureTests
{
    [Fact]
    public void ComputeHmacSignature_ProducesConsistentOutput()
    {
        var payload = "{\"event\":\"test\"}";
        var secret = "my-secret-key";

        var sig1 = ComputeHmac(payload, secret);
        var sig2 = ComputeHmac(payload, secret);

        Assert.Equal(sig1, sig2);
        Assert.NotEmpty(sig1);
    }

    [Fact]
    public void ComputeHmacSignature_DifferentPayload_DifferentSignature()
    {
        var secret = "my-secret-key";

        var sig1 = ComputeHmac("{\"a\":1}", secret);
        var sig2 = ComputeHmac("{\"a\":2}", secret);

        Assert.NotEqual(sig1, sig2);
    }

    [Fact]
    public void ComputeHmacSignature_DifferentSecret_DifferentSignature()
    {
        var payload = "{\"event\":\"test\"}";

        var sig1 = ComputeHmac(payload, "secret1");
        var sig2 = ComputeHmac(payload, "secret2");

        Assert.NotEqual(sig1, sig2);
    }

    [Fact]
    public void ComputeHmacSignature_IsValidBase64()
    {
        var payload = "{\"event\":\"test\"}";
        var secret = "my-secret-key";

        var signature = ComputeHmac(payload, secret);

        var bytes = Convert.FromBase64String(signature);
        Assert.Equal(32, bytes.Length);
    }

    [Fact]
    public void ComputeHmacSignature_KnownVector()
    {
        var payload = "test-payload";
        var secret = "test-secret";
        var expectedKey = Encoding.UTF8.GetBytes(secret);
        var expectedPayload = Encoding.UTF8.GetBytes(payload);
        var expectedHash = HMACSHA256.HashData(expectedKey, expectedPayload);
        var expected = Convert.ToBase64String(expectedHash);

        var actual = ComputeHmac(payload, secret);

        Assert.Equal(expected, actual);
    }

    private static string ComputeHmac(string payload, string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        var hash = HMACSHA256.HashData(keyBytes, payloadBytes);
        return Convert.ToBase64String(hash);
    }
}

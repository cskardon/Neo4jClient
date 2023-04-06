using System;
using FluentAssertions;
using Neo4j.Driver;
using Neo4jClient.Extensions;
using Xunit;

namespace Neo4jClient.Tests.Extensions;

public class ZonedDateTimeExtensionTests
{
    [Fact]
    public void CreatesEpochTime_WhenZtdStringIsZero()
    {
        const string ztdString = "Lazy-ZonedDateTime{EpochSeconds:0 Nanos:0 Zone:Z}";
        var dto = ztdString.FromZonedDateTimeString();
        dto.Year.Should().Be(1970);
        dto.Month.Should().Be(1);
        dto.Day.Should().Be(1);
    }

    [Fact]
    public void UsesZones()
    {
        var inDto = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(2));
        var ztd = new Lazy<ZonedDateTime>(() => new ZonedDateTime(inDto));
        var st = ztd.ToString();
        var dto = ztd.ToString().FromZonedDateTimeString();
        dto.Year.Should().Be(inDto.Year);
        dto.Month.Should().Be(inDto.Month);
        dto.Day.Should().Be(inDto.Day);
    }
}
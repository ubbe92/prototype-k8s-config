using CsvHelper.Configuration.Attributes;
using Orleans;

namespace Rebelbetting.Orleans.LoadGenerator.parsing;

[GenerateSerializer]
public record OddsDto
{
    [Id(0)] public required string Sport { get; init; }
    [Id(1)] public required string EventNameBookie1 { get; init; }
    [Id(2)] public required string Participant1OriginalBookie1 { get; init; }
    [Id(3)] public required string Participant2OriginalBookie1 { get; init; }
    [Id(4)] public required string OddsType { get; init; }
    [Id(5)] public required string LastBetDate { get; init; }
    [Id(6)] public required string Bookie1 { get; init; }
    [NullValues("")]
    [Id(7)] public string? Odds1Bookie1 { get; init; }
    [NullValues("")]
    [Id(8)] public string? Odds2Bookie1 { get; init; }
    [NullValues("")]
    [Id(9)] public string? Odds3Bookie1 { get; init; }
    [NullValues("")]
    [Id(10)] public string? Participant1HcpDecBookie1 { get; init; }
    [NullValues("")]
    [Id(11)] public string? Participant2HcpDecBookie1 { get; init; }
    [Id(12)] public required string GameLengthRuleBookie1  { get; init; }
    [NullValues("")]
    [Id(13)] public string? GameCountRuleBookie1  { get; init; }
    [NullValues("")]
    [Id(14)] public string? GameInterruptionRuleBookie1 { get; init; }
    [NullValues("")]
    [Id(15)] public string? OverUnderLimit1 { get; init; }
    
    public override string ToString()
    {
        return Sport + "," +
               EventNameBookie1 + "," +
               Participant1OriginalBookie1 + "," +
               Participant2OriginalBookie1 + "," +
               OddsType + "," +
               LastBetDate + "," +
               Bookie1 + "," +
               Odds1Bookie1 + "," +
               Odds2Bookie1 + "," +
               Odds3Bookie1 + "," +
               Participant1HcpDecBookie1 + "," +
               Participant2HcpDecBookie1 + "," +
               GameLengthRuleBookie1 + "," +
               GameCountRuleBookie1 + "," +
               GameInterruptionRuleBookie1 + "," +
               OverUnderLimit1;
    }
    
    public virtual bool Equals(OddsDto? other)
    {
        // jesus christ
        return other != null &&
               Sport == other.Sport &&
               EventNameBookie1 == other.EventNameBookie1 &&
               Participant1OriginalBookie1 == other.Participant1OriginalBookie1 &&
               Participant2OriginalBookie1 == other.Participant2OriginalBookie1 &&
               OddsType == other.OddsType &&
               LastBetDate == other.LastBetDate &&
               Bookie1 == other.Bookie1 &&
               Odds1Bookie1 == other.Odds1Bookie1 &&
               Odds2Bookie1 == other.Odds2Bookie1 &&
               Odds3Bookie1 == other.Odds3Bookie1 &&
               Participant1HcpDecBookie1 == other.Participant1HcpDecBookie1 &&
               Participant2HcpDecBookie1 == other.Participant2HcpDecBookie1 &&
               GameLengthRuleBookie1 == other.GameLengthRuleBookie1 &&
               GameCountRuleBookie1 == other.GameCountRuleBookie1 &&
               GameInterruptionRuleBookie1 == other.GameInterruptionRuleBookie1 &&
               OverUnderLimit1 == other.OverUnderLimit1;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Sport);
        hashCode.Add(EventNameBookie1);
        hashCode.Add(Participant1OriginalBookie1);
        hashCode.Add(Participant2OriginalBookie1);
        hashCode.Add(OddsType);
        hashCode.Add(LastBetDate);
        hashCode.Add(Bookie1);
        hashCode.Add(Odds1Bookie1);
        hashCode.Add(Odds2Bookie1);
        hashCode.Add(Odds3Bookie1);
        hashCode.Add(Participant1HcpDecBookie1);
        hashCode.Add(Participant2HcpDecBookie1);
        hashCode.Add(GameLengthRuleBookie1);
        hashCode.Add(GameCountRuleBookie1);
        hashCode.Add(GameInterruptionRuleBookie1);
        hashCode.Add(OverUnderLimit1);
        return hashCode.ToHashCode();
    }
}
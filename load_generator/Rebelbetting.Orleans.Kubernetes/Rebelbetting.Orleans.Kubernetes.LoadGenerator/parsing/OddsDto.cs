using CsvHelper.Configuration.Attributes;

namespace Rebelbetting.Orleans.LoadGenerator.parsing;

public record OddsDto
{
    public required string Sport { get; init; }
    public required string EventNameBookie1 { get; init; }
    public required string Participant1OriginalBookie1 { get; init; }
    public required string Participant2OriginalBookie1 { get; init; }
    public required string OddsType { get; init; }
    public required string LastBetDate { get; init; }
    public required string Bookie1 { get; init; }
    
    [NullValues("")]
    public string? Odds1Bookie1 { get; init; }
    
    [NullValues("")]
    public string? Odds2Bookie1 { get; init; }
    
    [NullValues("")]
    public string? Odds3Bookie1 { get; init; }
    
    [NullValues("")]
    public string? Participant1HcpDecBookie1 { get; init; }
    
    [NullValues("")]
    public string? Participant2HcpDecBookie1 { get; init; }
    
    public required string GameLengthRuleBookie1  { get; init; }
    
    [NullValues("")]
    public string? GameCountRuleBookie1  { get; init; }
    
    [NullValues("")]
    public string? GameInterruptionRuleBookie1 { get; init; }
    
    [NullValues("")]
    public string? OverUnderLimit1 { get; init; }
    
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
               GameLengthRuleBookie1 + "," +
               OverUnderLimit1;
    }
}
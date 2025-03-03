using System.Runtime.Serialization;

namespace Rebelbetting.Orleans.LoadGenerator.parsing;

[DataContract]
public record ContractOdds
{
    [DataMember]
    public string Sport { get; set; } = "default";
    
    [DataMember]
    public string EventNameBookie1 { get; set; } = "default";
    
    [DataMember]
    public string Participant1OriginalBookie1 { get; set; } = "default";
        
    [DataMember]
    public string Participant2OriginalBookie1 { get; set; } = "default";
    
    [DataMember]
    public string OddsType { get; set; } = "default";
    
    [DataMember]
    public string? LastBetDate { get; set; }
    
    [DataMember]
    public string Bookie1 { get; set; } = "default";

    [DataMember]
    public decimal? Odds1Bookie1 { get; set; }
    
    [DataMember]
    public decimal? Odds2Bookie1 { get; set; }

    [DataMember]
    public decimal? Odds3Bookie1 { get; set; }

    [DataMember]
    public decimal? Participant1HcpDecBookie1 { get; set; }

    [DataMember]
    public decimal? Participant2HcpDecBookie1 { get; set; }
    
    [DataMember]
    public string GameLengthRuleBookie1  { get; set; } = "default";
    
    [DataMember]
    public string? GameCountRuleBookie1  { get; set; }
    
    [DataMember]
    public string? GameInterruptionRuleBookie1 { get; set; }

    [DataMember]
    public decimal? OverUnderLimit1 { get; set; }
}
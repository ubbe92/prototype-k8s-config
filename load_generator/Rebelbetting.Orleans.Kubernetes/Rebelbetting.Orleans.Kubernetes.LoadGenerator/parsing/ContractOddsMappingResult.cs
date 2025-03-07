using System.Runtime.Serialization;
using Orleans;

namespace Rebelbetting.Orleans.Contracts.Contracts;

[GenerateSerializer]
[DataContract]
public record ContractOddsMappingResult()
{
    [Id(0)]
    [DataMember] 
    public required int NumOddsSuccessfullyMapped { get; set; } = -1;
    
    [Id(1)]
    [DataMember] 
    public required int NumTotalOdds { get; set; } = -1;    
    
    [Id(2)]
    [DataMember] 
    public required long FindMasterGrainsTime { get; set; } = -1;
    
    [Id(3)]
    [DataMember] 
    public required long FindMatchGrainsTime { get; set; } = -1;
    
    [Id(4)]
    [DataMember] 
    public required Dictionary<string, int> MappingExceptionMessagesDictionary { set; get; }
    
    
    public override string ToString()
    {
        return $"Successfully mapped {NumOddsSuccessfullyMapped} out of {NumTotalOdds}\n" +
               $"FindMasterGrainsTime: {FindMasterGrainsTime} ms\n" +
               $"FindMatchGrainsTime: {FindMatchGrainsTime} ms\n" +
               $"Number of unique exceptions: {MappingExceptionMessagesDictionary.Count}";
    }
}
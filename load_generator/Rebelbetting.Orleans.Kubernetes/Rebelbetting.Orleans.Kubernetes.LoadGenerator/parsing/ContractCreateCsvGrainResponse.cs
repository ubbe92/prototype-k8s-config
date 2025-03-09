using System.Runtime.Serialization;
using Orleans;

namespace Rebelbetting.Orleans.LoadGenerator.parsing;

[GenerateSerializer]
[DataContract]
public record ContractCreateCsvGrainResponse()
{
    [Id(0)] [DataMember] public required ContractRedisDataResult[] EventGrainCreationResult { get; set; }
    [Id(1)] [DataMember] public required ContractRedisDataResult[] ParticipantGrainCreationResult { get; set; }
}
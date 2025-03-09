using System.Runtime.Serialization;
using Orleans;

namespace Rebelbetting.Orleans.LoadGenerator.parsing;

[GenerateSerializer]
[DataContract]
public record ContractRedisDataResult()
{
    [Id(0)] [DataMember] public required int NumDataPointsInserted { get; set; } = -1;
    [Id(1)] [DataMember] public required int NumDataPointsTotal { get; set; } = -1;
    [Id(2)] [DataMember] public required long Time { get; set; } = -1;
    [Id(3)] [DataMember] public required Dictionary<string, int> InsertionExceptionMessagesDictionary { set; get; }
    
    public override string ToString()
    {
        return $"Successfully inserted {NumDataPointsInserted} out of {NumDataPointsTotal}\n" +
               $"Execution time: {Time} ms\n" +
               $"Number of unique exceptions: {InsertionExceptionMessagesDictionary.Count}";
    }
}
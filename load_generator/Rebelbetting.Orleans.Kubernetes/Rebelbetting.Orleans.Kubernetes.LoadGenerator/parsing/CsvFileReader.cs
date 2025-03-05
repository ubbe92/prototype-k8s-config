using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Rebelbetting.Orleans.LoadGenerator.parsing;

/// <summary>
/// A csv file reader that can save the data into records of type T, such as the EventDto or ParticipantDto record.
///
/// usage: var csvFileReader = new CsvFileReader&lt;T>(FilePath);
/// </summary>
/// <typeparam name="T"></typeparam>
public class CsvFileReader<T> where T : class
{
    public IEnumerable<T> Records { get; set; }
    private StreamReader _reader { get; set; }
    private CsvReader _csv { get; set; }
    
    public CsvFileReader(string path, string delimiter)
    {
        try
        {
            CsvConfiguration config;
            
            // Format of Events/Participants and Odds differ between .csv files.
            if (typeof(T) == typeof(OddsDto))
            {
                config = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = delimiter,
                    Quote = '"',
                    IgnoreBlankLines = true,
                    DetectColumnCountChanges = false,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    Mode = CsvMode.RFC4180, // Use RFC4180 mode for standard CSV parsing
                    TrimOptions = TrimOptions.Trim,
                    AllowComments = false,  // Disable comments if not needed
                };                
            }
            else
            {
                config = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = delimiter,
                    Quote = '"',
                    IgnoreBlankLines = true, 
                    DetectColumnCountChanges = false,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    Mode = CsvMode.NoEscape,
                    TrimOptions = TrimOptions.Trim,
                };
            }
            
            _reader = new StreamReader(path);
            _csv = new CsvReader(_reader, config);

            // The odds csv file contains " characters around almost all the data (which the Events/Participant 
            // files does not. Hence, we need to trim the OddsDto specifically of leading and trailing " chars
            if (typeof(T) == typeof(OddsDto))
            {
                _csv.Context.TypeConverterCache.AddConverter<string>(new QuoteTrimConverter());
                // _csv.Context.TypeConverterCache.AddConverter<DateTime>(new CustomDateTimeConverter());
            }
            
            Records = _csv.GetRecords<T>();
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading CSV file: {{e}} {e.StackTrace}");
        }
    }

    public void CleanUp()
    {
        _reader.Dispose();
        _csv.Dispose();
    }
    
    private class QuoteTrimConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.Trim('"').Replace("´", "'"); // Fix issue with strange quotes and ´ found in the odds.csv...
        }
    }
    
    public ContractOdds[] ConvertOddsDtoToContractOdds(OddsDto[] oddsDtos)
    {
        var contractOddsList = new List<ContractOdds>();
        foreach (var dto in oddsDtos)
        {
            var contract = new ContractOdds()
            {
                Sport = dto.Sport,
                EventNameBookie1 = dto.EventNameBookie1,
                Participant1OriginalBookie1 = dto.Participant1OriginalBookie1,
                Participant2OriginalBookie1 = dto.Participant2OriginalBookie1,
                OddsType = dto.OddsType,
                LastBetDate = dto.LastBetDate,
                Bookie1 = dto.Bookie1,
                Odds1Bookie1 = string.IsNullOrEmpty(dto.Odds1Bookie1) ? null : decimal.Parse(dto.Odds1Bookie1, CultureInfo.InvariantCulture),
                Odds2Bookie1 = string.IsNullOrEmpty(dto.Odds2Bookie1) ? null : decimal.Parse(dto.Odds2Bookie1, CultureInfo.InvariantCulture),
                Odds3Bookie1 = string.IsNullOrEmpty(dto.Odds3Bookie1) ? null : decimal.Parse(dto.Odds3Bookie1, CultureInfo.InvariantCulture),
                Participant1HcpDecBookie1 = string.IsNullOrEmpty(dto.Participant1HcpDecBookie1) ? null : decimal.Parse(dto.Participant1HcpDecBookie1, CultureInfo.InvariantCulture),
                Participant2HcpDecBookie1 = string.IsNullOrEmpty(dto.Participant2HcpDecBookie1) ? null : decimal.Parse(dto.Participant2HcpDecBookie1, CultureInfo.InvariantCulture),
                GameLengthRuleBookie1 = dto.GameLengthRuleBookie1,
                GameCountRuleBookie1 = dto.GameCountRuleBookie1,
                GameInterruptionRuleBookie1 = dto.GameInterruptionRuleBookie1,
                OverUnderLimit1 = string.IsNullOrEmpty(dto.OverUnderLimit1) ? null : decimal.Parse(dto.OverUnderLimit1, CultureInfo.InvariantCulture),
            };
            contractOddsList.Add(contract);
        }
        return contractOddsList.ToArray();
    }
}
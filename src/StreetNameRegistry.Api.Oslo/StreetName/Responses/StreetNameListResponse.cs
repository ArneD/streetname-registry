namespace StreetNameRegistry.Api.Oslo.StreetName.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Be.Vlaanderen.Basisregisters.Api.JsonConverters;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using Be.Vlaanderen.Basisregisters.GrAr.Legacy;
    using Be.Vlaanderen.Basisregisters.GrAr.Legacy.Straatnaam;
    using Infrastructure.Options;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "StraatnaamCollectie", Namespace = "")]
    public class StreetNameListResponse
    {
        /// <summary>
        /// De linked-data context van straatnamen.
        /// </summary>
        [DataMember(Name = "@context", Order = 0)]
        [JsonProperty(Required = Required.DisallowNull)]
        [JsonConverter(typeof(PlainStringJsonConverter))]
        public object Context => @"[
     {""@base"": ""https://data.vlaanderen.be/id/concept/"" ,   
      ""identificator"": ""@nest"",
      ""id"": ""@id"",
      ""versieId"": {
        ""@id"": ""https://data.vlaanderen.be/ns/generiek#versieIdentificator"",
        ""@type"": ""http://www.w3.org/2001/XMLSchema#string""
      },            
      ""versieId"": {
        ""@id"": ""https://data.vlaanderen.be/ns/generiek#versieIdentificator"",
        ""@type"": ""http://www.w3.org/2001/XMLSchema#string""
      }, 
      ""straatnaamStatus"": {
        ""@id"": ""https://data.vlaanderen.be/ns/adres#Straatnaam.status"",
        ""@type"": ""@id"",
        ""@context"": {
          ""@base"": ""https://data.vlaanderen.be/id/concept/straatnaamstatus/""
        }
      },
      ""straatnaam"": {
        ""@id"": ""https://data.vlaanderen.be/ns/adres#heeftStraatnaam"",
        ""@type"": ""@id"",
        ""@context"": {
          ""@base"": ""https://data.vlaanderen.be/id/straatnaam/"",
          ""objectId"": ""@id"",
          ""straatnaam"": ""@nest"",
          ""geografischeNaam"": {
            ""@id"": ""http://www.w3.org/2000/01/rdf-schema#label"",
            ""@context"": {
              ""spelling"": ""@value"",
              ""taal"": ""@language""
            }
          }
        }
      },     
      ""detail"": ""http://www.iana.org/assignments/relation/self"",
       ""straatnamen"": ""@graph""
   }                       
  ]";

        /// <summary>
        /// De verzameling van straatnamen.
        /// </summary>
        [DataMember(Name = "Straatnamen", Order = 1)]
        [JsonProperty(Required = Required.DisallowNull)]
        public List<StreetNameListItemResponse> Straatnamen { get; set; }

        ///// <summary>
        ///// Het totaal aantal gemeenten die overeenkomen met de vraag.
        ///// </summary>
        //[DataMember(Name = "TotaalAantal", Order = 2)]
        //[JsonProperty(Required = Required.DisallowNull)]
        //public long TotaalAantal { get; set; }

        /// <summary>
        /// De URL voor het ophalen van de volgende verzameling.
        /// </summary>
        [DataMember(Name = "Volgende", Order = 3, EmitDefaultValue = false)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri? Volgende { get; set; }
    }

    [DataContract(Name = "StraatnaamCollectieItem", Namespace = "")]
    public class StreetNameListItemResponse
    {
        /// <summary>
        /// Het linked-data type van de straatnaam.
        /// </summary>
        [DataMember(Name = "@type", Order = 0)]
        [JsonProperty(Required = Required.DisallowNull)]
        public string Type => "Straatnaam";

        /// <summary>
        /// De identificator van de straatnaam.
        /// </summary>
        [DataMember(Name = "Identificator", Order = 1)]
        [JsonProperty(Required = Required.DisallowNull)]
        public StraatnaamIdentificator Identificator { get; set; }

        /// <summary>
        /// De URL die de details van de meest recente versie van de straatnaam weergeeft.
        /// </summary>
        [DataMember(Name = "Detail", Order = 2)]
        [JsonProperty(Required = Required.DisallowNull)]
        public Uri Detail { get; set; }

        /// <summary>
        /// De straatnaam in de eerste officiële taal van de gemeente.
        /// </summary>
        [DataMember(Name = "Straatnaam", Order = 3)]
        [JsonProperty(Required = Required.DisallowNull)]
        public Straatnaam Straatnaam { get; set; }

        /// <summary>
        /// De homoniemtoevoeging in de eerste officiële taal van de gemeente.
        /// </summary>
        [DataMember(Name = "HomoniemToevoeging", Order = 4, EmitDefaultValue = false)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HomoniemToevoeging HomoniemToevoeging { get; set; }

        /// <summary>
        /// De huidige fase in de levensloop van een straatnaam.
        /// </summary>
        [DataMember(Name = "StraatnaamStatus", Order = 5)]
        [JsonProperty(Required = Required.DisallowNull)]
        public StraatnaamStatus StraatnaamStatus { get; set; }

        public StreetNameListItemResponse(
            int? id,
            string naamruimte,
            string detail,
            GeografischeNaam geografischeNaam,
            GeografischeNaam? homoniemToevoeging,
            StraatnaamStatus status,
            DateTimeOffset? version)
        {
            Identificator = new StraatnaamIdentificator(naamruimte, id?.ToString(), version);
            Detail = new Uri(string.Format(detail, id));
            Straatnaam = new Straatnaam(geografischeNaam);
            StraatnaamStatus = status;

            if (homoniemToevoeging != null)
            {
                HomoniemToevoeging = new HomoniemToevoeging(homoniemToevoeging);
            }
        }
    }

    public class StreetNameListResponseExamples : IExamplesProvider<StreetNameListResponse>
    {
        private readonly ResponseOptions _responseOptions;

        public StreetNameListResponseExamples(IOptions<ResponseOptions> responseOptionsProvider)
         => _responseOptions = responseOptionsProvider.Value;

        public StreetNameListResponse GetExamples()
        {
            var streetNameSamples = new List<StreetNameListItemResponse>
                {
                    new StreetNameListItemResponse(
                        1000,
                        _responseOptions.Naamruimte,
                        _responseOptions.DetailUrl,
                        new GeografischeNaam("Kerkstraat", Taal.NL),
                        null,
                        StraatnaamStatus.InGebruik,
                        DateTimeOffset.Now.ToExampleOffset()),

                    new StreetNameListItemResponse(
                        1001,
                        _responseOptions.Naamruimte,
                        _responseOptions.DetailUrl,
                        new GeografischeNaam("Wetstraat", Taal.NL),
                        new GeografischeNaam("BR", Taal.NL),
                        StraatnaamStatus.Voorgesteld,
                        DateTimeOffset.Now.ToExampleOffset())
                };

            return new StreetNameListResponse
            {
                Straatnamen = streetNameSamples,
                Volgende = new Uri(string.Format(_responseOptions.VolgendeUrl, 2, 10))
            };
        }
    }
}
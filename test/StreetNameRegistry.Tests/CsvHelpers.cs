﻿namespace StreetNameRegistry.Tests
{
    using System.IO;
    using System.Text;
    using Microsoft.AspNetCore.Http;

    public sealed class CsvHelpers
    {
        public const string OldNisCode = "11000";
        public const string Example = $"OUD NIS code;OUD straatnaamid;NIEUW NIS code;NIEUW straatnaam;NIEUW homoniemtoevoeging\n{OldNisCode};123;11001;naam;BB";
        //Convert the string to be able to passed as IFormFile

        public static IFormFile CreateFormFileFromString(string content, string fileName = "file.csv")
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };
        }
    }
}

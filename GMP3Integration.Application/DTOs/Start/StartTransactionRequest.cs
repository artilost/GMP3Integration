using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs
{
    public class StartTransactionRequest
    {
        //[JsonPropertyName("currentInterface")]
        public string CurrentInterface { get; set; }
    }
}
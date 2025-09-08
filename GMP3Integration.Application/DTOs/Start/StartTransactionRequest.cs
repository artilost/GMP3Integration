using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMP3Integration.Application.DTOs
{
    /// <summary>
    /// StartTransaction için request DTO
    /// Doküman sayfa 16: "First function to start a transaction"
    /// </summary>
    public class StartTransactionRequest
    {
        /// <summary>
        /// Interface string (örn: "COM1", "TCP:192.168.1.100:7500")
        /// </summary>
        public string Interface { get; set; }
        
        /// <summary>
        /// Unique ID for transaction (24 bytes)
        /// Doküman sayfa 13: "Unique ID 24 byte"
        /// </summary>
        public byte[] UniqueId { get; set; } = new byte[24];
        
        /// <summary>
        /// Timeout in milliseconds
        /// </summary>
        public int TimeoutMs { get; set; } = 10000;
        
        /// <summary>
        /// Current interface (legacy support)
        /// </summary>
        [JsonPropertyName("currentInterface")]
        public string CurrentInterface 
        { 
            get => Interface; 
            set => Interface = value; 
        }
    }
}
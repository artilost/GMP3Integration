using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace GMP3Integration.Application.DTOs.Payment
{
    public class PaymentResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("paymentId")]
        public string PaymentId { get; set; }
        
        [JsonPropertyName("resultCode")]
        public int ResultCode { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; }
        
        public PaymentResponse()
        {
            PaymentId = "";
            Message = "";
            ResultCode = 0;
        }
    }
}

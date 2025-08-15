namespace GMP3Integration.API.Models
{
    public class HealthResponse
    {
        public string Status { get; set; }          // "ok" / "degraded" / "fail"
        public string Environment { get; set; }     // Development/Staging/Production
        public string Version { get; set; }         // Assembly versiyonu
        public long UptimeSeconds { get; set; }     // Çalışma süresi (saniye)
        public string NowUtc { get; set; }          // Sunucu zamanı (UTC ISO)
        public string Detail { get; set; }          // (ready için) kısa açıklama
    }
}

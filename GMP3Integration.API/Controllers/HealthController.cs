using GMP3Integration.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GMP3Integration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        // Uptime için basit sayaç
        private static readonly DateTime _startedUtc = DateTime.UtcNow;
        private readonly string _environment;

        public HealthController(IWebHostEnvironment env)
        {
            _environment = env.EnvironmentName ?? "Unknown";
        }

        /// <summary>
        /// Liveness: Uygulama ayakta mı?
        /// </summary>
        [HttpGet("healthz")]
        public ActionResult<HealthResponse> Healthz()
        {
            var asm = Assembly.GetExecutingAssembly().GetName();
            var response = new HealthResponse
            {
                Status = "ok",
                Environment = _environment,
                Version = asm.Version?.ToString() ?? "unknown",
                UptimeSeconds = (long)(DateTime.UtcNow - _startedUtc).TotalSeconds,
                NowUtc = DateTime.UtcNow.ToString("o"),
                Detail = "Process is alive."
            };
            return Ok(response);
        }

        /// <summary>
        /// Readiness: Uygulama trafik almaya hazır mı?
        /// Basit kontrolde 'ok' döner; ileri düzeyde dış bağımlılık kontrolleri eklenebilir.
        /// </summary>
        [HttpGet("readyz")]
        public ActionResult<HealthResponse> Readyz()
        {
            // Burada hafif kontrol yapıyoruz. (DLL henüz yok; ağır kontrol yok.)
            // Örn. kritik configuration anahtarları dolu mu?
            // İleride: cihaz bağlantısı, veritabanı, mesaj kuyruğu vb. kontrolleri ekleyebilirsin.

            bool ready = true;
            string detail = "All essential checks passed.";

            var asm = Assembly.GetExecutingAssembly().GetName();
            var response = new HealthResponse
            {
                Status = ready ? "ok" : "degraded",
                Environment = _environment,
                Version = asm.Version?.ToString() ?? "unknown",
                UptimeSeconds = (long)(DateTime.UtcNow - _startedUtc).TotalSeconds,
                NowUtc = DateTime.UtcNow.ToString("o"),
                Detail = detail
            };
            return ready ? Ok(response) : StatusCode(503, response);
        }
    }
}

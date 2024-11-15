using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;


   
    [ApiController]
    public class HealthController : ControllerBase
    {
        private static readonly DateTime StartupTime = DateTime.UtcNow;
        private static readonly string Version = "1.0.0";  

        [HttpGet]
        [Route("/health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "UP",
                version = Version,
                uptime = GetUptime(),
                checks = new[]
                {
                    new
                    {
                        name = "Readiness check",
                        status = "UP",
                        data = new
                        {
                            from = StartupTime,
                            status = "READY"
                        }
                    },
                    new
                    {
                        name = "Liveness check",
                        status = "UP",
                        data = new
                        {
                            from = StartupTime,
                            status = "ALIVE"
                        }
                    }
                }
            });
        }

        [HttpGet]
        [Route("/health/ready")]
        public IActionResult Readiness()
        {
            return Ok(new
            {
                status = "UP",
                version = Version,
                uptime = GetUptime(),
                data = new
                {
                    from = StartupTime,
                    status = "READY"
                }
            });
        }

        [HttpGet]
        [Route("/health/live")]
        public IActionResult Liveness()
        {
            return Ok(new
            {
                status = "UP",
                version = Version,
                uptime = GetUptime(),
                data = new
                {
                    from = StartupTime,
                    status = "ALIVE"
                }
            });
        }

        private string GetUptime()
        {
            var uptime = DateTime.UtcNow - StartupTime;
            return uptime.ToString(@"dd\.hh\:mm\:ss");
        }
    }

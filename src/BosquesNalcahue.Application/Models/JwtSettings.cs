﻿namespace BosquesNalcahue.Application.Models
{
    public class JwtSettings
    {
        public string? SecurityKey { get; set; }
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
        public int TokenLifeTimeInHours { get; set; }
    }
}

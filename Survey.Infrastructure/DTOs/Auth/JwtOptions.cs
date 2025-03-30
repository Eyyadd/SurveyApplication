using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.DTOs.Auth
{
    public class JwtOptions
    {
        public static string SectionName = "JwtOptions";
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience {  get; set; } = string.Empty;

        [Range(1,72)]
        public int ExpireIn { get; set; }
    }
}

using System.Collections.Generic;
using System;

namespace Core.Application.Abstractions.ExternalLogin
{
    public class ExternalLoginOptions
    {
        public string ClientSecret { get; set; }
        public IEnumerable<string> Audience { get; set; }
        public Type ExternalLoginProviderType { get; set; }
    }
}
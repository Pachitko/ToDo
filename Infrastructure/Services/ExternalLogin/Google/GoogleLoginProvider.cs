using Core.Application.Abstractions.ExternalLogin;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Infrastructure.Services.ExternalLogin.Google
{
    internal class GoogleLoginProvider : IExternalLoginProvider
    {
        private readonly IOptions<GoogleOptions> _options;

        public GoogleLoginProvider(IOptions<GoogleOptions> options)
        {
            _options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public async Task<ExternalLoginPayload> GetPayloadAsync(string tokenId)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = _options.Value.Audience,
            };

            var googlePayload = await GoogleJsonWebSignature.ValidateAsync(tokenId, settings);

            ExternalLoginPayload externalLoginPayload = new(googlePayload.Subject, googlePayload.Email);

            return externalLoginPayload;
        }
    }
}
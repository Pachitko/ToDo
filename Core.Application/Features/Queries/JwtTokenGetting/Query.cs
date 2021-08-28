namespace Core.Application.Features.Queries.JwtLogin
{
    public partial class JwtTokenGetting
    {
        public record Query : IRequestWrapper<string>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
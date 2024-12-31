using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using campus.DBContext;
using campus.DBContext.Models;
using Microsoft.IdentityModel.Tokens;

namespace campus.AdditionalServices.TokenHelpers;

public class TokenInteraction
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string? _secretKey;
    private readonly string? _issuer;
    private readonly string? _audience;
    
    public TokenInteraction(IConfiguration configuration, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _secretKey = configuration.GetValue<string>("AppSettings:Secret");
        _issuer = configuration.GetValue<string>("AppSettings:Issuer");
        _audience = configuration.GetValue<string>("AppSettings:Audience");
        _serviceProvider = serviceProvider;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid GetAccountIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var id = jwtToken.Payload.TryGetValue(ClaimTypes.Sid, out var accountIdObj) && accountIdObj is string accountId ? accountId : string.Empty;
        return Guid.Parse(id);
    }
    public string GenerateToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, account.Id.ToString())
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(90),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    public string GetTokenFromHeader()
    {
        using var scope = _serviceProvider.CreateScope();
        var authorizationHeader = _serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.Request.Headers.Authorization.FirstOrDefault();
        return (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            ? authorizationHeader.Substring("Bearer ".Length)
            : "";
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        return jwtToken != null && jwtToken.ValidTo >= DateTime.UtcNow;
    }
}
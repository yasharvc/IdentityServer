namespace Tribit.IdentityServer.Shared.Entities
{
	public class JwtSettings
	{
		public bool ValidateIssuerSigningKey { get; set; }
		public string IssuerSigningKey { get; set; } = string.Empty;
		public bool ValidateIssuer { get; set; } = true;
		public string ValidIssuer { get; set; } = string.Empty;
		public bool ValidateAudience { get; set; } = true;
		public string ValidAudience { get; set; } = string.Empty;
		public bool RequireExpirationTime { get; set; }
		public bool ValidateLifetime { get; set; } = true;
	}
}
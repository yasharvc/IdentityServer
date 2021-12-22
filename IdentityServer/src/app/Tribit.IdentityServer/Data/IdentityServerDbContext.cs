using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tribit.IdentityServer.Shared.Entities;

namespace Tribit.IdentityServer.Data
{
    public class IdentityServerDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options)
            : base(options)
        {
        }
    }
}
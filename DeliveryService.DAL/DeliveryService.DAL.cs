using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeliveryService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}

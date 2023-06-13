using Journeys.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.DataAccess
{
    public class JourneyDataContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public virtual DbSet<Journey> Journeys { get; set; }

        public JourneyDataContext(DbContextOptions<JourneyDataContext> options) : base(options)
        {

        }
    }
}

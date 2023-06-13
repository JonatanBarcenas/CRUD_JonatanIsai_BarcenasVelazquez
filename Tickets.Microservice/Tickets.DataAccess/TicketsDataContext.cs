using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tickets.Core;

namespace Tickets.DataAccess
{
    public class TicketsDataContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public virtual DbSet<Ticket> Tickets { get; set; }

        public TicketsDataContext(DbContextOptions<TicketsDataContext> options) : base(options)
        {

        }
    }
}

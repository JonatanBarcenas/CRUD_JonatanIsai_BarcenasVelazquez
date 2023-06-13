
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Passengers.Core.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passengers.DataAccess
{
    public class PassengersDataContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public virtual DbSet<Passenger> Passenger { get; set; }

        public PassengersDataContext(DbContextOptions<PassengersDataContext> options) : base(options)
        {

        }

    }
}

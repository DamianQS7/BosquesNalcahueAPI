using BosquesNalcahue.Application.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BosquesNalcahue.Application.Models;

public class WebPortalDbContext(DbContextOptions<WebPortalDbContext> options) : IdentityDbContext<WebPortalUser>(options)
{
}

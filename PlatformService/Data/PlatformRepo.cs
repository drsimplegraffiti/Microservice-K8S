using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        // inject the AppDbContext into the constructor using dependency injection
        public PlatformRepo(AppDbContext context)
        {
            _context = context;

        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            _context.Platforms.Add(plat);
        }

        public void DeletePlatform(Platform plat)
        {
            _context.Platforms.Remove(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            // the Platforms is coming from the DbSet<Platform> Platforms { get; set; } in AppDbContext.cs
            return _context.Platforms.ToList(); 
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public Platform GetPlatformByName(string name)
        {
            return _context.Platforms.FirstOrDefault(p => p.Name == name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public Platform UpdatePlatform(Platform plat)
        {
            return plat;
        }
    }
}
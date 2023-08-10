using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Dtos
{
    public class PlatformReadDto
    {
        public int Id { get; set; } // because we are reading data, we don't need the data annotations
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Cost { get; set; }
    }
}
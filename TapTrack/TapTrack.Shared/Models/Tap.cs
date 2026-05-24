using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapTrack.Shared.Models
{
    internal class Tap : Entity
    {
        public required string Label { get; set; }
        public required string Color { get; set; }
    }
}

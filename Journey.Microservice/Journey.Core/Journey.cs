﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.Core
{
    public class Journey
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public int OriginId { get; set; }
        public DateTime Depature { get; set; }
        public DateTime Arrival { get; set; }
    }
}

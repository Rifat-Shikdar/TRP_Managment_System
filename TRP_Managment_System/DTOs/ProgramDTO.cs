using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRP_Managment_System.EF;

namespace TRP_Managment_System.DTOs
{
    public class ProgramDTO
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public double TRPScore { get; set; }
        public int ChannelId { get; set; }
        public System.TimeSpan AirTime { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
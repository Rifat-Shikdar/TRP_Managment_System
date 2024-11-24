using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRP_Managment_System.EF;

namespace TRP_Managment_System.DTOs
{
    public class ChannelDTO
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int EstablishedYear { get; set; }
        public string Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Program> Programs { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.BuddyUp.DomainDto
{
    public class GroupSummaryForUser
    {
        public List<GroupSummary> suggestedGroups { get; set; }
        public List<GroupSummary> AllGroups { get; set; }

        public GroupSummary registeredGroup { get; set; }
    }

    public class GroupSummary
    {
        /// <summary>
        /// Group Code
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// Name of Group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Timezone
        /// </summary>
        public string activeTimeZone { get; set; }

        /// <summary>
        /// Group Objective
        /// </summary>
        public string Objective { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.BuddyUp.DomainDto
{
    public class PostAddRequest
    {
        public string GroupCode { get; set; }

        public string PostText { get; set; }

        public string UserName { get; set; }

        public DateTime TimePosted { get; set; }

        public string ParentPostUserName { get; set; }

        public DateTime ParentPostTime { get; set; }
    }
}

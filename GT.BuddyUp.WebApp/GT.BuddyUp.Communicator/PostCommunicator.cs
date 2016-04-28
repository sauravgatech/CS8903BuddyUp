using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.DomainModel;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.Platform.Common;

namespace GT.BuddyUp.Communicator
{
    public class PostCommunicator
    {
        IPost post;
        public PostCommunicator(IPost post)
        {
            this.post = post;
        }

        public bool AddPost(PostAddRequest request)
        {
            try
            {
                post.AddPost(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

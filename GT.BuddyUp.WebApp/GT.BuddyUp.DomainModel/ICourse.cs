using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Platform.Common;

namespace GT.BuddyUp.DomainModel
{
    public interface ICourse
    {
        IEnumerable<CourseGetResponse> Get(string courseCode);

        IEnumerable<CourseGetResponse> Get();

        UserCourseGetReponse GetCourseForUser(string emailID);

        DomainModelResponse Add(CourseAddRequest request);

        DomainModelResponse Update(CourseUpdateRequest request);

        DomainModelResponse Delete(string courseCode);
    }
}

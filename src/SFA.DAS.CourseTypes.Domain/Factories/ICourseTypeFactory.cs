using SFA.DAS.CourseTypes.Domain.CourseTypes;

namespace SFA.DAS.CourseTypes.Domain.Factories
{
    public interface ICourseTypeFactory
    {
        public CourseType Get(string shortCode);
    }
}

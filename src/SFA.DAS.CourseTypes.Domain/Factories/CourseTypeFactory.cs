using SFA.DAS.CourseTypes.Domain.CourseTypes;

namespace SFA.DAS.CourseTypes.Domain.Factories
{
    public class CourseTypeFactory : ICourseTypeFactory
    {
        private readonly IEnumerable<CourseType> _courseTypes = new List<CourseType> { new Apprenticeship(), new FoundationApprenticeship() };

        public CourseType Get(string shortCode)
        {
            return _courseTypes.Single(x => string.Equals(x.ShortCode, shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

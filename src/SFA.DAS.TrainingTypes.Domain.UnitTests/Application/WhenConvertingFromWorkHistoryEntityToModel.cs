using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Application
{
    [TestFixture]
    public class WhenConvertingFromWorkHistoryEntityToModel
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Mapped(WorkHistoryEntity source)
        {
            var actual = (WorkHistory)source;

            actual.Id.Should().Be(source.Id);
            actual.ApplicationId.Should().Be(source.ApplicationId);
            actual.Description.Should().Be(source.Description);
            actual.Employer.Should().Be(source.Employer);
            actual.JobTitle.Should().Be(source.JobTitle);
            actual.StartDate.Should().Be(source.StartDate);
            actual.EndDate.Should().Be(source.EndDate);
            actual.WorkHistoryType.Should().Be((WorkHistoryType)source.WorkHistoryType);
        }
    }
}

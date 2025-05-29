using SFA.DAS.TrainingTypes.Domain.TrainingTypes;

namespace SFA.DAS.TrainingTypes.Domain.Factories
{
    public class TrainingTypeFactory : ITrainingTypeFactory
    {
        private readonly IEnumerable<TrainingType> _trainingTypes = new List<TrainingType> { new Apprenticeship(), new FoundationApprenticeship() };

        public TrainingType Get(string shortCode)
        {
            return _trainingTypes.Single(x => string.Equals(x.ShortCode, shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

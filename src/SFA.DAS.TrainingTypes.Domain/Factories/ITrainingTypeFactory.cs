using SFA.DAS.TrainingTypes.Domain.TrainingTypes;

namespace SFA.DAS.TrainingTypes.Domain.Factories
{
    public interface ITrainingTypeFactory
    {
        public TrainingType Get(string shortCode);
    }
}

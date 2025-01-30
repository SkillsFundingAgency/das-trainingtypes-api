using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.ReferenceData;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.AddLegacyApplication;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Application
{
    [TestFixture]
    [NonParallelizable]
    public class WhenHandlingAddLegacyApplicationCommand
    {
        private ApplicationEntity _capturedApplicationEntity = null!;
        private List<QualificationReferenceEntity> _qualificationReferenceEntities = null!;
        private Guid _applicationId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _applicationId = Guid.NewGuid();

            _capturedApplicationEntity = new ApplicationEntity();

            _qualificationReferenceEntities =
            [
                new() { Id = Guid.NewGuid(), Name = "GCSE" },
                new() { Id = Guid.NewGuid(), Name = "BTEC" },
                new() { Id = Guid.NewGuid(), Name = "OTHER" }
            ];
        }

        [Test, MoqAutoData]
        public async Task Then_The_Application_Is_Created_And_The_Application_Id_Is_Returned(
            AddLegacyApplicationCommand command,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
            AddLegacyApplicationCommandHandler handler)
        {
            // Arrange
            SetupTestData(command, qualificationReferenceRepository, applicationRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Id.Should().Be(_applicationId);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Application_Details_Are_Migrated_Correctly(
            AddLegacyApplicationCommand command,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
            AddLegacyApplicationCommandHandler handler)
        {
            // Arrange
            SetupTestData(command, qualificationReferenceRepository, applicationRepository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            using var scope = new AssertionScope();
            _capturedApplicationEntity.Status.Should().Be((short)command.LegacyApplication.Status);
            _capturedApplicationEntity.MigrationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            _capturedApplicationEntity.Strengths.Should().Be(command.LegacyApplication.SkillsAndStrengths);
            _capturedApplicationEntity.Support.Should().Be(command.LegacyApplication.Support);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Training_Courses_Are_Migrated_Correctly(
            AddLegacyApplicationCommand command,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
            AddLegacyApplicationCommandHandler handler)
        {
            // Arrange
            SetupTestData(command, qualificationReferenceRepository, applicationRepository);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _capturedApplicationEntity.TrainingCourseEntities.Count.Should().Be(command.LegacyApplication.TrainingCourses.Count);

            var expectedTrainingCourses = command.LegacyApplication.TrainingCourses.Select(x => new TrainingCourseEntity
            {
                Title = x.Title,
                ToYear = x.ToDate.Year
            }).ToList();

            _capturedApplicationEntity.TrainingCourseEntities.Should().BeEquivalentTo(expectedTrainingCourses);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Job_History_Is_Migrated_Correctly(
            AddLegacyApplicationCommand command,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
            AddLegacyApplicationCommandHandler handler)
        {
            // Arrange
            SetupTestData(command, qualificationReferenceRepository, applicationRepository);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _capturedApplicationEntity.WorkHistoryEntities.Count.Should().Be(command.LegacyApplication.WorkExperience.Count);

            var expectedWorkHistory = command.LegacyApplication.WorkExperience.Select(x => new WorkHistoryEntity
            {
                WorkHistoryType = (byte)WorkHistoryType.Job,
                Employer = x.Employer,
                JobTitle = x.JobTitle,
                Description = x.Description,
                StartDate = x.FromDate == DateTime.MinValue ? DateTime.UtcNow : x.FromDate,
                EndDate = x.ToDate == DateTime.MinValue ? null : x.ToDate,
            }).ToList();

            _capturedApplicationEntity.WorkHistoryEntities.Should().BeEquivalentTo(expectedWorkHistory);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Qualifications_Are_Migrated_Correctly(
            AddLegacyApplicationCommand command,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
            AddLegacyApplicationCommandHandler handler)
        {
            // Arrange
            SetupTestData(command, qualificationReferenceRepository, applicationRepository);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _capturedApplicationEntity.QualificationEntities.Count.Should().Be(command.LegacyApplication.Qualifications.Count);

            var expectedQualifications = command.LegacyApplication.Qualifications.Select(x => new QualificationEntity
            {
                QualificationReferenceId = _qualificationReferenceEntities.Single(q => q.Name == x.QualificationType).Id,
                Subject = x.QualificationType == "OTHER" ? x.QualificationType : x.Subject,
                IsPredicted = x.QualificationType != "OTHER" && x.IsPredicted,
                AdditionalInformation = x.QualificationType == "OTHER" ? x.Subject : string.Empty,
                Grade = x.Grade
            }).ToList();

            _capturedApplicationEntity.QualificationEntities.Should().BeEquivalentTo(expectedQualifications);
        }

        private void SetupTestData(
            AddLegacyApplicationCommand command,
            Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
            Mock<IApplicationRepository> applicationRepository)
        {
            for (var i = 0; i < command.LegacyApplication.Qualifications.Count; i++)
            {
                var qualification = command.LegacyApplication.Qualifications[i];
                qualification.QualificationType = _qualificationReferenceEntities[i].Name.ToString();
            }

            qualificationReferenceRepository.Setup(x => x.GetAll())
                .ReturnsAsync(_qualificationReferenceEntities);

            var returnedApplication = new ApplicationEntity
            {
                Id = _applicationId
            };

            applicationRepository.Setup(x => x.Upsert(It.IsAny<ApplicationEntity>()))
                .Callback<ApplicationEntity>(entity =>
                {
                    _capturedApplicationEntity = entity;
                })
                .ReturnsAsync(new Tuple<ApplicationEntity, bool>(returnedApplication, true));
        }
    }
}

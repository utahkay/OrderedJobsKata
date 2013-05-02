using NUnit.Framework;

namespace OrderedJobsKata
{
    [TestFixture]
    public class TestOrderedJobs
    {
        JobOrderer jobOrderer;

        [SetUp]
        public void Setup()
        {
            jobOrderer = new JobOrderer();
        }

        [Test]
        public void TestEmptyInput()
        {
            Assert.That(jobOrderer.Output(""), Is.EqualTo(""));
        }

        [Test]
        public void TestNoDependencies()
        {
            Assert.That(jobOrderer.Output("a =>"), Is.EqualTo("a"));
        }

        [Test]
        public void TestMultipleJobsNoDependencies()
        {
            Assert.That(jobOrderer.Output("a =>\n\nb =>\n\nc =>"), Is.EqualTo("abc"));
        }

        [Test]
        public void TestMultipleJobsOneHasDependency()
        {
            Assert.That(jobOrderer.Output("a => \n\nb => c \n\nc =>"), Is.EqualTo("acb"));
        }

        [Test]
        public void TestMultipleJobsWithDependencies()
        {
            Assert.That(jobOrderer.Output("a => \n\n b => c \n\n c => f \n\n d => a \n\n e => b \n\n f =>"),
                Is.EqualTo("afcbde"));
        }

        [Test]
        public void TestErrorOnCycles()
        {
            try
            {
                jobOrderer.Output("a => \n\n b => c \n\n c => f \n\n d => a \n\n e => \n\n f => b");
                Assert.Fail();
            }
            catch (CycleException)
            {
            }
        }
    }
}

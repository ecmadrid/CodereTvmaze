using CodereTvmaze.BLL;

namespace TestBLLAndWebApi
{
    /// <summary>
    /// Class <c>TestsBLLShowsById</c> tests BLL methods for get main info by id.
    /// </summary>
    public class TestsBLLShowsById
    {
        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Method to test bad values for getting main info (lower than 1).
        /// </summary>
        /// <param name="value"></param>
        [TestCase(0)]
        [TestCase(-1)]
        public void TestGetMainInfoById_LowerThanOne(long value)
        {
            var result = MainInfo.GetById(value);
            Assert.That(result, Is.Null, "Value lower than 1 is not allowed.");
        }
        /// <summary>
        /// Method to test correct values for getting main info (greater than 0).
        /// </summary>
        /// <param name="value"></param>
        [TestCase(1)]
        [TestCase(72)]
        public void TestGetMainInfoById_GreaterThanZero(long value)
        {
            var result = MainInfo.GetById(value);
            Assert.That(result, Is.Not.Null, "Value greater than 0 is allowed.");
        }

        /// <summary>
        /// Method to test bad values for getting main info (excessively great. More than max id).
        /// </summary>
        /// <param name="value"></param>
        [TestCase(1234567890)]
        public void TestGetMainInfoById_ExcessivelyGreat(long value)
        {
            var result = MainInfo.GetById(value);
            Assert.That(result, Is.Null, "Value excessively great (more than max id) is not allowed.");
        }
    }
}
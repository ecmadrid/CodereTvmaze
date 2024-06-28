using CodereTvmaze.BLL;
using CodereTvmaze.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodereTvmaze;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestBLLAndWebApi
{
    /// <summary>
    /// Class <c>TestsWebApi</c>
    /// </summary>
    public class TestsWebApi
    {
        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Method to test endpoint for validating api key with incorrect value.
        /// </summary>
        /// <param name="value"></param>
        [TestCase("xxx")]
        public void ValidateApi_BadApiKey(string value)
        {
            var controller = new CodereTvmazeController(null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "ApiKey: " + value);
            var response = controller.ValidateApi(value);
            NUnit.Framework.Assert.That(response == Results.Unauthorized(), "Api is incorrect and it detected it.");
        }

        /// <summary>
        /// Method to test endpoint for validating api key with correct value.
        /// </summary>
        /// <param name="value"></param>
        [TestCase("Q4w4T6pwAmGPmMZVvhrF2mT6aPIotwi7nSnMAGd365jB5Jzr3GxXP3n8yGGfPssqTnqnmHaPOlVJZbLmywdpBEW5PFhT2SmkUy5bp4aqxRcrrBzWLZPnWT1o3AcZOrgU")]
        public void ValidateApi_GoodApiKey(string value)
        {
            var controller = new CodereTvmazeController(null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Add("Authorization", "ApiKey: " + value);
            var response = controller.ValidateApi(value);
            NUnit.Framework.Assert.That(response == Results.Ok(), "Api is correct.");
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow.Reporting.NUnitExecutionReport;

namespace ReportingTests
{
    [TestClass]
    public class NUnitExecutionReport
    {
        [TestMethod]
        public void GenerateNUnitExecutionReportWithoutProjectFile()
        {
            var reportParameters = new NUnitExecutionReportParameters(
                                    "Test Project", 
                                    @"TestFiles\TestResult.xml", 
                                    @"TestFiles\TestResult.txt", 
                                    "TestResult.html");
            var generator = new NUnitExecutionReportGenerator(reportParameters);
            generator.GenerateAndTransformReport();
        }
    }
}

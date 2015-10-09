using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Project;

namespace TechTalk.SpecFlow.Reporting.NUnitExecutionReport
{
    public abstract class NUnitBasedExecutionReportGenerator
    {
        protected abstract ReportParameters ReportParameters { get; }

        protected virtual Type ReportType
        {
            get { return GetType(); }
        }

        private void TransformReport(ReportElements.NUnitExecutionReport report, SpecFlowProject specFlowProject)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReportElements.NUnitExecutionReport), 
                                                         ReportElements.NUnitExecutionReport.XmlNamespace);

            if (XsltHelper.IsXmlOutput(ReportParameters.OutputFile))
            {
                XsltHelper.TransformXml(serializer, report, ReportParameters.OutputFile);
            }
            else
            {
                XsltHelper.TransformHtml(serializer, report, ReportType, 
                                         ReportParameters.OutputFile, specFlowProject.Configuration.GeneratorConfiguration, 
                                         ReportParameters.XsltFile);
            }
        }

        public void GenerateAndTransformReport()
        {
            ReportElements.NUnitExecutionReport report = null;
            SpecFlowProject specFlowProject = null;

            if (!string.IsNullOrEmpty(ReportParameters.ProjectFile))
            {
                specFlowProject = MsBuildProjectReader.LoadSpecFlowProjectFromMsBuild(ReportParameters.ProjectFile);
                report = GenerateReport(specFlowProject);
            }
            else
            {
                report = GenerateReport(ReportParameters.ProjectName);
            }

            TransformReport(report, specFlowProject);
        }

        protected virtual ReportElements.NUnitExecutionReport GenerateReport(SpecFlowProject specFlowProject)
        {
            return GenerateReport(specFlowProject.ProjectSettings.ProjectName);
        }

        protected virtual ReportElements.NUnitExecutionReport GenerateReport(string projectName)
        {
            var report = new ReportElements.NUnitExecutionReport();
            report.ProjectName = projectName;
            report.GeneratedAt = DateTime.Now.ToString("g", CultureInfo.InvariantCulture);

            XmlDocument xmlTestResult = LoadXmlTestResult();
            report.NUnitXmlTestResult = xmlTestResult.DocumentElement;

            ExtendReport(report);

            return report;
        }

        protected virtual void ExtendReport(ReportElements.NUnitExecutionReport report)
        {
            
        }

        protected abstract XmlDocument LoadXmlTestResult();
    }
}
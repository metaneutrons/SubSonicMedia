// <copyright file="JUnitReportHelper.cs" company="Fabian Schmieder">
// This file is part of SubSonicMedia.
//
// SubSonicMedia is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SubSonicMedia is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SubSonicMedia. If not, see https://www.gnu.org/licenses/.
// </copyright>
using System.Xml;

using SubSonicMedia.TestKit.Models;
using SubSonicMedia.TestKit.Tests;

namespace SubSonicMedia.TestKit.Helpers
{
    /// <summary>
    /// Helper class for generating JUnit XML reports from test results.
    /// </summary>
    public static class JUnitReportHelper
    {
        /// <summary>
        /// Generates a JUnit XML report from test results.
        /// </summary>
        /// <param name="results">Dictionary containing test results.</param>
        /// <param name="tests">The test instances with timing and exception information.</param>
        /// <param name="totalTime">Total execution time in milliseconds.</param>
        /// <param name="outputDirectory">Directory where the report should be saved.</param>
        public static void GenerateJUnitXmlReport(
            Dictionary<string, TestResult> results,
            IEnumerable<TestBase> tests,
            long totalTime,
            string outputDirectory
        )
        {
            try
            {
                // Create XML document
                var doc = new XmlDocument();

                // Create XML declaration
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(xmlDeclaration);

                // Create testsuite element
                XmlElement testSuite = doc.CreateElement("testsuite");
                doc.AppendChild(testSuite);

                // Set testsuite attributes
                int totalTests = results.Count;
                int passCount = results.Count(r => r.Value == TestResult.Pass);
                int failCount = results.Count(r => r.Value == TestResult.Fail);
                int skipCount = results.Count(r => r.Value == TestResult.Skipped);

                testSuite.SetAttribute("name", "SubSonicMedia API Tests");
                testSuite.SetAttribute("tests", totalTests.ToString());
                testSuite.SetAttribute("failures", failCount.ToString());
                testSuite.SetAttribute("errors", "0");
                testSuite.SetAttribute("skipped", skipCount.ToString());
                testSuite.SetAttribute("time", (totalTime / 1000.0).ToString("0.000"));
                testSuite.SetAttribute(
                    "timestamp",
                    DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")
                );

                // Index tests by name for easier lookup
                var testsByName = tests.ToDictionary(t => t.Name);

                // Create test cases
                foreach (var (testName, result) in results)
                {
                    XmlElement testCase = doc.CreateElement("testcase");
                    testSuite.AppendChild(testCase);

                    testCase.SetAttribute("name", testName);
                    testCase.SetAttribute("classname", "SubSonicMedia.TestKit.Tests");

                    // Get the test for timing information
                    if (testsByName.TryGetValue(testName, out var test))
                    {
                        // Time should be in seconds for JUnit XML
                        double timeInSeconds = test.ElapsedMilliseconds / 1000.0;
                        testCase.SetAttribute("time", timeInSeconds.ToString("0.000"));
                    }
                    else
                    {
                        testCase.SetAttribute("time", "0.000");
                    }

                    // Add appropriate child elements based on result
                    switch (result)
                    {
                        case TestResult.Fail:
                            XmlElement failure = doc.CreateElement("failure");
                            testCase.AppendChild(failure);

                            string errorMessage = "Test failed";
                            string errorType = "AssertionError";

                            // Include error details if available
                            if (
                                testsByName.TryGetValue(testName, out var failedTest)
                                && failedTest.LastException != null
                            )
                            {
                                errorMessage = failedTest.LastException.Message;
                                errorType = failedTest.LastException.GetType().Name;
                                failure.InnerText = failedTest.LastException.ToString();
                            }

                            failure.SetAttribute("message", errorMessage);
                            failure.SetAttribute("type", errorType);
                            break;
                        case TestResult.Skipped:
                            XmlElement skipped = doc.CreateElement("skipped");
                            testCase.AppendChild(skipped);

                            string skipMessage = "Test skipped - API feature not available";

                            // Include skip reason if available
                            if (
                                testsByName.TryGetValue(testName, out var skippedTest)
                                && skippedTest.LastException != null
                            )
                            {
                                skipMessage = skippedTest.LastException.Message;
                            }

                            skipped.SetAttribute("message", skipMessage);
                            break;
                        case TestResult.Pass:
                            // No need to add anything for pass
                            break;
                    }
                }

                // Save the report
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string reportPath = Path.Combine(outputDirectory, $"junit_report_{timestamp}.xml");

                // Ensure directory exists
                Directory.CreateDirectory(outputDirectory);

                // Save the document
                doc.Save(reportPath);

                ConsoleHelper.LogInfo($"JUnit XML report generated at: {reportPath}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Failed to generate JUnit XML report: {ex.Message}");
            }
        }
    }
}

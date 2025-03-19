// <copyright file="TestResult.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Models;

namespace SubSonicMedia.TestKit.Models
{
    /// <summary>
    /// Represents the result of a test execution.
    /// </summary>
    public enum TestResult
    {
        /// <summary>
        /// The test was executed successfully.
        /// </summary>
        Pass,

        /// <summary>
        /// The test failed during execution.
        /// </summary>
        Fail,

        /// <summary>
        /// The test was skipped due to feature being unavailable or not implemented.
        /// </summary>
        Skipped,
    }
}

// <copyright file="TestInitializer.cs" company="Fabian Schmieder">
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

using SubSonicMedia.Tests.Helpers;
using Xunit;

namespace SubSonicMedia.Tests
{
    /// <summary>
    /// Handles test initialization tasks like syncing TestKit outputs.
    /// </summary>
    public class TestInitializer
    {
        private static bool _hasInitialized = false;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes test resources. This is called automatically when any test collection is created.
        /// </summary>
        public static void Initialize()
        {
            if (!_hasInitialized)
            {
                lock (_lock)
                {
                    if (!_hasInitialized)
                    {
                        try
                        {
                            int filesCopied = TestKitResponseLoader.SyncTestKitOutputs();
                            Console.WriteLine(
                                $"Synchronized {filesCopied} test response files from TestKit"
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"Warning: Failed to sync TestKit outputs: {ex.Message}"
                            );
                            Console.WriteLine(
                                "Make sure to run the TestKit with RECORD_TEST_RESULTS=true first."
                            );
                        }

                        _hasInitialized = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Collection fixture that ensures test initialization is performed.
    /// </summary>
    [CollectionDefinition("TestInit")]
    public class TestInitCollection : ICollectionFixture<TestInitFixture> { }

    /// <summary>
    /// Fixture that performs initialization when the collection is created.
    /// </summary>
    public class TestInitFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestInitFixture"/> class.
        /// </summary>
        public TestInitFixture()
        {
            TestInitializer.Initialize();
        }
    }
}

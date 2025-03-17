// <copyright file="ChatTest.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System.Linq;
using Spectre.Console;
using SubSonicMedia.TestKit.Helpers;
using SubSonicMedia.TestKit.Models;

namespace SubSonicMedia.TestKit.Tests
{
    /// <summary>
    /// Tests chat capabilities of the Subsonic API.
    /// </summary>
    public class ChatTest : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatTest"/> class.
        /// </summary>
        /// <param name="client">The Subsonic client.</param>
        /// <param name="settings">The application settings.</param>
        public ChatTest(SubsonicClient client, AppSettings settings)
            : base(client, settings) { }

        /// <inheritdoc/>
        public override string Name => "Chat Test";

        /// <inheritdoc/>
        public override string Description => "Tests retrieving and adding chat messages";

        /// <inheritdoc/>
        protected override async Task<TestResult> ExecuteTestAsync()
        {
            bool allTestsPassed = true;

            // Test 1: Get Chat Messages
            ConsoleHelper.LogInfo("Testing GetChatMessages...");
            try
            {
                var chatResponse = await this.Client.Chat.GetChatMessagesAsync();
                this.RecordTestResult(chatResponse, "chat_messages");

                if (chatResponse.IsSuccess)
                {
                    int messageCount = chatResponse.ChatMessages?.Message?.Count ?? 0;
                    ConsoleHelper.LogSuccess(
                        $"Successfully retrieved {messageCount} chat messages"
                    );

                    if (messageCount > 0 && chatResponse.ChatMessages?.Message != null)
                    {
                        // Display the chat messages in a table
                        var table = new Table();
                        table.AddColumn("User");
                        table.AddColumn("Time");
                        table.AddColumn("Message");

                        foreach (var message in chatResponse.ChatMessages.Message.Take(5))
                        {
                            table.AddRow(
                                message.Username ?? "Unknown",
                                message.Time.ToString(),
                                message.Message ?? "Empty message"
                            );
                        }

                        AnsiConsole.Write(table);
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to get chat messages: {chatResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error getting chat messages: {ex.Message}");
                allTestsPassed = false;
            }

            // Test 2: Add Chat Message
            ConsoleHelper.LogInfo("Testing AddChatMessage...");
            try
            {
                string testMessage =
                    $"Test message from SubSonicMedia.TestKit at {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var addResponse = await this.Client.Chat.AddChatMessageAsync(testMessage);

                if (addResponse.IsSuccess)
                {
                    ConsoleHelper.LogSuccess("Successfully added chat message");

                    // Verify the message was added by fetching messages again
                    var verifyResponse = await this.Client.Chat.GetChatMessagesAsync();

                    if (
                        verifyResponse.IsSuccess
                        && verifyResponse.ChatMessages?.Message != null
                        && verifyResponse.ChatMessages.Message.Count > 0
                    )
                    {
                        bool messageFound = verifyResponse.ChatMessages.Message.Any(m =>
                            m.Message == testMessage
                        );

                        if (messageFound)
                        {
                            ConsoleHelper.LogSuccess(
                                "Confirmed that the message was added successfully"
                            );
                        }
                        else
                        {
                            ConsoleHelper.LogWarning(
                                "Message was added successfully, but couldn't be found in the chat history"
                            );
                        }
                    }
                }
                else
                {
                    ConsoleHelper.LogError(
                        $"Failed to add chat message: {addResponse.Error?.Message}"
                    );
                    allTestsPassed = false;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.LogError($"Error adding chat message: {ex.Message}");
                allTestsPassed = false;
            }

            return allTestsPassed ? TestResult.Pass : TestResult.Fail;
        }
    }
}

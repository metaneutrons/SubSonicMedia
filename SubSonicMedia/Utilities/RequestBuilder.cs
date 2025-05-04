// <copyright file="RequestBuilder.cs" company="Fabian Schmieder">
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
using System.Text;

namespace SubSonicMedia.Utilities
{
    /// <summary>
    /// Builder for constructing Subsonic API request URLs with parameters.
    /// </summary>
    internal class RequestBuilder
    {
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();
        private readonly string _endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBuilder"/> class.
        /// </summary>
        /// <param name="endpoint">The API endpoint name.</param>
        public RequestBuilder(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));
            }

            this._endpoint = endpoint;
        }

        /// <summary>
        /// Adds a string parameter to the request.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>This instance for method chaining.</returns>
        public RequestBuilder AddParameter(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this._parameters[name] = value;
            }

            return this;
        }

        /// <summary>
        /// Adds a nullable value parameter to the request.
        /// </summary>
        /// <typeparam name="T">The type of the parameter value.</typeparam>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>This instance for method chaining.</returns>
        public RequestBuilder AddParameter<T>(string name, T? value)
            where T : struct
        {
            if (value.HasValue)
            {
                this._parameters[name] = value.Value.ToString() ?? string.Empty;
            }

            return this;
        }

        /// <summary>
        /// Adds a boolean parameter to the request.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>This instance for method chaining.</returns>
        public RequestBuilder AddParameter(string name, bool? value)
        {
            if (value.HasValue)
            {
                this._parameters[name] = value.Value ? "true" : "false";
            }

            return this;
        }

        /// <summary>
        /// Adds a date/time parameter to the request as a Unix timestamp.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>This instance for method chaining.</returns>
        public RequestBuilder AddParameter(string name, DateTime? value)
        {
            if (value.HasValue)
            {
                var unixTimestamp = new DateTimeOffset(value.Value).ToUnixTimeMilliseconds();
                this._parameters[name] = unixTimestamp.ToString();
            }

            return this;
        }

        /// <summary>
        /// Adds a collection of string parameters to the request.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="values">The parameter values.</param>
        /// <returns>This instance for method chaining.</returns>
        public RequestBuilder AddParameters(string name, IEnumerable<string> values)
        {
            if (values != null)
            {
                var nonEmptyValues = values.Where(v => !string.IsNullOrEmpty(v)).ToList();
                if (nonEmptyValues.Any())
                {
                    for (var i = 0; i < nonEmptyValues.Count; i++)
                    {
                        this._parameters[$"{name}"] = string.Join(",", nonEmptyValues);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Adds a collection of integer parameters to the request.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="values">The parameter values.</param>
        /// <returns>This instance for method chaining.</returns>
        public RequestBuilder AddParameters(string name, IEnumerable<int> values)
        {
            if (values != null)
            {
                var valuesList = values.ToList();
                if (valuesList.Any())
                {
                    for (var i = 0; i < valuesList.Count; i++)
                    {
                        this._parameters[$"{name}"] = string.Join(",", valuesList);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Builds the complete request URL.
        /// </summary>
        /// <returns>The request URL.</returns>
        public string BuildRequestUrl()
        {
            var queryString = new StringBuilder();
            foreach (var parameter in this._parameters)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append('&');
                }

                queryString.Append(Uri.EscapeDataString(parameter.Key));
                queryString.Append('=');
                queryString.Append(Uri.EscapeDataString(parameter.Value));
            }

            return $"rest/{this._endpoint}.view?{queryString}";
        }
    }
}

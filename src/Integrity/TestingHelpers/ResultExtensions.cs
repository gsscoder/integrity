using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RailwaySharp;
using Integrity;

namespace Integrity.TestingHelpers
{
    /// <summary>Trivial testing helper, future versions will extend FluentAssertions.</summary>
    public static class ResultExtensions
    {
        public static void ShouldBeProved(this Result<IEnumerable<Evidence>, string> result)
        {
            if (result.GetType().Equals(typeof(Ok<IEnumerable<Evidence>, string>))) return;

            var fail = (Bad<IEnumerable<Evidence>, string>)result;
            var builder = new StringBuilder(32 + 12 * fail.Messages.Count());
            builder.Append("Integrity not proven:\n");
            foreach (var message in fail.Messages) {
                builder.Append($"\t{message}");
            }
            throw new Exception(builder.ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace MSBuildProjectTools.LanguageServer.Tests
{
    using SemanticModel;
    using Xunit;

    /// <summary>
    ///     Tests for <see cref="XSPath"/> and <see cref="XSPathSegment"/>.
    /// </summary>
    public class XSPathTests
        : TestBase
    {
        /// <summary>
        ///     Create a new <see cref="XSPath"/> test suite.
        /// </summary>
        /// <param name="testOutput"></param>
        public XSPathTests(ITestOutputHelper testOutput)
            : base(testOutput)
        {
        }

        /// <summary>
        ///     Verify that <see cref="XSPath.Parse(string)"/> can parse an absolute path.
        /// </summary>
        /// <param name="path">
        ///     The path to parse.
        /// </param>
        /// <param name="expectedSegmentCount">
        ///     The expected number of <see cref="XSPathSegment"/>s in the resulting path.
        /// </param>
        [InlineData("/",     1)]
        [InlineData("/A",    2)]
        [InlineData("/A/",   2)]
        [InlineData("/A/B",  3)]
        [InlineData("/A/B/", 3)]
        [Theory(DisplayName = "XSPath can parse absolute path ")]
        public void Can_Parse_Path_Absolute(string path, int expectedSegmentCount)
        {
            XSPath actual = XSPath.Parse(path);
            Assert.NotNull(actual);

            Assert.True(actual.IsAbsolute, "IsAbsolute");
            Assert.Equal(expectedSegmentCount, actual.Segments.Count);
        }

        /// <summary>
        ///     Verify that <see cref="XSPath.Parse(string)"/> can parse a relative path.
        /// </summary>
        /// <param name="path">
        ///     The path to parse.
        /// </param>
        /// <param name="expectedSegmentCount">
        ///     The expected number of <see cref="XSPathSegment"/>s in the resulting path.
        /// </param>
        [InlineData("A",    1)]
        [InlineData("A/",   1)]
        [InlineData("A/B",  2)]
        [InlineData("A/B/", 2)]
        [Theory(DisplayName = "XSPath can parse relative path ")]
        public void Can_Parse_Path_Relative(string path, int expectedSegmentCount)
        {
            XSPath actual = XSPath.Parse(path);
            Assert.NotNull(actual);

            Assert.True(actual.IsRelative, "IsRelative");
            Assert.Equal(expectedSegmentCount, actual.Segments.Count);
        }

        /// <summary>
        ///     Verify that <see cref="XSPath"/> can append a string representing a relative path segment.
        /// </summary>
        /// <param name="path">
        ///     The original path.
        /// </param>
        /// <param name="segment">
        ///     The segment to append.
        /// </param>
        /// <param name="expectedPath">
        ///     The expected resulting path.
        /// </param>
        [InlineData("/",  "A", "/A"  )]
        [InlineData("/A", "B", "/A/B")]
        [Theory(DisplayName = "XSPath can append relative string segment ")]
        public void Can_Append_String_Segment_To_Path_Relative(string path, string segment, string expectedPath)
        {
            XSPath actual = XSPath.Parse(path);
            actual += segment;

            Assert.Equal(expectedPath, actual.Path);
        }

        /// <summary>
        ///     Verify that <see cref="XSPath"/> can append a string representing an absolute path segment.
        /// </summary>
        /// <param name="path">
        ///     The original path.
        /// </param>
        /// <param name="segment">
        ///     The segment to append.
        /// </param>
        /// <param name="expectedPath">
        ///     The expected resulting path.
        /// </param>
        [InlineData("/",    "/A",   "/A"  )]
        [InlineData("/A",   "/B",   "/B"  )]
        [InlineData("/A/B", "/C/D", "/C/D")]
        [Theory(DisplayName = "XSPath can append relative string segment ")]
        public void Can_Append_String_Segment_To_Path_Absolute(string path, string segment, string expectedPath)
        {
            XSPath actual = XSPath.Parse(path);
            actual += segment;

            Assert.Equal(expectedPath, actual.Path);
        }
    }
}
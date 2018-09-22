using NUnit.Framework;
using SearchEngineCompanySort;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class ProgramArgsSpec
    {
        [SetUp]
        public void Setup(){}

        [Test]
        public void Should_parse_cli_args(){
            var args = new string[]{ "32", "log1.txt", "log2.txt" };
            var result = ProgramArgs.ParseFromCommandLine(args);

            Assert.AreEqual(result.MemoryLimit, 32);
            Assert.AreEqual(result.FileNames, new string[]{ "log1.txt", "log2.txt" });
        }
    }
}
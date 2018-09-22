using NUnit.Framework;
using SearchEngineCompanySort;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class FileSplitterSpec
    {
        [SetUp]
        public void Setup(){}

        [Test]
        public void Split_stream_into_sorted_files_when_memeory_limit_higher_than_file_content(){
            int memLimit = 5;
            var chunkWriter = new ChunkWriterMock();
            var fileSplitter = new FileSplitter();
            var sourceFile = new TextReaderMock(new string[3]{ "zebra", "elephant", "ant" });
            var expectedChunk = new List<string[]> {
                new string[] { "ant", "elephant", "zebra" }
            };
            fileSplitter.SplitStreamIntoSortedFiles(sourceFile, memLimit, chunkWriter);
            
            Assert.AreEqual(expectedChunk, chunkWriter.SavedChunks);
        }

        [Test]
        public void Should_split_stream_into_sorted_files_when_memeory_limit_lower_then_file_content()
        {
            int memLimit = 2;
            var chunkWriter = new ChunkWriterMock();
            var fileSplitter = new FileSplitter();
            var sourceFile = new TextReaderMock(new string[3]{ "zebra", "elephant", "ant" });
            var expectedChunks = new List<string[]> {
                new string[] { "elephant", "zebra" },
                new string[] { "ant" }
            };
            fileSplitter.SplitStreamIntoSortedFiles(sourceFile, memLimit, chunkWriter);
            
            Assert.AreEqual(expectedChunks, chunkWriter.SavedChunks);
        }
    }
}
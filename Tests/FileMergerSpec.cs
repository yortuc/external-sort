using NUnit.Framework;
using SearchEngineCompanySort;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class FileMergerSpec
    {
        [SetUp]
        public void Setup(){}

        [Test]
        public void Should_create_a_batch_from_files_respecting_memory_limit(){
            int memLimit = 12;
            var sortedFiles = new List<string> {
                "file1.txt", "file2.txt", "file3.txt", "file4.txt", "file5.txt"
            };
            var fileMerger = new FileMerger();
            var batchCount = fileMerger.GetFilesBatch(sortedFiles, memLimit).Count();
            Assert.AreEqual(1, batchCount);
        }

        [Test]
        public void Should_create_batches_from_files_respecting_memory_limit(){
            int memLimit = 2;
            var sortedFiles = new List<string> {
                "file1.txt", "file2.txt", "file3.txt", "file4.txt", "file5.txt"
            };
            var fileMerger = new FileMerger();
            var batchCount = fileMerger.GetFilesBatch(sortedFiles, memLimit).Count();
            Assert.AreEqual(3, batchCount);
        }

        [Test]
        public void Should_handle_a_single_chunk_merge_correctly()
        {
            var sortedFiles = new List<IChunkReader>(){
                new ChunkReaderMock(new string[]{ "earth", "zion" })
            };
            var resultWriter = new ChunkWriterMock();
            var fileMerger = new FileMerger();
            fileMerger.MergeSortedChunks(sortedFiles, resultWriter);

            Assert.AreEqual(
                resultWriter.writtenLines,
                new string[]{ "earth", "zion" }
            );
        }

        [Test]
        public void Should_merge_sorted_chunks_correctly()
        {
            var sortedFiles = new List<IChunkReader>(){
                new ChunkReaderMock(new string[]{ "earth", "zion" }),
                new ChunkReaderMock(new string[]{ "moon", "world" }),
                new ChunkReaderMock(new string[]{ "apple" })
            };
            var resultWriter = new ChunkWriterMock();
            var fileMerger = new FileMerger();
            fileMerger.MergeSortedChunks(sortedFiles, resultWriter);

            Assert.AreEqual(
                resultWriter.writtenLines,
                new string[]{ "apple", "earth", "moon", "world", "zion" }
            );
        }

        [Test]
        public void Should_merge_sorted_files()
        {
            var sortedFiles = new List<string>(){ "file1.txt", "file2.txt", "file3.txt" };
            var fileMerger = new FileMergerMock();
            var resultWriter = new ChunkWriterMock();

            fileMerger.MergeSortedFiles(
                fileNames: sortedFiles, 
                memLimit: 3, 
                resultWriter: resultWriter
            );

            Assert.AreEqual(1, resultWriter.CreatedFilesCount);
        }

        /// <summary>
        /// Given 12 files and 3 memory limit, there should be 6 merge operations
        ///    First iteration, 12 -> 4 merge 
        ///    Second iteration, 4 -> 1 merge (merge 3 files and 1 file remains the same for next iteration)
        ///    Third iteration, 2 -> 1 merge
        /// </summary>
        [Test]
        public void Should_merge_sorted_files_recursively()
        {
            var sortedFiles = new List<string>(){ 
                "file1.txt", "file2.txt", "file3.txt", 
                "file4.txt", "file5.txt", "file6.txt",
                "file7.txt", "file8.txt", "file9.txt",
                "file10.txt", "file11.txt", "file12.txt"  
            };
            var fileMerger = new FileMergerMock();
            var resultWriter = new ChunkWriterMock();

            fileMerger.MergeSortedFiles(
                fileNames: sortedFiles, 
                memLimit: 3, 
                resultWriter: resultWriter
            );

            Assert.AreEqual(6, resultWriter.CreatedFilesCount);
        }
    }
}
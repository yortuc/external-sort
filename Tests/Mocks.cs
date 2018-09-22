using NUnit.Framework;
using SearchEngineCompanySort;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    class TextReaderMock : TextReader {
        private int readIndex = 0;
        private string[] buffer;

        public TextReaderMock(string[] buffer){
            this.buffer = buffer;
        }

        public override string ReadLine() {
            return (readIndex == buffer.Length) ? null : buffer[readIndex++];
        }
    }

    class ChunkReaderMock : IChunkReader {
        private int readIndex = 0;
        private string[] buffer = new string[2]{ "term1", "term2" };
        public string value;

        public ChunkReaderMock(string[] buffer){
            this.buffer = buffer;
        }

        public void Read(){
            this.value = (readIndex == buffer.Length) ? null : buffer[readIndex++];
        }
        public string GetValue(){ return this.value; }
        public string GetFileName(){ return ""; }
    } 

    class ChunkWriterMock : IChunkWriter {
        public List<string[]> SavedChunks = new List<string[]>();
        public List<string> writtenLines = new List<string>();
        public bool isClosed = false;
        public int CreatedFilesCount = 0;

        public string SaveChunk(string[] chunk){ 
            SavedChunks.Add(chunk.Where(s => s != null).ToArray());
            return "mock-file-name";
        }
        public void WriteLine(string line) {
            writtenLines.Add(line);
        }
        public string GetFileName(){ return ""; }
        public void NewFile() {
            CreatedFilesCount++;
            writtenLines = new List<string>(); 
        }
        public void Close() {}
    }

    class FileMergerMock : FileMerger {
        override public IChunkReader CreateChunkReaderFromFile(string fileName){
            return new ChunkReaderMock(new string[]{ "apple" });
        }
    }
}
using System;
using System.IO;

namespace SearchEngineCompanySort {

    public class ChunkReader : IChunkReader {
        private StreamReader sr;
        private string value;
        private string fileName;

        public ChunkReader(string fileName){
            sr = new StreamReader(fileName);
            this.fileName = fileName;
        }
        
        /// <summary>
        /// Reads a single line from sorted file and 
        /// updates current value
        /// Only one search term is held at a time in memory
        /// </summary>
        public void Read(){
            value = sr.ReadLine();
            if (value == null) sr.Close();
        }
        
        /// <returns>
        /// The current value
        /// </returns>
        public string GetValue(){
            return value;
        }

        public string GetFileName(){
            return fileName;
        }
    }
}
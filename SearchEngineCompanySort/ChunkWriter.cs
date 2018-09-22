using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngineCompanySort {

    public class ChunkWriter: IChunkWriter { 
        private StreamWriter sr;
        private string outputFileName;

        public ChunkWriter() {}

        public ChunkWriter(string outputFileName){
            this.outputFileName = outputFileName;
            this.sr = new StreamWriter(outputFileName);
        }

        public string GetFileName(){
            return outputFileName;
        }

        /// <summary>
        /// Save given string array to disk with a random guid file name
        /// </summary>
        /// <returns>
        /// Path of file saved
        /// </returns>
        public string SaveChunk(string[] chunk){
            string fileName = "./tmp_output/" + Guid.NewGuid().ToString() + ".txt";

            using (StreamWriter sr = new StreamWriter(fileName)) {
                foreach (var line in chunk){
                    if(line != null) {
                        sr.WriteLine(line);
                    }
                }
            }
            return fileName;
        }

        /// <summary>
        /// Writes a single file to the open file stream
        /// </summary>
        public void WriteLine(string line){
            sr.WriteLine(line);
        }

        /// <summary>
        /// Closes file stream
        /// </summary>
        public void Close(){
            sr.Close();
        }

        public void NewFile(){
            outputFileName = "./tmp_output/r_" + Guid.NewGuid().ToString() + ".txt";
            if(sr != null) {
                sr.Close();
            } 
            sr = new StreamWriter(outputFileName);
        }
    }
}
namespace SearchEngineCompanySort {

    public interface IChunkWriter{
        string SaveChunk(string[] chunk);
        void WriteLine(string line);
        void Close();
        string GetFileName();
        void NewFile();
    }

}
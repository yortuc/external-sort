namespace SearchEngineCompanySort {

    public interface IChunkReader{
        string GetValue();
        void Read();
        string GetFileName();
    }

}
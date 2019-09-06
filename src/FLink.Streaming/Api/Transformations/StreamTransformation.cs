namespace FLink.Streaming.Api.Transformations
{
    public abstract class StreamTransformation<T>  
    {
        protected static int IdCounter = 0;

        public static int GetNewNodeId()
        {
            return ++IdCounter;
        }
    }
}

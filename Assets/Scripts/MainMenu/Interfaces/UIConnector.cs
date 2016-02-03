namespace Seni.UI{        
    public interface UIConnectorStart{
        void StartConnection();
    }
    public interface UIConnectionResponse{        
        void Connected();
        void ConnectionFail();
    }
}
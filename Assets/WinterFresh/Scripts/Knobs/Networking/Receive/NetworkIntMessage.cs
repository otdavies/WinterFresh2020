    public struct NetworkIntMessage
    {
        public enum MessageType 
        {
            STATE=0, COLOR=1, KNOBS=2
        }

        public MessageType MsgType;
        public int[] data;

        public NetworkIntMessage(MessageType messageType, int[] data)
        {
            this.MsgType = messageType;
            this.data = data;
        }
    }

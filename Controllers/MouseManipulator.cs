using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SmartPCServer.MouseManipulator
{
    public class VirtualMouse
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        private int x;
        private int y;
        
        [DataContract]
        public class MouseCommand
        {
            [DataMember]
            public int x;

            [DataMember]
            public int y;

            [DataMember]
            public string command;


            public override string ToString()
            {
                string o = "";
                o += "{{{ Command: " + command;
                o += " X: " + x;
                o += " Y: " + y + " }}}";

                return o;
            }
        }
        public VirtualMouse(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;

            MoveTo(x, y);
        }

        public static MouseCommand JsonToMouseCommand(string jsonString)
        {
            MouseCommand outVector = new MouseCommand();
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(outVector.GetType());
            outVector = serializer.ReadObject(mStream) as MouseCommand;
            mStream.Close();

            return outVector;

        }

        public static string MouseCommandToJson(MouseCommand vector)
        {
            MemoryStream mStream = new MemoryStream();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MouseCommand));
            serializer.WriteObject(mStream, vector);
            byte[] jsonByteArray = mStream.ToArray();
            mStream.Close();

            return Encoding.UTF8.GetString(jsonByteArray, 0, jsonByteArray.Length);
        }

        public void Move(int xDelta, int yDelta)
        {
            x += xDelta;
            y += yDelta;
            mouse_event(MOUSEEVENTF_MOVE, xDelta, yDelta, 0, 0);
        }
        public void MoveTo(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x, y, 0, 0);
        }
        public void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, this.x, this.y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, this.x, this.y, 0, 0);
        }

        public void LeftDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, this.x, this.y, 0, 0);
        }

        public void LeftUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, this.x, this.y, 0, 0);
        }

        public void RightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, this.x, this.y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, this.x, this.y, 0, 0);
        }

        public void RightDown()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, this.x, this.y, 0, 0);
        }

        public void RightUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, this.x, this.y, 0, 0);
        }
    }
}
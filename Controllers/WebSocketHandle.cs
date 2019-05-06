using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SmartPCServer.MouseManipulator;

namespace SmartPCServer.Controllers
{
    public class WebSocketHandle
    {
        private WebSocket webSocket;
        private AudioManipulator audio;
        private VirtualMouse mouse;

        public byte[] buffer;

        public object Enconding { get; private set; }

        public WebSocketHandle(WebSocket webSocket)
        {
            // 4kb buffer
            buffer = new byte[1024 * 4];
            this.webSocket = webSocket;

            mouse = new VirtualMouse();
            audio = new AudioManipulator();
        }




        async Task SendMessage(string Message)
        {
            var bytes = Encoding.ASCII.GetBytes(Message);
            var arraySegment = new ArraySegment<byte>(bytes);

            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public Task ParseMessageReceived(WebSocketReceiveResult result)
        {
            string messageReceived = Encoding.Default.GetString(new ArraySegment<byte>(buffer, 0, result.Count));

            VirtualMouse.MouseCommand mc = VirtualMouse.JsonToMouseCommand(messageReceived);
            
            //Console.WriteLine("Parsing Command: " + mc);

            if (mc.command == "move")
            {
                mouse.Move(mc.x, mc.y);
            }

            else if(mc.command == "right_click")
            {
                mouse.RightClick();
            }
            else if(mc.command == "left_click")
            {
                mouse.LeftClick();
            }
            else if(mc.command == "left_down")
            {
                mouse.LeftDown();
            }
            else if(mc.command == "left_up")
            {
                mouse.LeftUp();
            }
            else if (mc.command == "right_down")
            {
                mouse.RightDown();
            }
            else if (mc.command == "right_up")
            {
                mouse.RightUp();
            }
            else if(mc.command == "touchpad_start")
            {
                mouse.TouchpadStart(mc.x, mc.y);
            }
            else if(mc.command == "touchpad_move")
            {
                mouse.TouchpadMove(mc.x, mc.y);
            }
            else if(mc.command == "set_volume")
            {
                audio.SetVolume(mc.x);
            }
            else
            {
                return Error();
            }

            return Success();
        }

        async Task Success()
        {
            await SendMessage("1");
        }
        async Task Error()
        {
            await SendMessage("0");
        }
    }
}

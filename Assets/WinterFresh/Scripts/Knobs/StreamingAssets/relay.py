# Oliver Davies, 2/6/20, WinterFresh
import asyncio
import websockets
import socket

UDP_IP = "127.0.0.1"
UDP_PORT = 7777

async def relay(websocket, path):
    try:
        async for message in websocket:
            print("Forwarded Message: " + message)
            # Send UDP
            sock.sendto(message.encode(), (UDP_IP, UDP_PORT))
    except Exception as e:
        print("Connection failed")
        print(e)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) # UDP
start_server = websockets.serve(relay, "0.0.0.0", 6969)
asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever() 
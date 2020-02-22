# Oliver Davies, 2/6/20, WinterFresh
import asyncio
import websockets
import socket
import time

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
		
		
def send_test(msg):
	sock.sendto(msg.encode(), (UDP_IP, UDP_PORT))
	
def test_all_portals():
	send_test("S, 1")
	send_test("C, 0")
	time.sleep(3)

	send_test("S, 1")
	send_test("C, 1")
	time.sleep(3)

	send_test("S, 1")
	send_test("C, 2")
	time.sleep(3)

	send_test("S, 1")
	send_test("C, 3")
	time.sleep(3)

	send_test("S, 1")
	send_test("C, 4")
	
	
def test_all_knobs():
	send_test("S, 1")
	send_test("C, 2")
	send_test("K,0,0");
	send_test("K,1,0");
	send_test("K,2,0");
	send_test("K,3,0");
	time.sleep(5)
	for x in range(0, 4):
		for y in range(0, 6):
			send_test("K," + str(x)+","+str((y*3)))
			time.sleep(0.7)
		time.sleep(0.7)



sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM) # UDP


#test_all_portals()
test_all_knobs()




start_server = websockets.serve(relay, "0.0.0.0", 6969)
asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever() 
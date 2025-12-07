import clr
import sys
import threading
import keyboard

#import pythonnet
#print (pythonnet.get_runtime_info())

# Add the directory containing your C# DLL to the assembly search path
sys.path.append('.\\mozaAPI_python_wrapper\\bin\\Release') 
# Load the C# assembly
clr.AddReference("mozaAPI_python_wrapper")
# Import the C# namespace and class
from PythonMozaAPI import Moza_functions
moza = Moza_functions()


# Function to continuously update control values
def update_controls():
    global throttle, brake, steering
    while True:
        HIDData= moza.pygetHIDData()
        steering=round(HIDData["steeringangle"],2)
        throttle=round(HIDData["throttle"],2)
        brake=round(HIDData["brake"],2)
        #print (round(HIDData["steeringangle"],2), round(HIDData["throttle"],2), round(HIDData["brake"],2))
def upate_switches():
    global currentSwitchValue
    while True:
        currentSwitchValue=moza.pyGetSwitchValues()
        #print ("currentSwitchValue ", currentSwitchValue, " is ON")


# Initialize control variables
throttle=0
brake=0
steering=0
currentSwitchValue=0

# Turn on spring force
moza.pySetSpringForce(None)

# Start the device monitoring threads
thread1 = threading.Thread(target=update_controls, args=())
thread2 = threading.Thread(target=upate_switches, args=())                          
thread1.start()
thread2.start()

# Main loop to print the control values
while (True):
    print ("steering:", steering, " throttle:", throttle, " brake:", brake, " switch value:", currentSwitchValue)
    if keyboard.is_pressed('q'):
        break
# Turn off spring force before exiting
moza.pySetSpringForce(None,1000)
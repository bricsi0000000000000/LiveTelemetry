import os
import time
import requests
import json
import warnings
from datetime import datetime
from requests.exceptions import ConnectionError

from sensor import Sensor
from sensor_value import SensorValue
from package import Package
from complex_encoder import ComplexEncoder


# ---------------------CONFIGURATION--------------------------------------------------------------------------------------------

configuration = json.loads(open("../configuration_files/configuration.json", "r").read())

if configuration['ignore_warnings'] == True:
  warnings.filterwarnings("ignore")

HTTP_STATUS_CODE_OK = int(configuration['HTTP_status_code_ok'])
WAIT_BETWEEN_TRIES = int(configuration['wait_between_tries']) # in seconds
WAIT_BETWEEN_SENDING = int(configuration['wait_between_sending']) # in seconds
MAX_BUFFER_SIZE = int(configuration['max_buffer_size'])
GET_LIVE_SESSION_API_CALL = configuration['get_live_session_api_call']
POST_PACKAGE_API_CALL = configuration['post_package_api_call']
POST_SENSOR_API_CALL = configuration['post_sensor_api_call']
URL = "{0}://{1}:{2}/".format('https' if configuration['isHTTPS'] == True else 'http',
                              configuration['url'],
                              configuration['port'])

# ------------------------------------------------------------------------------------------------------------------------------

send_data = True
can_send_data = False

live_session_id = -1
live_session_name = ''

stop_sending_data = False

package_id = 1

data_index = 0

SAMPLE_DATA_DIRECTORY = "../data_files"

# ---------------------TEMPORARY DATA COLLECTION SIMULATION---------------------------------------------------------------------

# Add sensor to database and get the id of the sensor
def get_sensor_id(sensor_name):
  sensor_id = 0
  posting_sensor = True

  while posting_sensor == True:
    try:
      post_sensor_response = requests.post(URL + POST_SENSOR_API_CALL + sensor_name, verify = False)
      if post_sensor_response.status_code == HTTP_STATUS_CODE_OK:
        posting_sensor = False
      else:
        print("There was a problem posting the sensor. Trying again in " + str(WAIT_BETWEEN_TRIES) + " seconds")
        time.sleep(WAIT_BETWEEN_TRIES)
      sensor_id = int(post_sensor_response.content)
    except Exception as e:
      print("There is no connection to the server. Trying again in " + str(WAIT_BETWEEN_TRIES) + " seconds")
      time.sleep(WAIT_BETWEEN_TRIES)
  
  return sensor_id


def read_sensor_values_from_file(sensor):
  sensor_values = []

  file = os.path.join(SAMPLE_DATA_DIRECTORY, sensor.name)
  data = open(file, "r").read()
  input = data.split("\n")

  for index in range(len(input)):
    if input[index] != '':
      sensor_values.append(SensorValue(input[index], sensor.id))

  return sensor_values


DELTA_TAR = "delta_tar"
sensor1 = Sensor(get_sensor_id(DELTA_TAR), DELTA_TAR)
sensor1_values = read_sensor_values_from_file(sensor1)

VX  = "vx"
sensor2 = Sensor(get_sensor_id(VX), VX)
sensor2_values = read_sensor_values_from_file(sensor2)

if False: # use for debugging
  for s in sensor1_values:
    print(s.repr_JSON())

  print()

  for s in sensor2_values:
    print(s.repr_JSON())


# ------------------------------------------------------------------------------------------------------------------------------
'''
import matplotlib.pyplot as plt
plt.plot(speeds)
plt.savefig('speeds.png')
'''

# ---------------------CREATE PACKAGE-------------------------------------------------------------------------------------------

def prepare_sensor_values(sensor_values):
  prepared_sensor_values = []

  sensor_value_index = data_index
  for value in sensor_values:
    if len(sensor_values) > sensor_value_index:
      if sensor_value_index < data_index + MAX_BUFFER_SIZE:
        sensor_values[sensor_value_index].session_id = live_session_id
        prepared_sensor_values.append(sensor_values[sensor_value_index])
    sensor_value_index = sensor_value_index + 1

  return prepared_sensor_values


def make_package(prepared_sensor_values_list):
  package = Package(package_id, live_session_id, datetime.now().timestamp())

  for prepare_sensor_values in prepared_sensor_values_list:
    for value in prepare_sensor_values:
      package.add_sensor_value(value)

  if False: # use for debugging
    for v in package.sensor_values:
      print(v.repr_JSON())

  return package
# ------------------------------------------------------------------------------------------------------------------------------

print("----------------------------")
print("Sending sensors [",end='')
print(sensor1.name,end='')
print(", ",end='')
print(sensor2.name,end='')
print("]")
print("----------------------------")
print()

while send_data:
  while stop_sending_data == False:
  # ----------ALWAYS CHECK IF THE LIVE STATUS OF THE LIVE SESSION IS CHANGED------------------------------------------------------

    try:
      session_get_request = requests.get(URL + GET_LIVE_SESSION_API_CALL, verify = False)
      if session_get_request.status_code == HTTP_STATUS_CODE_OK:
        try:
          session_json = session_get_request.json()
          live_session_id = session_json["sessionId"]
          live_session_name = session_json["name"]
          can_send_data = True
        except Exception as e:
          print("There was a problem getting the Id of the live session. Trying again in " + str(WAIT_BETWEEN_TRIES) + " seconds. Error: " + str(e.__class__))
          time.sleep(WAIT_BETWEEN_TRIES)
      else:
        print("There is no live session at the moment. Trying again in " + str(WAIT_BETWEEN_TRIES) + " seconds")
        time.sleep(WAIT_BETWEEN_TRIES)
    except Exception as e:
      print("There is no connection to the server. Trying again in " + str(WAIT_BETWEEN_TRIES) + " seconds")
      time.sleep(WAIT_BETWEEN_TRIES)

  # ------------------------------------------------------------------------------------------------------------------------------

  # ---------------------IF THERE IS A LIVE SESSION, COLLECT DATA-----------------------------------------------------------------

    if can_send_data == True:
      prepared_sensor1_values = prepare_sensor_values(sensor1_values)
      prepared_sensor2_values = prepare_sensor_values(sensor2_values)

      package = make_package([prepared_sensor1_values, prepared_sensor2_values])

      if len(package.sensor_values) == 0:
        stop_sending_data = True
      else:
        data_index = data_index + MAX_BUFFER_SIZE

        successfull = False
        while successfull == False:
          try:
            package_json = json.dumps(package.repr_JSON(), cls=ComplexEncoder)
            send_package_response = requests.post(URL + POST_PACKAGE_API_CALL + package_json, verify = False)
            successfull = send_package_response.status_code == HTTP_STATUS_CODE_OK
          except Exception as e:
            print("There is no connection to the server. Trying again in " + str(WAIT_BETWEEN_TRIES) + " seconds")
            time.sleep(WAIT_BETWEEN_TRIES)
            
          if successfull == False:
            print("An error occurred while sending package [" + str(package_id) + "]")
            time.sleep(WAIT_BETWEEN_TRIES)
        

        if successfull == True:    
          print("Package [" + str(package_id) + "] sent successfully to session [" + live_session_name + "]")

        package_id = package_id + 1
        time.sleep(WAIT_BETWEEN_SENDING)

  send_data = False
      
# ------------------------------------------------------------------------------------------------------------------------------

print("----------------------------")
print("Stopped sending data")
print("Summary:")
print("\tAll sent packages: " + str(package_id))
print("----------------------------")

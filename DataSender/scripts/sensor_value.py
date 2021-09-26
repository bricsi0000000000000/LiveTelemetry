class SensorValue:
  def __init__(self, value, sensor_id):
    self.value = value
    self.sensor_id = sensor_id
    self.session_id  = 0

  def repr_JSON(self):
    return dict(value=self.value, sensor_id=self.sensor_id, session_id=self.session_id) 
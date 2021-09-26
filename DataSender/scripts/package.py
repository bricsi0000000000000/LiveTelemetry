class Package:
  def __init__(self, package_id, session_id, sent_time):
    self.package_id = package_id
    self.session_id = session_id
    self.sent_time = sent_time
    self.sensor_values = []

  def add_sensor_value(self, sensor_value):
    self.sensor_values.append(sensor_value)

  def repr_JSON(self):
    return dict(package_id=self.package_id, session_id=self.session_id, sent_time=self.sent_time, sensor_values=self.sensor_values) 
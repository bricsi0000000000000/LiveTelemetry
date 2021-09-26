import json

class ComplexEncoder(json.JSONEncoder):
    def default(self, obj):
        if hasattr(obj,'repr_JSON'):
            return obj.repr_JSON()
        else:
            return json.JSONEncoder.default(self, obj)